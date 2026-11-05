using System;
using System.Data;

namespace ORICA.Datos
{
    /// <summary>
    /// Acceso a datos del inicio de sesión: credenciales PIN y sesiones de operador.
    /// </summary>
    public class clsAutenticacionDatos
    {
        /// <summary>Usuarios activos que tienen al menos una credencial PIN activa.</summary>
        public DataTable mtdListarUsuariosConPin()
        {
            const string sql = @"SELECT u.IdUsuario, u.Codigo, u.Nombres, u.Apellidos, u.IdRol, r.Codigo AS RolCodigo, r.Nombre AS RolNombre, u.Estado
FROM dbo.Usuario u
INNER JOIN dbo.Rol r ON r.IdRol = u.IdRol
WHERE u.Estado = 1
  AND r.Estado = 1
  AND EXISTS (SELECT 1
              FROM dbo.Credencial c
              INNER JOIN dbo.TipoCredencial tc ON tc.IdTipoCredencial = c.IdTipoCredencial
              WHERE c.IdUsuario = u.IdUsuario
                AND c.Estado = 1
                AND tc.Codigo = 'PIN')
ORDER BY u.Codigo;";

            return clsBaseDatos.mtdConsultar(sql);
        }

        /// <summary>Credencial PIN activa del usuario (si tiene varias, la más reciente).</summary>
        public DataTable mtdObtenerCredencialPin(int idUsuario)
        {
            const string sql = @"
SELECT TOP 1 c.IdCredencial, c.HashCredencial, c.Salt, c.IntentosFallidos
FROM dbo.Credencial c
INNER JOIN dbo.TipoCredencial tc ON tc.IdTipoCredencial = c.IdTipoCredencial
WHERE c.IdUsuario = @IdUsuario
  AND c.Estado = 1
  AND tc.Codigo = 'PIN'
ORDER BY c.IdCredencial DESC;";

            return clsBaseDatos.mtdConsultar(sql,
                clsBaseDatos.mtdParametro("@IdUsuario", idUsuario));
        }

        /// <summary>Suma un intento fallido y devuelve el total acumulado.</summary>
        public int mtdRegistrarIntentoFallido(int idCredencial)
        {
            const string sql = @"
UPDATE dbo.Credencial
SET IntentosFallidos = IntentosFallidos + 1
OUTPUT INSERTED.IntentosFallidos
WHERE IdCredencial = @IdCredencial;";

            object total = clsBaseDatos.mtdConsultarValor(sql,
                clsBaseDatos.mtdParametro("@IdCredencial", idCredencial));

            return Convert.ToInt32(total);
        }

        /// <summary>
        /// Reinicia los intentos, marca el último acceso y abre una sesión,
        /// todo en una sola transacción. Devuelve el IdSesion generado.
        /// </summary>
        public Guid mtdRegistrarAccesoExitoso(int idCredencial, int idUsuario, string metodo, string estacion)
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

            object id = clsBaseDatos.mtdConsultarValor(sql,
                clsBaseDatos.mtdParametro("@IdCredencial", idCredencial),
                clsBaseDatos.mtdParametro("@IdUsuario", idUsuario),
                clsBaseDatos.mtdParametro("@Metodo", metodo),
                clsBaseDatos.mtdParametro("@Estacion", estacion));

            return (Guid)id;
        }

        public int mtdCerrarSesion(Guid idSesion)
        {
            const string sql = @"
UPDATE dbo.SesionOperador
SET FechaFin = SYSDATETIME(),
    EstadoSesion = 'CERRADA'
WHERE IdSesion = @IdSesion
  AND EstadoSesion = 'ABIERTA';";

            return clsBaseDatos.mtdEjecutarComando(sql,
                clsBaseDatos.mtdParametro("@IdSesion", idSesion));
        }
    }
}
