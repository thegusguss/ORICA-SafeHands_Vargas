using System;
using System.Data;
using System.Data.SqlClient;

namespace ORICA.Datos
{
    /// <summary>
    /// Acceso a datos del inicio de sesión: credenciales PIN y sesiones de operador.
    /// </summary>
    public class AutenticacionDatos
    {
        private const string CodigoTipoPin = "PIN";

        /// <summary>Usuarios activos que tienen al menos una credencial PIN activa.</summary>
        public DataTable ListarUsuariosConPin()
        {
            const string sql = @"
SELECT u.IdUsuario, u.Codigo, u.Nombres, u.Apellidos, u.IdRol, r.Nombre AS RolNombre, u.Estado
FROM dbo.Usuario u
INNER JOIN dbo.Rol r ON r.IdRol = u.IdRol
WHERE u.Estado = 1
  AND r.Estado = 1
  AND EXISTS (SELECT 1
              FROM dbo.Credencial c
              INNER JOIN dbo.TipoCredencial tc ON tc.IdTipoCredencial = c.IdTipoCredencial
              WHERE c.IdUsuario = u.IdUsuario
                AND c.Estado = 1
                AND tc.Codigo = @TipoPin)
ORDER BY u.Codigo;";

            return BaseDatos.Consultar(sql, new SqlParameter("@TipoPin", CodigoTipoPin));
        }

        /// <summary>Credencial PIN activa del usuario (si tiene varias, la más reciente).</summary>
        public DataTable ObtenerCredencialPin(int idUsuario)
        {
            const string sql = @"
SELECT TOP 1 c.IdCredencial, c.HashCredencial, c.Salt, c.IntentosFallidos
FROM dbo.Credencial c
INNER JOIN dbo.TipoCredencial tc ON tc.IdTipoCredencial = c.IdTipoCredencial
WHERE c.IdUsuario = @IdUsuario
  AND c.Estado = 1
  AND tc.Codigo = @TipoPin
ORDER BY c.IdCredencial DESC;";

            return BaseDatos.Consultar(sql,
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario },
                new SqlParameter("@TipoPin", CodigoTipoPin));
        }

        /// <summary>Suma un intento fallido y devuelve el total acumulado.</summary>
        public int RegistrarIntentoFallido(int idCredencial)
        {
            const string sql = @"
UPDATE dbo.Credencial
SET IntentosFallidos = IntentosFallidos + 1
OUTPUT INSERTED.IntentosFallidos
WHERE IdCredencial = @IdCredencial;";

            object total = BaseDatos.Escalar(sql,
                new SqlParameter("@IdCredencial", SqlDbType.Int) { Value = idCredencial });

            return Convert.ToInt32(total);
        }

        /// <summary>
        /// Reinicia los intentos, marca el último acceso y abre una sesión.
        /// Todo en una sola transacción. Devuelve el IdSesion generado.
        /// </summary>
        public Guid RegistrarAccesoExitoso(int idCredencial, int idUsuario, string metodo, string estacion)
        {
            const string sql = @"
SET XACT_ABORT ON;
BEGIN TRANSACTION;

UPDATE dbo.Credencial
SET IntentosFallidos = 0,
    UltimoUso = SYSDATETIME()
WHERE IdCredencial = @IdCredencial;

UPDATE dbo.Usuario
SET UltimoAcceso = SYSDATETIME()
WHERE IdUsuario = @IdUsuario;

DECLARE @Sesion TABLE (IdSesion UNIQUEIDENTIFIER);

INSERT INTO dbo.SesionOperador (IdUsuario, MetodoAutenticacion, Estacion)
OUTPUT INSERTED.IdSesion INTO @Sesion
VALUES (@IdUsuario, @Metodo, @Estacion);

COMMIT TRANSACTION;

SELECT IdSesion FROM @Sesion;";

            object id = BaseDatos.Escalar(sql,
                new SqlParameter("@IdCredencial", SqlDbType.Int) { Value = idCredencial },
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario },
                new SqlParameter("@Metodo", metodo),
                new SqlParameter("@Estacion", BaseDatos.TextoONulo(estacion)));

            return (Guid)id;
        }

        public int CerrarSesion(Guid idSesion)
        {
            const string sql = @"
UPDATE dbo.SesionOperador
SET FechaFin = SYSDATETIME(),
    EstadoSesion = 'CERRADA'
WHERE IdSesion = @IdSesion
  AND EstadoSesion = 'ABIERTA';";

            return BaseDatos.Ejecutar(sql,
                new SqlParameter("@IdSesion", SqlDbType.UniqueIdentifier) { Value = idSesion });
        }
    }
}
