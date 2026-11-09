using System;
using System.Data;

namespace ORICA.Datos
{
    // Consulta de los eventos de operación (voz y paradas) para el módulo de Reportes.
    public class clsReporteDatos
    {
        // Devuelve los eventos entre dos fechas. Los filtros en NULL significan "todos".
        // hasta es exclusivo: para incluir todo el día 08/11 se envía 09/11 00:00.
        public DataTable mtdListarEventos(DateTime desde, DateTime hasta, int? idUsuario, string resultado,
                                          int? idNivel, int? idEquipo, int maximo)
        {
            const string sql = @"
SELECT TOP (@Maximo)
       e.FechaHora, e.TipoEvento, e.Resultado, e.Confianza, e.TextoDetectado, e.Confirmado,
       u.Codigo AS UsuarioCodigo, u.Nombres + ' ' + u.Apellidos AS UsuarioNombre,
       c.Frase, eq.Codigo AS EquipoCodigo, nc.Codigo AS NivelCodigo
FROM dbo.Evento e
LEFT JOIN dbo.Usuario u          ON u.IdUsuario = e.IdUsuario
LEFT JOIN dbo.Comando c          ON c.IdComando = e.IdComando
LEFT JOIN dbo.Equipo eq          ON eq.IdEquipo = e.IdEquipo
LEFT JOIN dbo.NivelCriticidad nc ON nc.IdNivelCriticidad = e.IdNivelCriticidad
WHERE e.TipoEvento IN ('COMANDO_VOZ', 'VOZ_NO_RECONOCIDA', 'PARADA_SIMULADA')
  AND e.FechaHora >= @Desde AND e.FechaHora < @Hasta
  AND (@IdUsuario IS NULL OR e.IdUsuario = @IdUsuario)
  AND (@Resultado IS NULL OR e.Resultado = @Resultado)
  AND (@IdNivel   IS NULL OR e.IdNivelCriticidad = @IdNivel)
  AND (@IdEquipo  IS NULL OR e.IdEquipo = @IdEquipo)
ORDER BY e.FechaHora DESC;";

            return clsBaseDatos.mtdConsultar(sql,
                clsBaseDatos.mtdParametro("@Maximo", maximo),
                clsBaseDatos.mtdParametro("@Desde", desde),
                clsBaseDatos.mtdParametro("@Hasta", hasta),
                clsBaseDatos.mtdParametro("@IdUsuario", idUsuario),
                clsBaseDatos.mtdParametro("@Resultado", resultado),
                clsBaseDatos.mtdParametro("@IdNivel", idNivel),
                clsBaseDatos.mtdParametro("@IdEquipo", idEquipo));
        }
    }
}
