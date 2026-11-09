using System;
using System.Data;

namespace ORICA.Datos
{
    // Acceso a datos de dbo.Alerta. Ciclo de vida: ACTIVA → RECONOCIDA → CERRADA.
    public class clsAlertaDatos
    {
        /// <summary>Inserta una alerta ACTIVA y devuelve su IdAlerta.</summary>
        public Guid mtdRegistrar(Guid? idEvento, string codigo, string tipo, string severidad, string descripcion)
        {
            const string sql = @"
INSERT INTO dbo.Alerta (IdEvento, Codigo, Tipo, Severidad, Descripcion)
OUTPUT INSERTED.IdAlerta
VALUES (@IdEvento, @Codigo, @Tipo, @Severidad, @Descripcion);";

            object id = clsBaseDatos.mtdConsultarValor(sql,
                clsBaseDatos.mtdParametro("@IdEvento", idEvento),
                clsBaseDatos.mtdParametro("@Codigo", codigo),
                clsBaseDatos.mtdParametro("@Tipo", tipo),
                clsBaseDatos.mtdParametro("@Severidad", severidad),
                clsBaseDatos.mtdParametro("@Descripcion", descripcion));

            return (Guid)id;
        }

        /// <summary>¿Ya hay una alerta sin cerrar de ese tipo y con esa descripción? (evita repetir la misma falla)</summary>
        public bool mtdExisteAbierta(string tipo, string descripcion)
        {
            const string sql = @"
SELECT COUNT(1) FROM dbo.Alerta
WHERE Tipo = @Tipo AND Descripcion = @Descripcion AND EstadoAlerta <> 'CERRADA';";

            object total = clsBaseDatos.mtdConsultarValor(sql,
                clsBaseDatos.mtdParametro("@Tipo", tipo),
                clsBaseDatos.mtdParametro("@Descripcion", descripcion));

            return Convert.ToInt32(total) > 0;
        }

        /// <summary>Alertas para la pantalla: primero las activas, luego las reconocidas y al final las cerradas.</summary>
        public DataTable mtdListar(int maximo)
        {
            const string sql = @"
SELECT TOP (@Maximo)
       a.IdAlerta, a.IdEvento, a.Codigo, a.Tipo, a.Severidad, a.Descripcion, a.EstadoAlerta,
       a.FechaHora, a.FechaReconocida, a.FechaCierre,
       ur.Codigo + ' - ' + ur.Nombres + ' ' + ur.Apellidos AS UsuarioReconoce,
       uc.Codigo + ' - ' + uc.Nombres + ' ' + uc.Apellidos AS UsuarioCierra,
       uo.Codigo + ' - ' + uo.Nombres + ' ' + uo.Apellidos AS UsuarioOrigen,
       eq.Codigo + ' - ' + eq.Nombre AS Equipo
FROM dbo.Alerta a
LEFT JOIN dbo.Usuario ur ON ur.IdUsuario = a.IdUsuarioReconoce
LEFT JOIN dbo.Usuario uc ON uc.IdUsuario = a.IdUsuarioCierra
LEFT JOIN dbo.Evento e   ON e.IdEvento = a.IdEvento
LEFT JOIN dbo.Usuario uo ON uo.IdUsuario = e.IdUsuario
LEFT JOIN dbo.Equipo eq  ON eq.IdEquipo = e.IdEquipo
ORDER BY CASE a.EstadoAlerta WHEN 'ACTIVA' THEN 0 WHEN 'RECONOCIDA' THEN 1 ELSE 2 END,
         a.FechaHora DESC;";

            return clsBaseDatos.mtdConsultar(sql, clsBaseDatos.mtdParametro("@Maximo", maximo));
        }

        /// <summary>ACTIVA → RECONOCIDA. Devuelve 0 si ya no estaba activa.</summary>
        public int mtdReconocer(Guid idAlerta, int? idUsuario)
        {
            const string sql = @"
UPDATE dbo.Alerta
SET EstadoAlerta = 'RECONOCIDA', FechaReconocida = dbo.fnAhora(), IdUsuarioReconoce = @IdUsuario
WHERE IdAlerta = @IdAlerta AND EstadoAlerta = 'ACTIVA';";

            return clsBaseDatos.mtdEjecutarComando(sql,
                clsBaseDatos.mtdParametro("@IdAlerta", idAlerta),
                clsBaseDatos.mtdParametro("@IdUsuario", idUsuario));
        }

        /// <summary>RECONOCIDA → CERRADA. Devuelve 0 si no estaba reconocida.</summary>
        public int mtdCerrar(Guid idAlerta, int? idUsuario)
        {
            const string sql = @"
UPDATE dbo.Alerta
SET EstadoAlerta = 'CERRADA', FechaCierre = dbo.fnAhora(), IdUsuarioCierra = @IdUsuario
WHERE IdAlerta = @IdAlerta AND EstadoAlerta = 'RECONOCIDA';";

            return clsBaseDatos.mtdEjecutarComando(sql,
                clsBaseDatos.mtdParametro("@IdAlerta", idAlerta),
                clsBaseDatos.mtdParametro("@IdUsuario", idUsuario));
        }

        /// <summary>Números para las tarjetas de la pantalla (una sola fila).</summary>
        public DataTable mtdResumen()
        {
            const string sql = @"
SELECT
    (SELECT COUNT(1) FROM dbo.Alerta WHERE EstadoAlerta = 'ACTIVA')      AS Activas,
    (SELECT COUNT(1) FROM dbo.Alerta WHERE EstadoAlerta = 'RECONOCIDA')  AS Reconocidas,
    (SELECT MAX(FechaHora) FROM dbo.Alerta WHERE EstadoAlerta = 'ACTIVA') AS UltimaActiva,
    (SELECT COUNT(1) FROM dbo.Evento
      WHERE Resultado = 'RECHAZADO' AND Detalle LIKE 'Confianza insuficiente%'
        AND FechaHora >= CAST(CAST(dbo.fnAhora() AS DATE) AS DATETIME2))   AS RechazosConfianzaHoy,
    (SELECT COUNT(1) FROM dbo.Alerta
      WHERE Tipo = 'ENLACE_ARDUINO'
        AND FechaHora >= CAST(CAST(dbo.fnAhora() AS DATE) AS DATETIME2))   AS FallasEnlaceHoy,
    (SELECT COUNT(1) FROM dbo.Alerta
      WHERE Tipo = 'ENLACE_ARDUINO' AND EstadoAlerta <> 'CERRADA')         AS FallasEnlaceAbiertas;";

            return clsBaseDatos.mtdConsultar(sql);
        }
    }
}
