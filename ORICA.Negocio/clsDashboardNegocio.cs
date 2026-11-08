using System;
using System.Data;
using System.Linq;
using ORICA.Datos;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    /// <summary>
    /// Arma el resumen del panel principal y deduce el estado simulado del prototipo
    /// (LED, motor, buzzer, parada) a partir de los últimos comandos ejecutados.
    /// </summary>
    public class clsDashboardNegocio
    {
        private const int MinutosBuzzer = 2;   // el buzzer se considera sonando 2 minutos tras una operación crítica

        private readonly clsDashboardDatos _datos = new clsDashboardDatos();

        public ResumenDashboard mtdResumen()
        {
            DataTable dt = _datos.mtdResumen(DateTime.Today);
            ResumenDashboard r = new ResumenDashboard();
            if (dt.Rows.Count == 0) return r;

            DataRow f = dt.Rows[0];

            r.Operadores = clsLectura.mtdLeerEntero(f, "Operadores");
            r.Credenciales = clsLectura.mtdLeerEntero(f, "Credenciales");
            r.Comandos = clsLectura.mtdLeerEntero(f, "Comandos");
            r.Equipos = clsLectura.mtdLeerEntero(f, "Equipos");
            r.IntentosHoy = clsLectura.mtdLeerEntero(f, "IntentosHoy");
            r.EjecutadosHoy = clsLectura.mtdLeerEntero(f, "EjecutadosHoy");
            r.RechazadosHoy = clsLectura.mtdLeerEntero(f, "RechazadosHoy");
            r.ConfianzaHoy = LeerDecimal(f, "ConfianzaHoy");
            r.Confianza7Dias = LeerDecimal(f, "Confianza7Dias");
            r.AlertasPendientes = clsLectura.mtdLeerEntero(f, "AlertasPendientes");
            r.AlertasActivas = clsLectura.mtdLeerEntero(f, "AlertasActivas");
            r.SesionesAbiertas = clsLectura.mtdLeerEntero(f, "SesionesAbiertas");

            DateTime? operar = clsLectura.mtdLeerFecha(f, "UltimoOperar");
            DateTime? detener = clsLectura.mtdLeerFecha(f, "UltimoDetener");
            DateTime? indicador = clsLectura.mtdLeerFecha(f, "UltimoIndicador");
            DateTime? critica = clsLectura.mtdLeerFecha(f, "UltimoCritica");
            DateTime? estadoSeguro = clsLectura.mtdLeerFecha(f, "UltimoEstadoSeguro");
            DateTime? parada = clsLectura.mtdLeerFecha(f, "UltimaParada");

            // Lo último que apagó todo: una parada o un comando de estado seguro.
            DateTime? reinicio = Mayor(parada, estadoSeguro);

            // Motor: en marcha si lo último fue "iniciar" (después de cualquier detener / parada).
            DateTime? apagadoMotor = Mayor(detener, reinicio);
            r.MotorEnMarcha = operar.HasValue && (!apagadoMotor.HasValue || operar > apagadoMotor);
            r.MotorDesde = r.MotorEnMarcha ? operar : apagadoMotor;

            // LED: encendido si se encendió después de la última parada / estado seguro.
            r.LedEncendido = indicador.HasValue && (!reinicio.HasValue || indicador > reinicio);
            r.LedDesde = r.LedEncendido ? indicador : reinicio;

            // Buzzer: suena unos minutos después de una operación crítica (si no hubo parada después).
            r.BuzzerActivo = critica.HasValue && (!reinicio.HasValue || critica > reinicio) &&
                             (DateTime.Now - critica.Value).TotalMinutes <= MinutosBuzzer;
            r.BuzzerDesde = critica;

            // Parada: activa si después de la última parada no se volvió a operar nada.
            DateTime? ultimaActividad = Mayor(Mayor(operar, indicador), critica);
            r.ParadaActiva = parada.HasValue && (!ultimaActividad.HasValue || parada > ultimaActividad);
            r.UltimaParada = parada;

            return r;
        }

        /// <summary>Resumen de la pantalla de inicio del operador que inició sesión.</summary>
        public ResumenOperador mtdResumenOperador()
        {
            ResumenOperador r = new ResumenOperador();
            if (!Sesion.Activa) return r;

            int idUsuario = Sesion.UsuarioActual.IdUsuario;

            DataTable dt = _datos.mtdResumenOperador(idUsuario, Sesion.IdSesion);
            if (dt.Rows.Count > 0)
            {
                DataRow f = dt.Rows[0];
                r.AccionesSesion = clsLectura.mtdLeerEntero(f, "AccionesSesion");
                r.EjecutadasSesion = clsLectura.mtdLeerEntero(f, "EjecutadasSesion");
                r.ConfirmadasSesion = clsLectura.mtdLeerEntero(f, "ConfirmadasSesion");
                r.RechazadasSesion = clsLectura.mtdLeerEntero(f, "RechazadasSesion");
                r.CanceladasSesion = clsLectura.mtdLeerEntero(f, "CanceladasSesion");
                r.EjecutadasTotal = clsLectura.mtdLeerEntero(f, "EjecutadasTotal");
                r.SesionAnterior = clsLectura.mtdLeerFecha(f, "SesionAnterior");
                r.EquiposDisponibles = clsLectura.mtdLeerEntero(f, "EquiposDisponibles");
                r.ComandosDisponibles = clsLectura.mtdLeerEntero(f, "ComandosDisponibles");
                r.CodigosEquipos = clsLectura.mtdLeerTexto(f, "CodigosEquipos");
            }

            DataTable ultima = _datos.mtdUltimaAccionOperador(idUsuario);
            if (ultima.Rows.Count > 0)
            {
                DataRow u = ultima.Rows[0];
                string frase = clsLectura.mtdLeerTexto(u, "Frase");
                string texto = clsLectura.mtdLeerTexto(u, "TextoDetectado");
                string equipo = clsLectura.mtdLeerTexto(u, "EquipoCodigo");
                bool esParada = clsLectura.mtdLeerTexto(u, "TipoEvento") == clsEventoNegocio.TipoParada;

                r.UltimaAccionFecha = clsLectura.mtdLeerFecha(u, "FechaHora");
                r.UltimaAccionResultado = clsLectura.mtdLeerTexto(u, "Resultado");
                r.UltimaAccionTexto = (esParada ? "Parada de equipo"
                                      : "\"" + (frase ?? texto ?? "comando") + "\"") +
                                      (equipo == null ? "" : " en " + equipo);
            }

            return r;
        }

        /// <summary>Envía una parada a todos los equipos activos (botón PARADA SIMULADA del administrador).</summary>
        public Resultado mtdParadaGeneral()
        {
            try
            {
                clsOperacionVozNegocio voz = new clsOperacionVozNegocio();
                var equipos = new clsEquipoNegocio().mtdListarActivos();

                if (equipos.Count == 0) return Resultado.Error("No hay equipos activos para detener.");

                foreach (Equipo eq in equipos)
                    voz.mtdRegistrarParada(eq, null);

                return Resultado.Ok("Parada enviada a " + equipos.Count + " equipo(s): " +
                                    string.Join(", ", equipos.Select(e => e.Codigo)) + " (simulación).");
            }
            catch (Exception ex)
            {
                return Resultado.Error("No se pudo registrar la parada: " + ex.Message);
            }
        }

        private static DateTime? Mayor(DateTime? a, DateTime? b)
        {
            if (!a.HasValue) return b;
            if (!b.HasValue) return a;
            return a > b ? a : b;
        }

        private static decimal? LeerDecimal(DataRow r, string columna)
        {
            return r[columna] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(r[columna]);
        }
    }
}
