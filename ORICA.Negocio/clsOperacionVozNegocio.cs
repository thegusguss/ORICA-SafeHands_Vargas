using System;
using System.Collections.Generic;
using System.Linq;
using ORICA.Datos;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    // Reglas de la operación por voz (sin nada de pantalla ni micrófono):
    // 1) interpretar lo que se escuchó contra la gramática cerrada del equipo,
    // 2) llevar los pasos de confirmación según el nivel de criticidad,
    // 3) ejecutar en el Arduino (clsEnlaceArduino) o cancelar,
    // 4) dejar todo registrado en dbo.Evento y dbo.Confirmacion.
    public class clsOperacionVozNegocio
    {
        public const string PalabraConfirmar = "CONFIRMAR";
        public const string PalabraCancelar = "CANCELAR";
        private const string RespuestaTimeout = "TIMEOUT";

        private readonly clsComandoNegocio _comandos = new clsComandoNegocio();
        private readonly clsEquipoNegocio _equipos = new clsEquipoNegocio();
        private readonly clsConfiguracionNegocio _configuracion = new clsConfiguracionNegocio();
        private readonly clsEventoNegocio _eventos = new clsEventoNegocio();
        private readonly clsEventoDatos _eventoDatos = new clsEventoDatos();
        private readonly clsAlertaNegocio _alertas = new clsAlertaNegocio();

        //CONSULTAS

        public List<Equipo> mtdListarEquipos()
        {
            return _equipos.mtdListarActivos();
        }
        public List<Comando> mtdListarComandos(int idEquipo)
        {
            return _comandos.mtdListarPorEquipo(idEquipo);
        }

        public decimal mtdUmbralGlobal()
        {
            decimal umbral = _configuracion.mtdObtenerDecimal(clsConfiguracionNegocio.UmbralConfianza);
            return umbral <= 0 ? 85m : umbral;
        }

        public bool mtdPttObligatorio()
        {
            return _configuracion.mtdObtenerBooleano(clsConfiguracionNegocio.PttObligatorio);
        }

        public bool mtdFeedbackVoz()
        {
            return _configuracion.mtdObtenerBooleano(clsConfiguracionNegocio.FeedbackVoz);
        }

        // ================= 1) INTERPRETAR =================

        /// <summary>
        /// Busca la frase en los comandos del equipo y revisa la confianza.
        /// confianza: 0 a 100 (el reconocedor da 0 a 1; la pantalla lo multiplica por 100).
        /// </summary>
        public InterpretacionVoz mtdInterpretar(IList<Comando> comandos, string texto, decimal confianza)
        {
            InterpretacionVoz r = new InterpretacionVoz
            {
                TextoReconocido = texto,
                Confianza = Math.Max(0m, Math.Min(100m, Math.Round(confianza, 1)))
            };

            string frase = clsComandoNegocio.mtdNormalizarFrase(texto);
            r.Comando = (comandos ?? new List<Comando>()).FirstOrDefault(c => c.Frase == frase);

            if (r.Comando == null)
            {
                r.Motivo = "La frase no es un comando autorizado para este equipo.";
                return r;
            }

            r.ConfianzaRequerida = Math.Max(r.Comando.ConfianzaMinima, mtdUmbralGlobal());
            if (r.Confianza < r.ConfianzaRequerida)
            {
                r.Motivo = "Confianza insuficiente (" + r.Confianza.ToString("0.#") + " % de " +
                           r.ConfianzaRequerida.ToString("0.#") + " % requerido). Repita el comando.";
                return r;
            }

            r.Aceptado = true;
            return r;
        }

        /// <summary>Registra en Evento un intento rechazado (frase desconocida o poca confianza).</summary>
        public void mtdRegistrarRechazo(InterpretacionVoz r, Equipo equipo)
        {
            Guid id = _eventos.mtdRegistrar(
                r.Comando == null ? clsEventoNegocio.TipoNoReconocido : clsEventoNegocio.TipoComandoVoz,
                clsEventoNegocio.ResultadoRechazado, r.Motivo,
                equipo == null ? (int?)null : equipo.IdEquipo,
                r.Comando == null ? (int?)null : r.Comando.IdComando,
                r.Comando == null ? (int?)null : r.Comando.IdNivelCriticidad,
                r.TextoReconocido, r.Confianza);

            // N3/N4 rechazado → alerta (para N1/N2 o frases desconocidas no se genera).
            _alertas.mtdAlertaComando(clsAlertaNegocio.TipoComandoRechazado, r.Comando, equipo, id, r.Motivo);
        }

        // ================= 2) CONFIRMACIONES =================

        /// <summary>Crea la operación pendiente. Si el nivel no pide confirmación, ya está lista para ejecutar.</summary>
        public OperacionPendiente mtdIniciar(InterpretacionVoz r, Equipo equipo)
        {
            int timeout = r.Comando.TimeoutSegundos;
            if (timeout <= 0) timeout = _configuracion.mtdObtenerEntero(clsConfiguracionNegocio.TimeoutConfirmacion);
            if (timeout <= 0) timeout = 8;

            return new OperacionPendiente
            {
                Comando = r.Comando,
                Equipo = equipo,
                TextoReconocido = r.TextoReconocido,
                Confianza = r.Confianza,
                TimeoutSegundos = timeout,
                PasoActual = 1,
                InicioPaso = DateTime.Now
            };
        }

        /// <summary>
        /// Anota la respuesta del paso actual. respuesta: CONFIRMAR, CANCELAR o TIMEOUT.
        /// Devuelve si falta otro paso, si ya se puede ejecutar o si quedó cancelada.
        /// </summary>
        public EstadoConfirmacion mtdResponder(OperacionPendiente p, string respuesta)
        {
            respuesta = (respuesta ?? string.Empty).Trim().ToUpperInvariant();

            p.Respuestas.Add(new RespuestaConfirmacion
            {
                NumeroPaso = p.PasoActual,
                Respuesta = respuesta == PalabraConfirmar ? PalabraConfirmar
                          : respuesta == RespuestaTimeout ? RespuestaTimeout : PalabraCancelar,
                TiempoRespuestaMs = (int)Math.Min(int.MaxValue, (DateTime.Now - p.InicioPaso).TotalMilliseconds)
            });

            if (respuesta != PalabraConfirmar) return EstadoConfirmacion.Cancelada;
            if (p.PasoActual >= p.TotalPasos) return EstadoConfirmacion.ListaParaEjecutar;

            p.PasoActual++;
            p.InicioPaso = DateTime.Now;   // el siguiente paso tiene su propio timeout
            return EstadoConfirmacion.EsperandoSiguientePaso;
        }

        public EstadoConfirmacion mtdVencer(OperacionPendiente p)
        {
            return mtdResponder(p, RespuestaTimeout);
        }

        // ================= 3) EJECUTAR / CANCELAR =================

        /// <summary>
        /// Ejecuta el comando. Antes vuelve a comprobar en la BD que el equipo y el comando siguen activos.
        /// La orden se envía al Arduino y se espera su ACK. Si no hay Arduino conectado, solo se simula
        /// cuando SIMULAR_SIN_ARDUINO = true en Configuración; si no, el comando no se ejecuta.
        /// </summary>
        public Resultado mtdEjecutar(OperacionPendiente p)
        {
            try
            {
                bool sigueDisponible = mtdListarComandos(p.Equipo.IdEquipo).Any(c => c.IdComando == p.Comando.IdComando);
                if (!sigueDisponible)
                {
                    Guid idRechazo = mtdGuardar(p, clsEventoNegocio.ResultadoRechazado,
                        "El comando o el equipo fue desactivado mientras se confirmaba.");
                    _alertas.mtdAlertaComando(clsAlertaNegocio.TipoComandoRechazado, p.Comando, p.Equipo, idRechazo,
                        "el comando o el equipo fue desactivado mientras se confirmaba.");
                    return Resultado.Error("El comando ya no está disponible para este equipo. No se ejecutó.");
                }

                string orden = mtdOrdenArduino(p.Comando);
                if (orden == null)
                    return Fallo(p, "El tipo de acción \"" + p.Comando.TipoAccionNombre + "\" no tiene una orden para el Arduino.", null);

                // Sin Arduino: se simula solo si está permitido en Configuración.
                if (!mtdArduinoListo() && _configuracion.mtdObtenerBooleano(clsConfiguracionNegocio.SimularSinArduino))
                {
                    Guid idSimulado = mtdGuardar(p, clsEventoNegocio.ResultadoEjecutado, "Ejecutado en modo simulación (Arduino no conectado).");
                    return Resultado.Ok("\"" + p.Comando.Frase + "\" enviado a " + p.Equipo.Codigo + " (simulación)." + AvisoHistorial(idSimulado));
                }

                RespuestaArduino r = clsEnlaceArduino.mtdEnviarOrden(orden, p.Equipo.IdentificadorHardware);
                if (!r.Exito) return Fallo(p, r.MotivoTexto, r);

                Guid id = mtdGuardar(p, clsEventoNegocio.ResultadoEjecutado,
                    "Ejecutado en el Arduino: " + orden + " (" + r.LatenciaMs + " ms). " + mtdTextoSalidas(r), r.Secuencia, r.Crc);

                string mensaje = "\"" + p.Comando.Frase + "\" ejecutado en " + p.Equipo.Codigo + ".";
                if (orden == "ESTADO") mensaje += " " + mtdTextoSalidas(r);
                return Resultado.Ok(mensaje + AvisoHistorial(id));
            }
            catch (Exception ex)
            {
                Guid idError = mtdGuardar(p, clsEventoNegocio.ResultadoError, ex.Message);
                _alertas.mtdAlertaComando(clsAlertaNegocio.TipoErrorEjecucion, p.Comando, p.Equipo, idError, ex.Message);
                return Resultado.Error("No se pudo ejecutar el comando: " + ex.Message);
            }
        }

        // El Arduino no ejecutó la orden: se registra como error y se genera la alerta.
        private Resultado Fallo(OperacionPendiente p, string motivo, RespuestaArduino r)
        {
            Guid id = mtdGuardar(p, clsEventoNegocio.ResultadoError, "Arduino: " + motivo,
                                 r == null ? (long?)null : r.Secuencia, r == null ? null : r.Crc);
            _alertas.mtdAlertaComando(clsAlertaNegocio.TipoErrorEjecucion, p.Comando, p.Equipo, id, motivo);
            return Resultado.Error("No se ejecutó: " + motivo);
        }

        private static string AvisoHistorial(Guid id)
        {
            return id == Guid.Empty ? " (no se pudo guardar en el historial)" : "";
        }

        /// <summary>Qué orden del Arduino corresponde a cada tipo de acción del comando.</summary>
        public static string mtdOrdenArduino(Comando comando)
        {
            switch (comando.TipoAccionCodigo)
            {
                case "INFORMATIVA": return "ESTADO";
                case "INDICADOR": return "INDICADOR";
                case "OPERAR": return "OPERAR";
                case "DETENER": return "DETENER";
                case "CRITICA": return "CRITICA";
                case "ESTADO_SEGURO": return "PARADA";
                default: return null;
            }
        }

        private static bool mtdArduinoListo()
        {
            return clsEnlaceArduino.Estado == clsEnlaceArduino.EstadoEnlace.Conectado ||
                   clsEnlaceArduino.Estado == clsEnlaceArduino.EstadoEnlace.SinHeartbeat;
        }

        private static string mtdTextoSalidas(RespuestaArduino r)
        {
            return "Indicador " + (r.Indicador ? "encendido" : "apagado") +
                   ", simulador " + (r.Simulador ? "en marcha" : "detenido") +
                   (r.Critica ? ", operación crítica activa" : "") +
                   (r.Modo != "NORMAL" && !string.IsNullOrEmpty(r.Modo) ? ", equipo en " + r.Modo : "") + ".";
        }

        public void mtdCancelar(OperacionPendiente p, string motivo)
        {
            Guid id = mtdGuardar(p, clsEventoNegocio.ResultadoCancelado, motivo);

            bool vencida = p.Respuestas.Count > 0 && p.Respuestas[p.Respuestas.Count - 1].Respuesta == RespuestaTimeout;
            _alertas.mtdAlertaComando(vencida ? clsAlertaNegocio.TipoConfirmacionVencida : clsAlertaNegocio.TipoComandoCancelado,
                                      p.Comando, p.Equipo, id, motivo);
        }

        /// <summary>
        /// Parada del equipo (botón DETENER o parada general). Es una acción segura: no pide confirmación
        /// y se envía al Arduino antes que cualquier otra cosa. Devuelve el texto para mostrar.
        /// </summary>
        public string mtdRegistrarParada(Equipo equipo, OperacionPendiente pendienteCancelada)
        {
            RespuestaArduino r = mtdArduinoListo()
                ? clsEnlaceArduino.mtdEnviarOrden("PARADA", equipo == null ? "*" : equipo.IdentificadorHardware)
                : null;

            if (pendienteCancelada != null)
                mtdGuardar(pendienteCancelada, clsEventoNegocio.ResultadoCancelado, "Cancelado por parada del equipo.");

            string codigo = equipo == null ? "Todos los equipos" : equipo.Codigo;
            string detalle = r == null ? "parada registrada, Arduino no conectado."
                           : r.Exito ? "parada ejecutada en el Arduino (" + r.LatenciaMs + " ms)."
                           : "el Arduino no confirmó la parada: " + r.MotivoTexto;

            Guid id = _eventos.mtdRegistrar(clsEventoNegocio.TipoParada,
                r != null && !r.Exito ? clsEventoNegocio.ResultadoError : clsEventoNegocio.ResultadoEjecutado,
                "Parada solicitada por el operador: " + detalle,
                equipo == null ? (int?)null : equipo.IdEquipo,
                secuencia: r == null ? (long?)null : r.Secuencia, crc: r == null ? null : r.Crc);

            _alertas.mtdAlertaParada(equipo, id);
            return codigo + ": " + detalle;
        }

        /// <summary>Saca al Arduino del estado de PARADA o WATCHDOG (lo hace mantenimiento en Diagnóstico).</summary>
        public Resultado mtdRearmar()
        {
            RespuestaArduino r = clsEnlaceArduino.mtdRearmar();
            _eventos.mtdRegistrar(clsEventoNegocio.TipoRearme,
                r.Exito ? clsEventoNegocio.ResultadoEjecutado : clsEventoNegocio.ResultadoError,
                r.Exito ? "Equipo rearmado desde Diagnóstico." : "No se pudo rearmar: " + r.MotivoTexto,
                secuencia: r.Secuencia == 0 ? (long?)null : r.Secuencia, crc: r.Crc);

            return r.Exito ? Resultado.Ok("Equipo rearmado. Ya acepta órdenes.")
                           : Resultado.Error("No se pudo rearmar: " + r.MotivoTexto);
        }

        // Evento + cada paso de confirmación. Devuelve el IdEvento (Guid.Empty si no se pudo guardar).
        private Guid mtdGuardar(OperacionPendiente p, string resultado, string detalle, long? secuencia = null, string crc = null)
        {
            bool? confirmado = null;
            if (p.RequiereConfirmacion)
                confirmado = p.Respuestas.Count == p.TotalPasos && p.Respuestas.All(x => x.Respuesta == PalabraConfirmar);

            Guid id = _eventos.mtdRegistrar(clsEventoNegocio.TipoComandoVoz, resultado, detalle,
                p.Equipo.IdEquipo, p.Comando.IdComando, p.Comando.IdNivelCriticidad,
                p.TextoReconocido, p.Confianza, confirmado, secuencia: secuencia, crc: crc);

            if (id != Guid.Empty)
            {
                foreach (RespuestaConfirmacion c in p.Respuestas)
                {
                    try { _eventoDatos.mtdRegistrarConfirmacion(id, c.NumeroPaso, c.Respuesta, c.TiempoRespuestaMs); }
                    catch (Exception) { /* el evento ya quedó; la confirmación es un detalle */ }
                }
            }

            return id;
        }
    }
}
