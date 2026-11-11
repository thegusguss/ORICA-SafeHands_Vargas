using System;
using System.Data;
using System.Data.SqlClient;

namespace ORICA.Datos
{
    public class clsCredencialDatos
    {
        public DataTable mtdListar()
        {
            const string sql = @"SELECT c.IdCredencial, c.Codigo, c.IdUsuario, c.IdTipoCredencial,
                               u.Codigo AS UsuarioCodigo, u.Nombres + ' ' + u.Apellidos AS Operador,
                               tc.Codigo AS Tipo, c.ReferenciaVisible, c.FechaRegistro, c.Estado, c.IntentosFallidos
                        FROM dbo.Credencial c
                        INNER JOIN dbo.Usuario u         ON u.IdUsuario = c.IdUsuario
                        INNER JOIN dbo.TipoCredencial tc ON tc.IdTipoCredencial = c.IdTipoCredencial
                        ORDER BY c.IdCredencial DESC;";

            return clsBaseDatos.mtdConsultar(sql);
        }

        public DataTable mtdListarTipos()
        {
            const string sql = @"SELECT IdTipoCredencial, Codigo, Nombre, RequiereSalt
                                FROM dbo.TipoCredencial
                                WHERE Estado = 1
                                ORDER BY IdTipoCredencial;";

            return clsBaseDatos.mtdConsultar(sql);
        }

        // ¿Ya hay una credencial activa de este tipo con el mismo hash? (misma tarjeta RFID/NFC)
        public bool mtdExisteHash(int idTipo, byte[] hash)
        {
            const string sql = @"SELECT COUNT(1) FROM dbo.Credencial
                                WHERE IdTipoCredencial = @IdTipo AND HashCredencial = @Hash AND Estado = 1;";

            object total = clsBaseDatos.mtdConsultarValor(sql,
                clsBaseDatos.mtdParametro("@IdTipo", idTipo),
                clsBaseDatos.mtdParametro("@Hash", hash));

            return Convert.ToInt32(total) > 0;
        }

        // ¿El usuario ya tiene una credencial activa de este tipo?
        public bool mtdTieneActiva(int idUsuario, int idTipo)
        {
            const string sql = @"SELECT COUNT(1) FROM dbo.Credencial
                                WHERE IdUsuario = @IdUsuario AND IdTipoCredencial = @IdTipo AND Estado = 1;";

            object total = clsBaseDatos.mtdConsultarValor(sql,
                clsBaseDatos.mtdParametro("@IdUsuario", idUsuario),
                clsBaseDatos.mtdParametro("@IdTipo", idTipo));

            return Convert.ToInt32(total) > 0;
        }

        // ---------------- Escritura ----------------

        // Desactiva la credencial anterior del mismo tipo (si la hay) e inserta la nueva,
        // todo en una transacción. Devuelve el IdCredencial nuevo.
        public int mtdRegistrar(string codigo, int idUsuario, int idTipo, byte[] hash,
                                byte[] salt, string referencia)
        {
            const string sql = @"SET XACT_ABORT ON;
                                BEGIN TRANSACTION;

                                UPDATE dbo.Credencial
                                SET Estado = 0, FechaModificacion = dbo.fnAhora()
                                WHERE IdUsuario = @IdUsuario AND IdTipoCredencial = @IdTipo AND Estado = 1;

                                INSERT INTO dbo.Credencial (Codigo, IdUsuario, IdTipoCredencial, HashCredencial, Salt, ReferenciaVisible)
                                OUTPUT INSERTED.IdCredencial
                                VALUES (@Codigo, @IdUsuario, @IdTipo, @Hash, @Salt, @Referencia);

                                COMMIT TRANSACTION;";

            object id = clsBaseDatos.mtdConsultarValor(sql,
                clsBaseDatos.mtdParametro("@Codigo", codigo),
                clsBaseDatos.mtdParametro("@IdUsuario", idUsuario),
                clsBaseDatos.mtdParametro("@IdTipo", idTipo),
                clsBaseDatos.mtdParametro("@Hash", hash),
                // Salt puede ser null (RFID/NFC). Se indica el tipo VarBinary porque SQL Server
                // no acepta un NULL sin tipo (lo toma como texto) en una columna binaria.
                new SqlParameter("@Salt", SqlDbType.VarBinary) { Value = (object)salt ?? DBNull.Value },
                clsBaseDatos.mtdParametro("@Referencia", referencia));

            return Convert.ToInt32(id);
        }

        public int mtdCambiarEstado(int idCredencial, bool estado)
        {
            const string sql = @"UPDATE dbo.Credencial
                                SET Estado = @Estado, FechaModificacion = dbo.fnAhora()
                                WHERE IdCredencial = @IdCredencial;";

            return clsBaseDatos.mtdEjecutarComando(sql,
                clsBaseDatos.mtdParametro("@IdCredencial", idCredencial),
                clsBaseDatos.mtdParametro("@Estado", estado));
        }

        // Pone los intentos fallidos en 0 para que el usuario pueda volver a entrar.
        public int mtdDesbloquear(int idCredencial)
        {
            const string sql = @"UPDATE dbo.Credencial
                                SET IntentosFallidos = 0, FechaModificacion = dbo.fnAhora()
                                WHERE IdCredencial = @IdCredencial;";

            return clsBaseDatos.mtdEjecutarComando(sql,
                clsBaseDatos.mtdParametro("@IdCredencial", idCredencial));
        }
    }
}
