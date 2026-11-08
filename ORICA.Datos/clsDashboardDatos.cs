using System;
using System.Data;

namespace ORICA.Datos
{
    // Una sola consulta con todos los números del panel principal del administrador.
    public class clsDashboardDatos
    {
        public DataTable mtdResumen(DateTime hoy)
        {
            const string sql = @"
SELECT
    -- Totales del sistema (solo activos)
    (SELECT COUNT(1) FROM dbo.Usuario    WHERE Estado = 1) AS Operadores,
    (SELECT COUNT(1) FROM dbo.Credencial WHERE Estado = 1) AS Credenciales,
    (SELECT COUNT(1) FROM dbo.Comando    WHERE Estado = 1) AS Comandos,
    (SELECT COUNT(1) FROM dbo.Equipo     WHERE Estado = 1) AS Equipos,

    -- Comandos de voz de hoy
    (SELECT COUNT(1) FROM dbo.Evento
      WHERE TipoEvento = 'COMANDO_VOZ' AND FechaHora >= @Hoy)                          AS IntentosHoy,
    (SELECT COUNT(1) FROM dbo.Evento
      WHERE TipoEvento = 'COMANDO_VOZ' AND Resultado = 'EJECUTADO' AND FechaHora >= @Hoy) AS EjecutadosHoy,
    (SELECT COUNT(1) FROM dbo.Evento
      WHERE TipoEvento = 'COMANDO_VOZ' AND Resultado = 'RECHAZADO' AND FechaHora >= @Hoy) AS RechazadosHoy,
    (SELECT AVG(Confianza) FROM dbo.Evento
      WHERE TipoEvento = 'COMANDO_VOZ' AND Confianza IS NOT NULL AND FechaHora >= @Hoy)  AS ConfianzaHoy,
    (SELECT AVG(Confianza) FROM dbo.Evento
      WHERE TipoEvento = 'COMANDO_VOZ' AND Confianza IS NOT NULL
        AND FechaHora >= DATEADD(DAY, -7, @Hoy))                                        AS Confianza7Dias,

    -- Alertas
    (SELECT COUNT(1) FROM dbo.Alerta WHERE EstadoAlerta IN ('ACTIVA', 'RECONOCIDA')) AS AlertasPendientes,
    (SELECT COUNT(1) FROM dbo.Alerta WHERE EstadoAlerta = 'ACTIVA')                  AS AlertasActivas,

    -- Sesiones de hoy que siguen abiertas
    (SELECT COUNT(1) FROM dbo.SesionOperador
      WHERE EstadoSesion = 'ABIERTA' AND FechaInicio >= @Hoy)                        AS SesionesAbiertas,

    -- Última vez que se ejecutó cada tipo de acción (para el estado simulado del prototipo)
    (SELECT MAX(e.FechaHora) FROM dbo.Evento e
       INNER JOIN dbo.Comando c     ON c.IdComando = e.IdComando
       INNER JOIN dbo.TipoAccion ta ON ta.IdTipoAccion = c.IdTipoAccion
      WHERE e.TipoEvento = 'COMANDO_VOZ' AND e.Resultado = 'EJECUTADO' AND ta.Codigo = 'OPERAR')        AS UltimoOperar,
    (SELECT MAX(e.FechaHora) FROM dbo.Evento e
       INNER JOIN dbo.Comando c     ON c.IdComando = e.IdComando
       INNER JOIN dbo.TipoAccion ta ON ta.IdTipoAccion = c.IdTipoAccion
      WHERE e.TipoEvento = 'COMANDO_VOZ' AND e.Resultado = 'EJECUTADO' AND ta.Codigo = 'DETENER')       AS UltimoDetener,
    (SELECT MAX(e.FechaHora) FROM dbo.Evento e
       INNER JOIN dbo.Comando c     ON c.IdComando = e.IdComando
       INNER JOIN dbo.TipoAccion ta ON ta.IdTipoAccion = c.IdTipoAccion
      WHERE e.TipoEvento = 'COMANDO_VOZ' AND e.Resultado = 'EJECUTADO' AND ta.Codigo = 'INDICADOR')     AS UltimoIndicador,
    (SELECT MAX(e.FechaHora) FROM dbo.Evento e
       INNER JOIN dbo.Comando c     ON c.IdComando = e.IdComando
       INNER JOIN dbo.TipoAccion ta ON ta.IdTipoAccion = c.IdTipoAccion
      WHERE e.TipoEvento = 'COMANDO_VOZ' AND e.Resultado = 'EJECUTADO' AND ta.Codigo = 'CRITICA')       AS UltimoCritica,
    (SELECT MAX(e.FechaHora) FROM dbo.Evento e
       INNER JOIN dbo.Comando c     ON c.IdComando = e.IdComando
       INNER JOIN dbo.TipoAccion ta ON ta.IdTipoAccion = c.IdTipoAccion
      WHERE e.TipoEvento = 'COMANDO_VOZ' AND e.Resultado = 'EJECUTADO' AND ta.Codigo = 'ESTADO_SEGURO') AS UltimoEstadoSeguro,
    (SELECT MAX(FechaHora) FROM dbo.Evento WHERE TipoEvento = 'PARADA_SIMULADA')                       AS UltimaParada;";

            return clsBaseDatos.mtdConsultar(sql, clsBaseDatos.mtdParametro("@Hoy", hoy));
        }

        /// <summary>Números de la pantalla de inicio de UN operador (su sesión actual y su historial).</summary>
        public DataTable mtdResumenOperador(int idUsuario, Guid idSesion)
        {
            const string sql = @"
SELECT
    -- Lo hecho en la sesión actual
    (SELECT COUNT(1) FROM dbo.Evento WHERE IdSesion = @IdSesion AND TipoEvento = 'COMANDO_VOZ')                           AS AccionesSesion,
    (SELECT COUNT(1) FROM dbo.Evento WHERE IdSesion = @IdSesion AND TipoEvento = 'COMANDO_VOZ' AND Resultado = 'EJECUTADO') AS EjecutadasSesion,
    (SELECT COUNT(1) FROM dbo.Evento WHERE IdSesion = @IdSesion AND TipoEvento = 'COMANDO_VOZ'
                                       AND Resultado = 'EJECUTADO' AND Confirmado = 1)                                    AS ConfirmadasSesion,
    (SELECT COUNT(1) FROM dbo.Evento WHERE IdSesion = @IdSesion AND TipoEvento = 'COMANDO_VOZ' AND Resultado = 'RECHAZADO') AS RechazadasSesion,
    (SELECT COUNT(1) FROM dbo.Evento WHERE IdSesion = @IdSesion AND TipoEvento = 'COMANDO_VOZ' AND Resultado = 'CANCELADO') AS CanceladasSesion,

    -- Histórico del operador
    (SELECT COUNT(1) FROM dbo.Evento WHERE IdUsuario = @IdUsuario AND TipoEvento = 'COMANDO_VOZ' AND Resultado = 'EJECUTADO') AS EjecutadasTotal,
    (SELECT TOP 1 FechaInicio FROM dbo.SesionOperador
      WHERE IdUsuario = @IdUsuario AND IdSesion <> @IdSesion ORDER BY FechaInicio DESC)                                   AS SesionAnterior,

    -- Equipos que se pueden usar por voz (equipo, comando y asignación activos)
    (SELECT COUNT(DISTINCT e.IdEquipo) FROM dbo.Equipo e
       INNER JOIN dbo.ComandoEquipo ce ON ce.IdEquipo = e.IdEquipo AND ce.Estado = 1
       INNER JOIN dbo.Comando c        ON c.IdComando = ce.IdComando AND c.Estado = 1
      WHERE e.Estado = 1)                                                                                                AS EquiposDisponibles,
    (SELECT COUNT(DISTINCT c.IdComando) FROM dbo.Equipo e
       INNER JOIN dbo.ComandoEquipo ce ON ce.IdEquipo = e.IdEquipo AND ce.Estado = 1
       INNER JOIN dbo.Comando c        ON c.IdComando = ce.IdComando AND c.Estado = 1
      WHERE e.Estado = 1)                                                                                                AS ComandosDisponibles,
    (SELECT STRING_AGG(x.Codigo, ', ') FROM (
        SELECT DISTINCT e.Codigo FROM dbo.Equipo e
          INNER JOIN dbo.ComandoEquipo ce ON ce.IdEquipo = e.IdEquipo AND ce.Estado = 1
          INNER JOIN dbo.Comando c        ON c.IdComando = ce.IdComando AND c.Estado = 1
         WHERE e.Estado = 1) x)                                                                                          AS CodigosEquipos;";

            return clsBaseDatos.mtdConsultar(sql,
                clsBaseDatos.mtdParametro("@IdUsuario", idUsuario),
                clsBaseDatos.mtdParametro("@IdSesion", idSesion));
        }

        /// <summary>Última acción del operador (comando de voz o parada), de cualquier sesión.</summary>
        public DataTable mtdUltimaAccionOperador(int idUsuario)
        {
            const string sql = @"
SELECT TOP 1 e.FechaHora, e.TipoEvento, e.Resultado, e.TextoDetectado, c.Frase, eq.Codigo AS EquipoCodigo
FROM dbo.Evento e
LEFT JOIN dbo.Comando c ON c.IdComando = e.IdComando
LEFT JOIN dbo.Equipo eq ON eq.IdEquipo = e.IdEquipo
WHERE e.IdUsuario = @IdUsuario
  AND e.TipoEvento IN ('COMANDO_VOZ', 'PARADA_SIMULADA')
ORDER BY e.FechaHora DESC;";

            return clsBaseDatos.mtdConsultar(sql, clsBaseDatos.mtdParametro("@IdUsuario", idUsuario));
        }
    }
}
