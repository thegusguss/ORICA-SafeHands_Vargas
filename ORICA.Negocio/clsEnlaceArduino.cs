using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    /// <summary>
    /// Enlace serial (USB) con el Arduino Uno. Corre solo mientras la app está abierta:
    /// - abre el puerto de Configuración (PUERTO_COM / BAUD_RATE) y se reconecta si se desconecta,
    /// - envía PING cada HEARTBEAT_MS y cuenta las respuestas perdidas (alerta si llega a MAX_HEARTBEAT_PERDIDOS),
    /// - envía las órdenes y espera el ACK/NAK del Arduino,
    /// - recibe los avisos del Arduino (parada física, watchdog) y los registra.
    /// El protocolo está en clsProtocoloArduino y en Arduino/ORICA_SafeHands/ORICA_SafeHands.ino.
    /// </summary>
    public static class clsEnlaceArduino
    {
        public enum EstadoEnlace { SinConfigurar, Desconectado, Conectando, Conectado, SinHeartbeat }

        private const int EsperaRespuestaMs = 1500;  // tiempo máximo para el ACK/NAK de una orden
        private const int EsperaListoMs = 4000;      // al abrir el puerto el Uno se reinicia y tarda ~2 s
        private const int ReintentoMs = 2000;        // cada cuánto se intenta reconectar
        private const int RecargarConfigMs = 15000;  // cada cuánto se vuelve a leer la configuración
        private const int MaximoLog = 200;

        private static readonly object _candadoOrden = new object();     // una orden a la vez
        private static readonly object _candadoEscritura = new object(); // una línea a la vez en el puerto
        private static readonly AutoResetEvent _respuestaLlego = new AutoResetEvent(false);
        private static readonly List<string> _log = new List<string>();

        private static SerialPort _puerto;
        private static Timer _temporizador;
        private static int _trabajando;           // evita que dos vueltas del temporizador se crucen
        private static string _buffer = "";
        private static long _secuencia;
        private static long _esperaSecuencia;
        private static TramaArduino _respuesta;
        private static bool _esperandoPong;
        private static DateTime _envioPing, _abiertoEn, _ultimoReintento, _ultimaConfig;
        private static bool _listo;               // el Arduino avisó LISTO (o pasó el tiempo de espera)
        private static bool _crcObligatorio = true, _secuenciaObligatoria = true, _watchdogActivo = true, _rearmeRequerido = true;

        // ---------------- Estado (lo leen las pantallas) ----------------

        public static EstadoEnlace Estado { get; private set; }
        public static string Puerto { get; private set; }
        public static int Baudios { get; private set; }
        public static int HeartbeatMs { get; private set; }
        public static int MaxPerdidos { get; private set; }
        public static int Perdidos { get; private set; }
        public static int LatenciaMs { get; private set; }
        public static bool HeartbeatPausado { get; private set; }
        public static string Modo { get; private set; } = "";   // NORMAL, PARADA o WATCHDOG (lo dice el Arduino)
        public static string IdArduino { get; private set; } = "";
        public static int Enviadas { get; private set; }
        public static int Recibidas { get; private set; }
        public static int ErroresCrc { get; private set; }

        // Salidas del Arduino según su última respuesta (ACK).
        public static bool SalidasConocidas { get; private set; }
        public static bool Indicador { get; private set; }
        public static bool Simulador { get; private set; }
        public static bool Critica { get; private set; }

        public static bool Conectado { get { return Estado == EstadoEnlace.Conectado; } }
        public static bool RequiereRearme { get { return Modo == "PARADA" || Modo == "WATCHDOG"; } }

        public static string TextoEstado
        {
            get
            {
                switch (Estado)
                {
                    case EstadoEnlace.SinConfigurar: return "● Arduino sin configurar";
                    case EstadoEnlace.Conectando: return "● Conectando Arduino (" + Puerto + ")";
                    case EstadoEnlace.Conectado: return RequiereRearme ? "● Arduino en " + Modo : "● Arduino en " + Puerto;
                    case EstadoEnlace.SinHeartbeat: return "● Arduino sin respuesta (" + Puerto + ")";
                    default: return "● Arduino desconectado (" + Puerto + ")";
                }
            }
        }

        /// <summary>Últimas líneas enviadas (→) y recibidas (←), para la pantalla Diagnóstico.</summary>
        public static string[] mtdLog()
        {
            lock (_log) return _log.ToArray();
        }

        // ---------------- Inicio / fin ----------------

        public static void mtdIniciar()
        {
            if (_temporizador != null) return;
            LeerConfiguracion();
            _temporizador = new Timer(_ => Revisar(), null, 0, HeartbeatMs);
        }

        public static void mtdDetener()
        {
            if (_temporizador != null) _temporizador.Dispose();
            _temporizador = null;
            Cerrar(null);
        }

        // ---------------- Lo que se hace en cada vuelta ----------------

        private static void Revisar()
        {
            if (Interlocked.Exchange(ref _trabajando, 1) == 1) return;
            try
            {
                // La configuración se relee aparte, para que una consulta lenta (Azure) no frene el heartbeat.
                if ((DateTime.Now - _ultimaConfig).TotalMilliseconds > RecargarConfigMs)
                {
                    _ultimaConfig = DateTime.Now;
                    ThreadPool.QueueUserWorkItem(_ => LeerConfiguracion());
                }

                if (_puerto == null)
                {
                    if ((DateTime.Now - _ultimoReintento).TotalMilliseconds > ReintentoMs) Abrir();
                    return;
                }

                if (!PuertoExiste(Puerto))
                {
                    Cerrar("el puerto desapareció (cable USB desconectado o Arduino apagado)");
                    return;
                }

                if (Estado == EstadoEnlace.Conectando)
                {
                    // Cuando el Arduino avisa LISTO (o pasa el tiempo), se le envía la configuración.
                    if (_listo || (DateTime.Now - _abiertoEn).TotalMilliseconds > EsperaListoMs) EnviarConfiguracion();
                    return;
                }

                Heartbeat();
            }
            catch (Exception ex)
            {
                AgregarLog("! " + ex.Message);
            }
            finally
            {
                Interlocked.Exchange(ref _trabajando, 0);
            }
        }

        private static void Heartbeat()
        {
            // Si el PING anterior no tuvo respuesta, cuenta como perdido.
            // Con "Perder heartbeat" (prueba) no se envía nada y también se cuenta.
            if (_esperandoPong || HeartbeatPausado) Perdidos++;

            if (Perdidos >= MaxPerdidos && Estado == EstadoEnlace.Conectado)
            {
                Estado = EstadoEnlace.SinHeartbeat;
                new clsAlertaNegocio().mtdAlertaEnlace(Puerto,
                    "se perdieron " + Perdidos + " heartbeat seguidos" + (HeartbeatPausado ? " (prueba desde Diagnóstico)" : ""));
            }

            if (HeartbeatPausado) return;

            _esperandoPong = true;
            _envioPing = DateTime.Now;
            Escribir(clsProtocoloArduino.mtdArmar(0, "PING"), false);   // el PING no usa secuencia
        }

        // ---------------- Abrir / cerrar el puerto ----------------

        private static void Abrir()
        {
            _ultimoReintento = DateTime.Now;
            if (string.IsNullOrWhiteSpace(Puerto)) { Estado = EstadoEnlace.SinConfigurar; return; }
            if (!PuertoExiste(Puerto)) { Estado = EstadoEnlace.Desconectado; return; }

            try
            {
                SerialPort puerto = new SerialPort(Puerto, Baudios) { NewLine = "\n", DtrEnable = true, WriteTimeout = 500 };
                puerto.DataReceived += (s, e) => Recibir();
                puerto.Open();

                _puerto = puerto;
                _buffer = "";
                _secuencia = 0;              // el Arduino se reinicia al abrir: la secuencia vuelve a empezar
                _listo = false;
                _esperandoPong = false;
                Perdidos = 0;
                Modo = "";
                SalidasConocidas = false;
                _abiertoEn = DateTime.Now;
                Estado = EstadoEnlace.Conectando;
                AgregarLog("· Puerto " + Puerto + " abierto a " + Baudios + " baudios. Esperando al Arduino...");
            }
            catch (Exception ex)
            {
                Estado = EstadoEnlace.Desconectado;
                AgregarLog("! No se pudo abrir " + Puerto + ": " + ex.Message);
            }
        }

        // motivo: si no es null, se perdió la conexión sin querer y se genera la alerta.
        private static void Cerrar(string motivo)
        {
            SerialPort puerto = _puerto;
            _puerto = null;
            if (puerto != null)
            {
                try { puerto.Close(); } catch (Exception) { }
                puerto.Dispose();
            }

            bool estabaConectado = Estado == EstadoEnlace.Conectado || Estado == EstadoEnlace.SinHeartbeat;
            Estado = string.IsNullOrWhiteSpace(Puerto) ? EstadoEnlace.SinConfigurar : EstadoEnlace.Desconectado;
            Modo = "";
            _respuestaLlego.Set();   // si había una orden esperando, deja de esperar

            if (motivo != null)
            {
                AgregarLog("! Enlace cerrado: " + motivo);
                if (estabaConectado) new clsAlertaNegocio().mtdAlertaEnlace(Puerto, motivo);
            }
        }

        /// <summary>Cierra y vuelve a abrir el puerto (botón "Recuperar enlace"). También reanuda el heartbeat.</summary>
        public static void mtdReconectar()
        {
            HeartbeatPausado = false;
            LeerConfiguracion();
            Cerrar(null);
            _ultimoReintento = DateTime.MinValue;   // se reabre en la siguiente vuelta
        }

        /// <summary>Prueba de seguridad: deja de enviar heartbeat para que el watchdog del Arduino actúe.</summary>
        public static void mtdPausarHeartbeat(bool pausar)
        {
            HeartbeatPausado = pausar;
            if (!pausar) Perdidos = 0;
            AgregarLog(pausar ? "· Heartbeat detenido (prueba)." : "· Heartbeat reanudado.");
        }

        // ---------------- Enviar ----------------

        private static void EnviarConfiguracion()
        {
            // El watchdog del Arduino actúa si no recibe nada en (heartbeat x perdidos), con un mínimo de 1 s.
            int watchdogMs = Math.Max(1000, HeartbeatMs * MaxPerdidos);
            RespuestaArduino r = Enviar("CONFIG", watchdogMs.ToString(), Bit(_watchdogActivo), Bit(_crcObligatorio),
                                        Bit(_secuenciaObligatoria), Bit(_rearmeRequerido));
            if (r.Exito)
            {
                Estado = EstadoEnlace.Conectado;
                Perdidos = 0;
                AgregarLog("· Arduino listo. Watchdog: " + watchdogMs + " ms.");
            }
            else
            {
                AgregarLog("! El Arduino no respondió a CONFIG (" + r.Motivo + "). ¿Está cargado ORICA_SafeHands.ino?");
                _abiertoEn = DateTime.Now;   // se vuelve a intentar en unos segundos
                _listo = false;
            }
        }

        /// <summary>
        /// Envía una orden a un equipo y espera la respuesta. tipo: ESTADO, INDICADOR, OPERAR, CRITICA, DETENER o PARADA.
        /// idHardware: el "Identificador de hardware" del equipo (o "*" para todos).
        /// </summary>
        public static RespuestaArduino mtdEnviarOrden(string tipo, string idHardware)
        {
            if (Estado != EstadoEnlace.Conectado && Estado != EstadoEnlace.SinHeartbeat)
                return RespuestaArduino.Fallo(Estado == EstadoEnlace.Conectando ? "El Arduino se está conectando, intente en unos segundos."
                                                                                 : "Arduino no conectado (" + (Puerto ?? "sin puerto") + ").");
            return Enviar(tipo, string.IsNullOrWhiteSpace(idHardware) ? "*" : idHardware.Trim());
        }

        /// <summary>Sale del estado de PARADA o WATCHDOG (rearme explícito).</summary>
        public static RespuestaArduino mtdRearmar()
        {
            if (Estado != EstadoEnlace.Conectado)
                return RespuestaArduino.Fallo("Arduino no conectado o sin heartbeat.");
            return Enviar("REARME");
        }

        // Envía una trama con secuencia nueva y espera su ACK/NAK.
        private static RespuestaArduino Enviar(string tipo, params string[] datos)
        {
            lock (_candadoOrden)
            {
                if (_puerto == null) return RespuestaArduino.Fallo("Puerto cerrado.");

                long secuencia = Interlocked.Increment(ref _secuencia);
                string trama = clsProtocoloArduino.mtdArmar(secuencia, tipo, datos);
                string crc = trama.Substring(trama.LastIndexOf('*') + 1);

                _esperaSecuencia = secuencia;
                _respuesta = null;
                _respuestaLlego.Reset();
                DateTime inicio = DateTime.Now;

                if (!Escribir(trama, true)) return RespuestaArduino.Fallo("No se pudo escribir en el puerto.", secuencia, crc);

                if (!_respuestaLlego.WaitOne(EsperaRespuestaMs) || _respuesta == null)
                    return RespuestaArduino.Fallo("El Arduino no respondió a tiempo.", secuencia, crc);

                TramaArduino r = _respuesta;
                RespuestaArduino resultado = new RespuestaArduino
                {
                    Exito = r.Tipo == "ACK",
                    Secuencia = secuencia,
                    Crc = crc,
                    LatenciaMs = (int)(DateTime.Now - inicio).TotalMilliseconds,
                    // ACK,<tipo>,<modo>,<indicador>,<simulador>,<critica>   |   NAK,<tipo>,<motivo>
                    Motivo = r.Tipo == "NAK" ? r.Dato(1) : "",
                    Modo = r.Tipo == "ACK" ? r.Dato(1) : Modo,
                    Indicador = r.Dato(2) == "1",
                    Simulador = r.Dato(3) == "1",
                    Critica = r.Dato(4) == "1"
                };
                if (resultado.Exito)
                {
                    Modo = resultado.Modo;
                    Indicador = resultado.Indicador;
                    Simulador = resultado.Simulador;
                    Critica = resultado.Critica;
                    SalidasConocidas = true;
                }
                return resultado;
            }
        }

        private static bool Escribir(string trama, bool contar)
        {
            try
            {
                lock (_candadoEscritura)
                {
                    SerialPort puerto = _puerto;
                    if (puerto == null) return false;
                    puerto.Write(trama + "\n");
                }
                if (contar) { Enviadas++; AgregarLog("→ " + trama); }
                return true;
            }
            catch (Exception ex)
            {
                Cerrar("error al escribir en el puerto: " + ex.Message);
                return false;
            }
        }

        // ---------------- Recibir ----------------

        // Se ejecuta en otro hilo cada vez que llegan bytes. Se juntan hasta tener líneas completas.
        private static void Recibir()
        {
            try
            {
                SerialPort puerto = _puerto;
                if (puerto == null) return;
                _buffer += puerto.ReadExisting();

                int fin;
                while ((fin = _buffer.IndexOf('\n')) >= 0)
                {
                    string linea = _buffer.Substring(0, fin).Trim();
                    _buffer = _buffer.Substring(fin + 1);
                    if (linea.Length > 0) Procesar(linea);
                }
                if (_buffer.Length > 500) _buffer = "";   // basura sin saltos de línea
            }
            catch (Exception ex)
            {
                AgregarLog("! Error al leer: " + ex.Message);
            }
        }

        private static void Procesar(string linea)
        {
            TramaArduino t = clsProtocoloArduino.mtdLeer(linea);
            if (t == null) { AgregarLog("? " + linea); return; }   // texto que no es trama (p. ej. prints de prueba)

            Recibidas++;
            if (t.Tipo != "PONG") AgregarLog("← " + linea);

            if (!t.CrcValido && _crcObligatorio)
            {
                ErroresCrc++;
                AgregarLog("! CRC incorrecto, se ignora: " + linea);
                return;
            }

            switch (t.Tipo)
            {
                case "PONG":
                    _esperandoPong = false;
                    Perdidos = 0;
                    LatenciaMs = (int)(DateTime.Now - _envioPing).TotalMilliseconds;
                    Modo = t.Dato(0);
                    if (Estado == EstadoEnlace.SinHeartbeat)
                    {
                        Estado = EstadoEnlace.Conectado;
                        AgregarLog("· Heartbeat recuperado." + (RequiereRearme ? " El Arduino espera REARME." : ""));
                    }
                    break;

                case "ACK":
                case "NAK":
                    if (t.Secuencia == _esperaSecuencia)
                    {
                        _respuesta = t;
                        _respuestaLlego.Set();
                    }
                    break;

                case "EVT":
                    ProcesarAviso(t);
                    break;
            }
        }

        // Avisos que el Arduino envía por su cuenta.
        private static void ProcesarAviso(TramaArduino t)
        {
            string aviso = t.Dato(0);
            string idHardware = t.Dato(1);

            if (aviso == "LISTO")
            {
                IdArduino = idHardware;
                _listo = true;
                return;
            }

            // Se registra en otro hilo para no frenar la lectura del puerto.
            if (aviso == "PARADA_FISICA")
            {
                Modo = "PARADA";
                Indicador = Simulador = Critica = false;
                ThreadPool.QueueUserWorkItem(_ => RegistrarAviso(clsEventoNegocio.TipoParada, idHardware,
                    "Se presionó el pulsador de parada del equipo " + idHardware + "."));
            }
            else if (aviso == "WATCHDOG")
            {
                Modo = _rearmeRequerido ? "WATCHDOG" : "NORMAL";
                Indicador = Simulador = Critica = false;
                ThreadPool.QueueUserWorkItem(_ => RegistrarAviso(clsEventoNegocio.TipoWatchdog, idHardware,
                    "El watchdog del Arduino " + idHardware + " pasó a estado seguro: no recibió mensajes de la PC."));
            }
        }

        private static void RegistrarAviso(string tipoEvento, string idHardware, string detalle)
        {
            try
            {
                Equipo equipo = new clsEquipoNegocio().mtdListarActivos()
                    .FirstOrDefault(e => string.Equals(e.IdentificadorHardware, idHardware, StringComparison.OrdinalIgnoreCase));

                Guid id = new clsEventoNegocio().mtdRegistrar(tipoEvento, clsEventoNegocio.ResultadoEjecutado, detalle,
                    equipo == null ? (int?)null : equipo.IdEquipo);
                new clsAlertaNegocio().mtdAlertaSeguridad(tipoEvento, detalle, id);
            }
            catch (Exception ex)
            {
                AgregarLog("! No se pudo registrar el aviso: " + ex.Message);
            }
        }

        // ---------------- Ayudas ----------------

        // Lee la configuración con una sola consulta. Si la BD no responde, se mantiene lo que había.
        private static void LeerConfiguracion()
        {
            _ultimaConfig = DateTime.Now;
            Dictionary<string, ParametroConfiguracion> p;
            try { p = new clsConfiguracionNegocio().mtdListar(); }
            catch (Exception) { return; }

            Func<string, string> valor = clave =>
            {
                ParametroConfiguracion x;
                return p.TryGetValue(clave, out x) && !string.IsNullOrWhiteSpace(x.Valor) ? x.Valor.Trim() : null;
            };
            Func<string, int, int> entero = (clave, defecto) =>
            {
                int n;
                return int.TryParse(valor(clave), out n) && n > 0 ? n : defecto;
            };
            Func<string, bool> si = clave => !string.Equals(valor(clave), "false", StringComparison.OrdinalIgnoreCase);

            string puerto = (valor(clsConfiguracionNegocio.PuertoCom) ?? "").ToUpperInvariant();
            int baudios = entero(clsConfiguracionNegocio.BaudRate, 115200);
            HeartbeatMs = Math.Max(100, entero(clsConfiguracionNegocio.HeartbeatMs, 250));
            MaxPerdidos = entero(clsConfiguracionNegocio.MaxHeartbeatPerdidos, 3);
            _crcObligatorio = si(clsConfiguracionNegocio.CrcObligatorio);
            _secuenciaObligatoria = si(clsConfiguracionNegocio.SecuenciaObligatoria);
            _watchdogActivo = si(clsConfiguracionNegocio.WatchdogActivo);
            _rearmeRequerido = si(clsConfiguracionNegocio.RearmeRequerido);

            if (_temporizador != null) _temporizador.Change(HeartbeatMs, HeartbeatMs);

            // Si cambiaron el puerto o la velocidad en Configuración, se reconecta.
            if (_puerto != null && (puerto != Puerto || baudios != Baudios)) Cerrar(null);
            Puerto = puerto;
            Baudios = baudios;
            if (_puerto == null) Estado = string.IsNullOrWhiteSpace(Puerto) ? EstadoEnlace.SinConfigurar : EstadoEnlace.Desconectado;
        }

        private static bool PuertoExiste(string puerto)
        {
            try { return SerialPort.GetPortNames().Any(p => string.Equals(p, puerto, StringComparison.OrdinalIgnoreCase)); }
            catch (Exception) { return false; }
        }

        private static string Bit(bool valor)
        {
            return valor ? "1" : "0";
        }

        private static void AgregarLog(string texto)
        {
            lock (_log)
            {
                _log.Add(DateTime.Now.ToString("HH:mm:ss.fff") + "  " + texto);
                if (_log.Count > MaximoLog) _log.RemoveAt(0);
            }
        }
    }

    /// <summary>Resultado de una orden enviada al Arduino.</summary>
    public class RespuestaArduino
    {
        public bool Exito { get; set; }
        public string Motivo { get; set; }      // si falló: CRC, SECUENCIA, EQUIPO, PARADA, WATCHDOG, DESCONOCIDO o un texto
        public long Secuencia { get; set; }     // de la trama enviada (se guarda en dbo.Evento)
        public string Crc { get; set; }
        public int LatenciaMs { get; set; }
        public string Modo { get; set; }
        public bool Indicador { get; set; }
        public bool Simulador { get; set; }
        public bool Critica { get; set; }

        public static RespuestaArduino Fallo(string motivo, long secuencia = 0, string crc = null)
        {
            return new RespuestaArduino { Exito = false, Motivo = motivo, Secuencia = secuencia, Crc = crc };
        }

        /// <summary>Explicación del motivo para el operador.</summary>
        public string MotivoTexto
        {
            get
            {
                switch (Motivo)
                {
                    case "PARADA": return "El equipo está en PARADA. Mantenimiento debe rearmarlo (Diagnóstico → Rearmar sistema).";
                    case "WATCHDOG": return "El equipo pasó a estado seguro por pérdida de enlace. Mantenimiento debe rearmarlo.";
                    case "EQUIPO": return "El Arduino no corresponde a este equipo (revise el identificador de hardware).";
                    case "CRC": return "El Arduino recibió la orden dañada (CRC incorrecto).";
                    case "SECUENCIA": return "El Arduino rechazó la orden por secuencia repetida.";
                    case "DESCONOCIDO": return "El Arduino no conoce esta orden.";
                    default: return Motivo;
                }
            }
        }
    }
}
