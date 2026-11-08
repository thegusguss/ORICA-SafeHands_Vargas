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
    }
}
