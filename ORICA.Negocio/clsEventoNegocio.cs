using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ORICA.Datos;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    /// <summary>
    /// Punto único para registrar eventos de operación (dbo.Evento) y para leer el historial.
    /// mtdRegistrar nunca lanza excepciones: devuelve Guid.Empty si no pudo guardar.
    /// </summary>
    public class clsEventoNegocio
    {
        // Tipos de evento (TipoEvento es texto libre en la BD; se usan siempre estos)
        public const string TipoInicioSesion = "INICIO_SESION";
        public const string TipoCierreSesion = "CIERRE_SESION";
        public const string TipoAccesoFallido = "ACCESO_FALLIDO";
        public const string TipoComandoVoz = "COMANDO_VOZ";
        public const string TipoNoReconocido = "VOZ_NO_RECONOCIDA";
        public const string TipoParada = "PARADA_SIMULADA";

        // Resultados permitidos por el CHECK de dbo.Evento
        public const string ResultadoEjecutado = "EJECUTADO";
        public const string ResultadoRechazado = "RECHAZADO";
        public const string ResultadoCancelado = "CANCELADO";
        public const string ResultadoError = "ERROR";
        public const string ResultadoIgnorado = "IGNORADO";
        public const string ResultadoInformativo = "INFORMATIVO";

        private readonly clsEventoDatos _datos = new clsEventoDatos();

        // ================= ESCRITURA =================

        /// <summary>
        /// Registra un evento. Si no se indica idUsuario, se usa el de la sesión actual.
        /// </summary>
        public Guid mtdRegistrar(string tipoEvento, string resultado, string detalle = null,
                                 int? idEquipo = null, int? idComando = null, int? idNivelCriticidad = null,
                                 string textoDetectado = null, decimal? confianza = null, bool? confirmado = null,
                                 int? idUsuario = null)
        {
            try
            {
                if (!idUsuario.HasValue && Sesion.Activa) idUsuario = Sesion.UsuarioActual.IdUsuario;
                Guid? idSesion = Sesion.IdSesion == Guid.Empty ? (Guid?)null : Sesion.IdSesion;

                if (confianza.HasValue) confianza = Math.Max(0m, Math.Min(100m, Math.Round(confianza.Value, 2)));

                return _datos.mtdRegistrar(idSesion, idUsuario, idEquipo, idComando, idNivelCriticidad,
                    clsAuditoriaNegocio.mtdRecortar(tipoEvento, 40),
                    clsAuditoriaNegocio.mtdRecortar(textoDetectado, 250),
                    confianza, resultado, confirmado,
                    clsAuditoriaNegocio.mtdRecortar(detalle, 500));
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }

        // ================= CONSULTAS =================

        /// <summary>Eventos y auditoría de todos los usuarios (pantalla del administrador).</summary>
        public List<RegistroHistorial> mtdListarHistorial(int maximo = 500)
        {
            return mtdLeer(_datos.mtdListarHistorial(maximo, null, true));
        }

        /// <summary>Solo los eventos de un usuario (pantalla "Mi historial" del operador).</summary>
        public List<RegistroHistorial> mtdListarHistorialUsuario(int idUsuario, int maximo = 200)
        {
            return mtdLeer(_datos.mtdListarHistorial(maximo, idUsuario, false));
        }

        // ================= MAPEO =================

        private static List<RegistroHistorial> mtdLeer(DataTable dt)
        {
            List<RegistroHistorial> lista = new List<RegistroHistorial>();

            foreach (DataRow r in dt.Rows)
            {
                string fuente = clsLectura.mtdLeerTexto(r, "Fuente");
                string tipo = clsLectura.mtdLeerTexto(r, "Tipo");
                string codigo = clsLectura.mtdLeerTexto(r, "UsuarioCodigo");
                string nombre = clsLectura.mtdLeerTexto(r, "UsuarioNombre");
                string frase = clsLectura.mtdLeerTexto(r, "Frase");
                string equipo = clsLectura.mtdLeerTexto(r, "EquipoCodigo");
                string equipoNombre = clsLectura.mtdLeerTexto(r, "EquipoNombre");
                string texto = clsLectura.mtdLeerTexto(r, "TextoDetectado");
                string detalle = clsLectura.mtdLeerTexto(r, "Detalle");
                string modulo = clsLectura.mtdLeerTexto(r, "Modulo");
                string anterior = clsLectura.mtdLeerTexto(r, "ValorAnterior");
                string nuevo = clsLectura.mtdLeerTexto(r, "ValorNuevo");
                int nivel = clsLectura.mtdLeerEntero(r, "IdNivelCriticidad");

                // Texto corto para la tabla
                StringBuilder corto = new StringBuilder();
                if (fuente == "AUDITORIA")
                {
                    corto.Append(modulo).Append(" · ").Append(tipo);
                    if (!string.IsNullOrEmpty(detalle)) corto.Append(": ").Append(detalle);
                }
                else
                {
                    corto.Append(mtdNombreTipo(tipo));
                    if (!string.IsNullOrEmpty(frase)) corto.Append(": \"").Append(frase).Append("\"");
                    else if (!string.IsNullOrEmpty(texto)) corto.Append(": \"").Append(texto).Append("\"");
                    if (!string.IsNullOrEmpty(equipo)) corto.Append(" · ").Append(equipo);
                    if (string.IsNullOrEmpty(frase) && string.IsNullOrEmpty(texto) && !string.IsNullOrEmpty(detalle))
                        corto.Append(" · ").Append(detalle);
                }

                // Texto completo para el detalle
                StringBuilder completo = new StringBuilder(corto.ToString());
                if (fuente == "EVENTO" && !string.IsNullOrEmpty(detalle) && completo.ToString().IndexOf(detalle, StringComparison.Ordinal) < 0)
                    completo.Append("\n").Append(detalle);
                if (anterior != null) completo.Append("\nAntes: ").Append(anterior);
                if (nuevo != null) completo.Append("\nDespués: ").Append(nuevo);

                lista.Add(new RegistroHistorial
                {
                    FechaHora = clsLectura.mtdLeerFecha(r, "FechaHora") ?? DateTime.MinValue,
                    Fuente = fuente,
                    Tipo = tipo,
                    Usuario = codigo == null ? "Sistema" : codigo + " - " + nombre,
                    Descripcion = corto.ToString(),
                    Confianza = r["Confianza"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(r["Confianza"]),
                    IdNivelCriticidad = nivel > 0 ? (int?)nivel : null,
                    Criticidad = clsLectura.mtdLeerTexto(r, "CriticidadNombre"),
                    Resultado = clsLectura.mtdLeerTexto(r, "Resultado"),
                    Detalle = completo.ToString(),
                    Equipo = equipo == null ? null : equipo + " - " + equipoNombre,
                    Comando = !string.IsNullOrEmpty(frase) ? frase
                            : !string.IsNullOrEmpty(texto) ? "\"" + texto + "\""
                            : fuente == "AUDITORIA" ? modulo + " · " + tipo : mtdNombreTipo(tipo),
                    TextoDetectado = texto
                });
            }

            return lista;
        }

        private static string mtdNombreTipo(string tipo)
        {
            switch (tipo)
            {
                case TipoInicioSesion: return "Inicio de sesión";
                case TipoCierreSesion: return "Cierre de sesión";
                case TipoAccesoFallido: return "Acceso fallido";
                case TipoComandoVoz: return "Comando de voz";
                case TipoNoReconocido: return "Voz no reconocida";
                case TipoParada: return "Parada simulada";
                default: return tipo;
            }
        }
    }
}
