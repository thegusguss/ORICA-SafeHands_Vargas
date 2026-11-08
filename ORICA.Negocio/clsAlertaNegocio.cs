using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ORICA.Datos;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    /// <summary>
    /// Alertas automáticas y su ciclo de vida (ACTIVA → RECONOCIDA → CERRADA).
    /// Los métodos que crean alertas nunca lanzan excepciones: si falla la BD, la operación sigue.
    /// </summary>
    public class clsAlertaNegocio
    {
        // Tipos de alerta
        public const string TipoComandoRechazado = "COMANDO_RECHAZADO";
        public const string TipoComandoCancelado = "COMANDO_CANCELADO";
        public const string TipoConfirmacionVencida = "CONFIRMACION_VENCIDA";
        public const string TipoErrorEjecucion = "ERROR_EJECUCION";
        public const string TipoParadaEquipo = "PARADA_EQUIPO";
        public const string TipoEnlaceArduino = "ENLACE_ARDUINO";

        // Severidades permitidas por el CHECK de dbo.Alerta
        public const string SeveridadCritica = "CRITICA";
        public const string SeveridadAlta = "ALTA";
        public const string SeveridadMedia = "MEDIA";

        private readonly clsAlertaDatos _datos = new clsAlertaDatos();

        // ================= GENERACIÓN AUTOMÁTICA =================

        /// <summary>
        /// Alerta por un comando de nivel Alto (N3) o Crítico (N4) que no se ejecutó.
        /// Para N1/N2 no hace nada (no son operaciones de riesgo).
        /// </summary>
        public void mtdAlertaComando(string tipo, Comando comando, Equipo equipo, Guid idEvento, string motivo)
        {
            if (comando == null || comando.IdNivelCriticidad < 3) return;

            bool critico = comando.IdNivelCriticidad >= 4;
            string severidad;
            switch (tipo)
            {
                case TipoComandoRechazado: severidad = critico ? SeveridadCritica : SeveridadAlta; break;
                case TipoConfirmacionVencida: severidad = critico ? SeveridadAlta : SeveridadMedia; break;
                case TipoErrorEjecucion: severidad = critico ? SeveridadCritica : SeveridadAlta; break;
                default: severidad = critico ? SeveridadAlta : SeveridadMedia; break;   // cancelado
            }

            string descripcion = "\"" + comando.Frase + "\" (N" + comando.IdNivelCriticidad + ")" +
                                 (equipo == null ? "" : " en " + equipo.Codigo) + ": " + motivo;

            mtdCrear(idEvento, tipo, severidad, descripcion);
        }

        /// <summary>Alerta por parada del equipo (botón DETENER).</summary>
        public void mtdAlertaParada(Equipo equipo, Guid idEvento)
        {
            string usuario = Sesion.Activa ? Sesion.UsuarioActual.Codigo : "sin sesión";
            mtdCrear(idEvento, TipoParadaEquipo, SeveridadAlta,
                     "Parada solicitada en " + (equipo == null ? "equipo sin seleccionar" : equipo.Codigo) +
                     " por " + usuario + ".");
        }

        /// <summary>
        /// Alerta porque se perdió la conexión con el Arduino. Si ya hay una abierta igual, no se repite.
        /// La llamará el módulo de enlace serial cuando se implemente.
        /// </summary>
        public void mtdAlertaEnlace(string puerto, string detalle)
        {
            string descripcion = "Se perdió la conexión con el Arduino" +
                                 (string.IsNullOrEmpty(puerto) ? "" : " en " + puerto) +
                                 (string.IsNullOrEmpty(detalle) ? "." : ": " + detalle);
            try
            {
                if (_datos.mtdExisteAbierta(TipoEnlaceArduino, descripcion)) return;
            }
            catch (Exception) { return; }

            mtdCrear(null, TipoEnlaceArduino, SeveridadCritica, descripcion);
        }

        private void mtdCrear(Guid? idEvento, string tipo, string severidad, string descripcion)
        {
            try
            {
                string codigo = "ALR-" + DateTime.Now.ToString("yyMMddHHmmssfff");
                _datos.mtdRegistrar(idEvento.HasValue && idEvento.Value != Guid.Empty ? idEvento : null,
                                    codigo, tipo, severidad, clsAuditoriaNegocio.mtdRecortar(descripcion, 500));
            }
            catch (Exception)
            {
                // La alerta es secundaria: no interrumpe la operación.
            }
        }

        // ================= CONSULTAS =================

        public List<Alerta> mtdListar(int maximo = 300)
        {
            List<Alerta> lista = new List<Alerta>();

            foreach (DataRow r in _datos.mtdListar(maximo).Rows)
            {
                lista.Add(new Alerta
                {
                    IdAlerta = (Guid)r["IdAlerta"],
                    IdEvento = r["IdEvento"] == DBNull.Value ? (Guid?)null : (Guid)r["IdEvento"],
                    Codigo = clsLectura.mtdLeerTexto(r, "Codigo"),
                    Tipo = clsLectura.mtdLeerTexto(r, "Tipo"),
                    Severidad = clsLectura.mtdLeerTexto(r, "Severidad"),
                    Descripcion = clsLectura.mtdLeerTexto(r, "Descripcion"),
                    Estado = clsLectura.mtdLeerTexto(r, "EstadoAlerta"),
                    FechaHora = clsLectura.mtdLeerFecha(r, "FechaHora") ?? DateTime.MinValue,
                    FechaReconocida = clsLectura.mtdLeerFecha(r, "FechaReconocida"),
                    FechaCierre = clsLectura.mtdLeerFecha(r, "FechaCierre"),
                    UsuarioReconoce = clsLectura.mtdLeerTexto(r, "UsuarioReconoce"),
                    UsuarioCierra = clsLectura.mtdLeerTexto(r, "UsuarioCierra"),
                    UsuarioOrigen = clsLectura.mtdLeerTexto(r, "UsuarioOrigen"),
                    Equipo = clsLectura.mtdLeerTexto(r, "Equipo")
                });
            }

            return lista;
        }

        public ResumenAlertas mtdResumen()
        {
            DataTable dt = _datos.mtdResumen();
            if (dt.Rows.Count == 0) return new ResumenAlertas();

            DataRow r = dt.Rows[0];
            return new ResumenAlertas
            {
                Activas = clsLectura.mtdLeerEntero(r, "Activas"),
                Reconocidas = clsLectura.mtdLeerEntero(r, "Reconocidas"),
                UltimaActiva = clsLectura.mtdLeerFecha(r, "UltimaActiva"),
                RechazosConfianzaHoy = clsLectura.mtdLeerEntero(r, "RechazosConfianzaHoy"),
                FallasEnlaceHoy = clsLectura.mtdLeerEntero(r, "FallasEnlaceHoy"),
                FallasEnlaceAbiertas = clsLectura.mtdLeerEntero(r, "FallasEnlaceAbiertas")
            };
        }

        // ================= CICLO DE VIDA =================

        /// <summary>ACTIVA → RECONOCIDA: alguien tomó conocimiento de la alerta.</summary>
        public Resultado mtdReconocer(Alerta a)
        {
            try
            {
                if (a == null) return Resultado.Error("Seleccione una alerta.");
                if (!a.EstaActiva) return Resultado.Error("Solo se puede reconocer una alerta activa (esta está " + a.EstadoTexto.ToLower() + ").");

                if (_datos.mtdReconocer(a.IdAlerta, mtdUsuarioActual()) == 0)
                    return Resultado.Error("Otra persona ya cambió esta alerta. Se actualizará la lista.");

                clsAuditoriaNegocio.mtdRegistrar("ALERTAS", "RECONOCER", "Alerta", a.Codigo,
                    "Alerta " + a.Codigo + " (" + a.TipoTexto + ") reconocida.");
                return Resultado.Ok("Alerta " + a.Codigo + " reconocida.");
            }
            catch (SqlException ex) { return Resultado.Error("Error de base de datos: " + ex.Message); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        /// <summary>RECONOCIDA → CERRADA: el incidente quedó atendido. Una alerta activa primero debe reconocerse.</summary>
        public Resultado mtdCerrar(Alerta a)
        {
            try
            {
                if (a == null) return Resultado.Error("Seleccione una alerta.");
                if (a.EstaActiva) return Resultado.Error("Primero reconozca la alerta y después ciérrela.");
                if (a.EstaCerrada) return Resultado.Error("La alerta ya está cerrada.");

                if (_datos.mtdCerrar(a.IdAlerta, mtdUsuarioActual()) == 0)
                    return Resultado.Error("Otra persona ya cambió esta alerta. Se actualizará la lista.");

                clsAuditoriaNegocio.mtdRegistrar("ALERTAS", "CERRAR", "Alerta", a.Codigo,
                    "Alerta " + a.Codigo + " (" + a.TipoTexto + ") cerrada.");
                return Resultado.Ok("Alerta " + a.Codigo + " cerrada.");
            }
            catch (SqlException ex) { return Resultado.Error("Error de base de datos: " + ex.Message); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        private static int? mtdUsuarioActual()
        {
            return Sesion.Activa ? (int?)Sesion.UsuarioActual.IdUsuario : null;
        }
    }
}
