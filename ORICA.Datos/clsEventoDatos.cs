using System;
using System.Data;

namespace ORICA.Datos
{
    // Acceso a datos de dbo.Evento (lo que pasa en la operación) y del historial combinado con Auditoría.
    public class clsEventoDatos
    {
        /// <summary>Inserta un evento y devuelve su IdEvento (GUID generado por la BD).</summary>
        public Guid mtdRegistrar(Guid? idSesion, int? idUsuario, int? idEquipo, int? idComando, int? idNivel,
                                 string tipoEvento, string textoDetectado, decimal? confianza, string resultado,
                                 bool? confirmado, string detalle)
        {
            const string sql = @"
INSERT INTO dbo.Evento (IdSesion, IdUsuario, IdEquipo, IdComando, IdNivelCriticidad, TipoEvento,
                        TextoDetectado, Confianza, Resultado, Confirmado, Detalle)
OUTPUT INSERTED.IdEvento
VALUES (@IdSesion, @IdUsuario, @IdEquipo, @IdComando, @IdNivel, @TipoEvento,
        @Texto, @Confianza, @Resultado, @Confirmado, @Detalle);";

            object id = clsBaseDatos.mtdConsultarValor(sql,
                clsBaseDatos.mtdParametro("@IdSesion", idSesion),
                clsBaseDatos.mtdParametro("@IdUsuario", idUsuario),
                clsBaseDatos.mtdParametro("@IdEquipo", idEquipo),
                clsBaseDatos.mtdParametro("@IdComando", idComando),
                clsBaseDatos.mtdParametro("@IdNivel", idNivel),
                clsBaseDatos.mtdParametro("@TipoEvento", tipoEvento),
                clsBaseDatos.mtdParametro("@Texto", textoDetectado),
                clsBaseDatos.mtdParametro("@Confianza", confianza),
                clsBaseDatos.mtdParametro("@Resultado", resultado),
                clsBaseDatos.mtdParametro("@Confirmado", confirmado),
                clsBaseDatos.mtdParametro("@Detalle", detalle));

            return (Guid)id;
        }

        /// <summary>Guarda la respuesta a un paso de confirmación de un evento (dbo.Confirmacion).</summary>
        public int mtdRegistrarConfirmacion(Guid idEvento, int numeroPaso, string respuesta, int? tiempoRespuestaMs)
        {
            const string sql = @"
INSERT INTO dbo.Confirmacion (IdEvento, NumeroPaso, Respuesta, TiempoRespuestaMs)
VALUES (@IdEvento, @Paso, @Respuesta, @Tiempo);";

            return clsBaseDatos.mtdEjecutarComando(sql,
                clsBaseDatos.mtdParametro("@IdEvento", idEvento),
                clsBaseDatos.mtdParametro("@Paso", numeroPaso),
                clsBaseDatos.mtdParametro("@Respuesta", respuesta),
                clsBaseDatos.mtdParametro("@Tiempo", tiempoRespuestaMs));
        }

        /// <summary>
        /// Últimos registros de Evento y Auditoría juntos, del más nuevo al más viejo.
        /// idUsuario: si se indica, solo los de ese usuario. incluirAuditoria: false = solo eventos.
        /// </summary>
        public DataTable mtdListarHistorial(int maximo, int? idUsuario, bool incluirAuditoria)
        {
            const string sql = @"
SELECT TOP (@Maximo) *
FROM (
    SELECT e.FechaHora, 'EVENTO' AS Fuente, e.TipoEvento AS Tipo,
           u.Codigo AS UsuarioCodigo, u.Nombres + ' ' + u.Apellidos AS UsuarioNombre,
           c.Frase, eq.Codigo AS EquipoCodigo, eq.Nombre AS EquipoNombre, e.TextoDetectado, e.Detalle,
           e.Confianza, e.IdNivelCriticidad, nc.Nombre AS CriticidadNombre,
           e.Resultado, e.Confirmado, CAST(NULL AS VARCHAR(60)) AS Modulo,
           CAST(NULL AS NVARCHAR(MAX)) AS ValorAnterior, CAST(NULL AS NVARCHAR(MAX)) AS ValorNuevo
    FROM dbo.Evento e
    LEFT JOIN dbo.Usuario u          ON u.IdUsuario = e.IdUsuario
    LEFT JOIN dbo.Comando c          ON c.IdComando = e.IdComando
    LEFT JOIN dbo.Equipo eq          ON eq.IdEquipo = e.IdEquipo
    LEFT JOIN dbo.NivelCriticidad nc ON nc.IdNivelCriticidad = e.IdNivelCriticidad
    WHERE (@IdUsuario IS NULL OR e.IdUsuario = @IdUsuario)

    UNION ALL

    SELECT a.FechaHora, 'AUDITORIA', a.Accion,
           u.Codigo, u.Nombres + ' ' + u.Apellidos,
           NULL, NULL, NULL, NULL, a.Descripcion,
           NULL, NULL, NULL,
           'AUDITORIA', NULL, a.Modulo,
           a.ValorAnterior, a.ValorNuevo
    FROM dbo.Auditoria a
    LEFT JOIN dbo.Usuario u ON u.IdUsuario = a.IdUsuario
    WHERE @IncluirAuditoria = 1
      AND (@IdUsuario IS NULL OR a.IdUsuario = @IdUsuario)
) h
ORDER BY h.FechaHora DESC;";

            return clsBaseDatos.mtdConsultar(sql,
                clsBaseDatos.mtdParametro("@Maximo", maximo),
                clsBaseDatos.mtdParametro("@IdUsuario", idUsuario),
                clsBaseDatos.mtdParametro("@IncluirAuditoria", incluirAuditoria));
        }
    }
}
