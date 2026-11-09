using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using ORICA.Datos;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    /// <summary>
    /// Lee y guarda los parámetros de dbo.Configuracion.
    /// Los demás módulos (voz, enlace ESP32) leen sus valores con mtdObtenerEntero / Decimal / Booleano / Texto.
    /// </summary>
    public class clsConfiguracionNegocio
    {
        // ---------------- Claves ----------------
        public const string UmbralConfianza = "UMBRAL_CONFIANZA";
        public const string TimeoutConfirmacion = "TIMEOUT_CONFIRMACION";
        public const string GramaticaCerrada = "GRAMATICA_CERRADA";
        public const string PttObligatorio = "PTT_OBLIGATORIO";
        public const string FeedbackVoz = "FEEDBACK_VOZ";
        public const string PuertoCom = "PUERTO_COM";
        public const string BaudRate = "BAUD_RATE";
        public const string HeartbeatMs = "HEARTBEAT_MS";
        public const string MaxHeartbeatPerdidos = "MAX_HEARTBEAT_PERDIDOS";
        public const string CrcObligatorio = "CRC_OBLIGATORIO";
        public const string SecuenciaObligatoria = "SECUENCIA_OBLIGATORIA";
        public const string WatchdogActivo = "WATCHDOG_ACTIVO";
        public const string RearmeRequerido = "REARME_REQUERIDO";
        public const string EstadoSeguroError = "ESTADO_SEGURO_ERROR";
        public const string ParadaPrioritaria = "PARADA_PRIORITARIA";
        public const string ModoOffline = "MODO_OFFLINE";
        public const string SincronizacionAutomatica = "SINCRONIZACION_AUTOMATICA";
        public const string SimularSinArduino = "SIMULAR_SIN_ARDUINO";

        // Rangos permitidos (los usa también la pantalla)
        public const int MinUmbral = 50, MaxUmbral = 100;
        public const int MinTimeout = 3, MaxTimeout = 60;
        public const int MinHeartbeat = 50, MaxHeartbeat = 10000;
        public const int MinHeartbeatPerdidos = 1, MaxHeartbeatPerdidosPermitidos = 20;
        public static readonly int[] BaudRatesPermitidos = { 9600, 19200, 38400, 57600, 115200 };

        // Definición de cada clave: categoría, tipo, valor por defecto y descripción.
        // Si una clave no existe en la BD (p. ej. FEEDBACK_VOZ), se crea al guardar.
        private static readonly Dictionary<string, string[]> Definiciones = new Dictionary<string, string[]>
        {
            { UmbralConfianza,          new[] { "VOZ", "DECIMAL", "85", "Confianza mínima global." } },
            { TimeoutConfirmacion,      new[] { "VOZ", "INTEGER", "8", "Timeout global de confirmación." } },
            { GramaticaCerrada,         new[] { "VOZ", "BOOLEAN", "true", "Restringe el reconocimiento a comandos autorizados." } },
            { PttObligatorio,           new[] { "VOZ", "BOOLEAN", "true", "PTT obligatorio." } },
            { FeedbackVoz,              new[] { "VOZ", "BOOLEAN", "true", "La aplicación responde por voz al operador." } },
            { PuertoCom,                new[] { "COMUNICACION", "STRING", "COM3", "Puerto serial del Arduino." } },
            { BaudRate,                 new[] { "COMUNICACION", "INTEGER", "115200", "Velocidad serial." } },
            { HeartbeatMs,              new[] { "COMUNICACION", "INTEGER", "250", "Intervalo de heartbeat." } },
            { MaxHeartbeatPerdidos,     new[] { "COMUNICACION", "INTEGER", "3", "Máximo de heartbeat perdidos." } },
            { CrcObligatorio,           new[] { "COMUNICACION", "BOOLEAN", "true", "CRC obligatorio." } },
            { SecuenciaObligatoria,     new[] { "COMUNICACION", "BOOLEAN", "true", "Secuencia obligatoria." } },
            { WatchdogActivo,           new[] { "SEGURIDAD", "BOOLEAN", "true", "Watchdog activo." } },
            { RearmeRequerido,          new[] { "SEGURIDAD", "BOOLEAN", "true", "Rearme explícito después de parada." } },
            { EstadoSeguroError,        new[] { "SEGURIDAD", "BOOLEAN", "true", "Ante un error, el prototipo pasa a estado seguro." } },
            { ParadaPrioritaria,        new[] { "SEGURIDAD", "BOOLEAN", "true", "La parada tiene prioridad sobre cualquier comando." } },
            { ModoOffline,              new[] { "PERSISTENCIA", "BOOLEAN", "true", "Permite trabajo offline." } },
            { SincronizacionAutomatica, new[] { "PERSISTENCIA", "BOOLEAN", "true", "Sincronización automática." } },
            { SimularSinArduino,        new[] { "COMUNICACION", "BOOLEAN", "false", "Si no hay Arduino conectado, los comandos se simulan." } }
        };

        private readonly clsConfiguracionDatos _datos = new clsConfiguracionDatos();

        // ================= CONSULTAS =================

        /// <summary>Todos los parámetros activos, por clave. Las claves que faltan en la BD vienen con su valor por defecto.</summary>
        public Dictionary<string, ParametroConfiguracion> mtdListar()
        {
            Dictionary<string, ParametroConfiguracion> lista = new Dictionary<string, ParametroConfiguracion>(StringComparer.OrdinalIgnoreCase);

            foreach (DataRow r in _datos.mtdListar().Rows)
            {
                ParametroConfiguracion p = new ParametroConfiguracion
                {
                    IdConfiguracion = clsLectura.mtdLeerEntero(r, "IdConfiguracion"),
                    Categoria = clsLectura.mtdLeerTexto(r, "Categoria"),
                    Clave = clsLectura.mtdLeerTexto(r, "Clave"),
                    Valor = clsLectura.mtdLeerTexto(r, "Valor"),
                    TipoDato = clsLectura.mtdLeerTexto(r, "TipoDato"),
                    Descripcion = clsLectura.mtdLeerTexto(r, "Descripcion"),
                    EsEditable = clsLectura.mtdLeerBooleano(r, "EsEditable"),
                    FechaModificacion = clsLectura.mtdLeerFecha(r, "FechaModificacion")
                };
                lista[p.Clave] = p;
            }

            foreach (KeyValuePair<string, string[]> d in Definiciones)
            {
                if (!lista.ContainsKey(d.Key))
                {
                    lista[d.Key] = new ParametroConfiguracion
                    {
                        Categoria = d.Value[0], Clave = d.Key, TipoDato = d.Value[1],
                        Valor = d.Value[2], Descripcion = d.Value[3], EsEditable = true
                    };
                }
            }

            return lista;
        }

        public string mtdObtenerTexto(string clave)
        {
            try
            {
                ParametroConfiguracion p;
                if (mtdListar().TryGetValue(clave, out p) && !string.IsNullOrWhiteSpace(p.Valor)) return p.Valor.Trim();
            }
            catch (Exception) { /* sin BD se usa el valor por defecto */ }

            string[] def;
            return Definiciones.TryGetValue(clave, out def) ? def[2] : null;
        }

        public int mtdObtenerEntero(string clave)
        {
            int valor;
            return int.TryParse(mtdObtenerTexto(clave), NumberStyles.Integer, CultureInfo.InvariantCulture, out valor) ? valor : 0;
        }

        public decimal mtdObtenerDecimal(string clave)
        {
            decimal valor;
            return decimal.TryParse(mtdObtenerTexto(clave), NumberStyles.Number, CultureInfo.InvariantCulture, out valor) ? valor : 0m;
        }

        public bool mtdObtenerBooleano(string clave)
        {
            return string.Equals(mtdObtenerTexto(clave), "true", StringComparison.OrdinalIgnoreCase);
        }

        // ================= ESCRITURA =================

        /// <summary>
        /// Valida y guarda los valores (clave → valor en texto). Solo escribe los que cambiaron
        /// y deja un registro de auditoría con el valor anterior y el nuevo.
        /// </summary>
        public Resultado mtdGuardar(Dictionary<string, string> valores)
        {
            try
            {
                if (valores == null || valores.Count == 0) return Resultado.Error("No hay valores para guardar.");

                string error = mtdValidar(valores);
                if (error != null) return Resultado.Error(error);

                Dictionary<string, ParametroConfiguracion> actuales = mtdListar();
                List<string[]> cambios = new List<string[]>();
                List<string> antes = new List<string>(), despues = new List<string>();

                foreach (KeyValuePair<string, string> v in valores)
                {
                    ParametroConfiguracion actual;
                    actuales.TryGetValue(v.Key, out actual);

                    bool existeEnBd = actual != null && actual.IdConfiguracion > 0;
                    if (existeEnBd && !actual.EsEditable) continue;
                    if (existeEnBd && string.Equals(actual.Valor, v.Value, StringComparison.OrdinalIgnoreCase)) continue;

                    string[] def;
                    Definiciones.TryGetValue(v.Key, out def);
                    cambios.Add(new[]
                    {
                        actual != null ? actual.Categoria : (def != null ? def[0] : "GENERAL"),
                        v.Key,
                        v.Value,
                        actual != null ? actual.TipoDato : (def != null ? def[1] : "STRING"),
                        actual != null ? actual.Descripcion : (def != null ? def[3] : null)
                    });

                    antes.Add(v.Key + "=" + (existeEnBd ? actual.Valor : "(no existía)"));
                    despues.Add(v.Key + "=" + v.Value);
                }

                if (cambios.Count == 0) return Resultado.Ok("No había cambios que guardar.");

                int? idUsuario = Sesion.Activa ? (int?)Sesion.UsuarioActual.IdUsuario : null;
                _datos.mtdGuardar(cambios, idUsuario);

                clsAuditoriaNegocio.mtdRegistrar(clsAuditoriaNegocio.ModuloConfiguracion, "MODIFICAR", "Configuracion",
                    null, cambios.Count + " parámetro(s) modificados.",
                    string.Join("; ", antes), string.Join("; ", despues));

                return Resultado.Ok("Configuración guardada (" + cambios.Count + " cambio(s)).");
            }
            catch (SqlException ex) { return Resultado.Error("Error de base de datos: " + ex.Message); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        // ================= REGLAS =================

        private static string mtdValidar(Dictionary<string, string> v)
        {
            string error;
            if ((error = mtdRango(v, UmbralConfianza, MinUmbral, MaxUmbral, "El umbral de confianza")) != null) return error;
            if ((error = mtdRango(v, TimeoutConfirmacion, MinTimeout, MaxTimeout, "El timeout de confirmación")) != null) return error;
            if ((error = mtdRango(v, HeartbeatMs, MinHeartbeat, MaxHeartbeat, "El intervalo de heartbeat")) != null) return error;
            if ((error = mtdRango(v, MaxHeartbeatPerdidos, MinHeartbeatPerdidos, MaxHeartbeatPerdidosPermitidos, "El máximo de heartbeat perdidos")) != null) return error;

            string baud;
            if (v.TryGetValue(BaudRate, out baud))
            {
                int b;
                if (!int.TryParse(baud, out b) || !BaudRatesPermitidos.Contains(b))
                    return "Seleccione un baud rate válido.";
            }

            string puerto;
            if (v.TryGetValue(PuertoCom, out puerto))
            {
                puerto = (puerto ?? string.Empty).Trim().ToUpperInvariant();
                if (!puerto.StartsWith("COM") || puerto.Length < 4 || !puerto.Substring(3).All(char.IsDigit))
                    return "Seleccione un puerto serial válido (por ejemplo COM3).";
                v[PuertoCom] = puerto;
            }

            // Por seguridad, estas opciones no se pueden apagar desde la pantalla.
            if (mtdEsFalso(v, ParadaPrioritaria)) return "La parada prioritaria no se puede desactivar.";
            if (mtdEsFalso(v, EstadoSeguroError)) return "El estado seguro ante error no se puede desactivar.";

            return null;
        }

        private static string mtdRango(Dictionary<string, string> v, string clave, int min, int max, string nombre)
        {
            string texto;
            if (!v.TryGetValue(clave, out texto)) return null;

            decimal valor;
            if (!decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out valor) || valor < min || valor > max)
                return nombre + " debe estar entre " + min + " y " + max + ".";

            return null;
        }

        private static bool mtdEsFalso(Dictionary<string, string> v, string clave)
        {
            string texto;
            return v.TryGetValue(clave, out texto) && string.Equals(texto, "false", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>Convierte un valor a texto como se guarda en la BD (punto decimal, true/false).</summary>
        public static string mtdATexto(object valor)
        {
            if (valor is bool) return (bool)valor ? "true" : "false";
            if (valor is decimal) return ((decimal)valor).ToString("0.##", CultureInfo.InvariantCulture);
            return Convert.ToString(valor, CultureInfo.InvariantCulture);
        }
    }
}
