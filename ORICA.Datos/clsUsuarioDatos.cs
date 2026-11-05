using System;
using System.Data;

namespace ORICA.Datos
{
    /// <summary>
    /// Acceso a datos de Usuario. Devuelve DataTable y recibe valores simples
    /// para evitar referencia circular con ORICA.Negocio (donde viven los modelos).
    /// </summary>
    public class clsUsuarioDatos
    {
        // SELECT común a todas las consultas de usuario.
        private const string SelectUsuarios = @"
SELECT u.IdUsuario, u.Codigo, u.Nombres, u.Apellidos, u.Documento, u.Telefono, u.Correo,
       u.IdRol, r.Nombre AS RolNombre, u.Estado, u.FechaRegistro, u.FechaModificacion,
       u.UltimoAcceso, u.RowVersion,
       -- Tipos de credencial activos del usuario, por ejemplo 'PIN, RFID' (NULL si no tiene).
       (SELECT STRING_AGG(tc.Codigo, ', ')
        FROM dbo.Credencial c
        INNER JOIN dbo.TipoCredencial tc ON tc.IdTipoCredencial = c.IdTipoCredencial
        WHERE c.IdUsuario = u.IdUsuario AND c.Estado = 1) AS Credenciales
FROM dbo.Usuario u
INNER JOIN dbo.Rol r ON r.IdRol = u.IdRol";

        // ---------------- Consultas ----------------

        public DataTable mtdListarUsuarios()
        {
            return clsBaseDatos.mtdConsultar(SelectUsuarios + " ORDER BY u.Codigo;");
        }

        public DataTable mtdBuscarUsuarios(string texto)
        {
            string sql = SelectUsuarios + @"
WHERE u.Codigo    LIKE @Texto
   OR u.Nombres   LIKE @Texto
   OR u.Apellidos LIKE @Texto
   OR u.Documento LIKE @Texto
   OR r.Nombre    LIKE @Texto
ORDER BY u.Codigo;";

            return clsBaseDatos.mtdConsultar(sql,
                clsBaseDatos.mtdParametro("@Texto", "%" + texto + "%"));
        }

        public DataTable mtdObtenerPorId(int idUsuario)
        {
            return clsBaseDatos.mtdConsultar(SelectUsuarios + " WHERE u.IdUsuario = @IdUsuario;",
                clsBaseDatos.mtdParametro("@IdUsuario", idUsuario));
        }

        // ---------------- Escritura ----------------

        /// <summary>Inserta y devuelve el IdUsuario generado.</summary>
        public int mtdRegistrar(string codigo, string nombres, string apellidos, string documento,
                                string telefono, string correo, int idRol, bool estado)
        {
            const string sql = @"
INSERT INTO dbo.Usuario (Codigo, Nombres, Apellidos, Documento, Telefono, Correo, IdRol, Estado, FechaRegistro)
OUTPUT INSERTED.IdUsuario
VALUES (@Codigo, @Nombres, @Apellidos, @Documento, @Telefono, @Correo, @IdRol, @Estado, SYSDATETIME());";

            object id = clsBaseDatos.mtdConsultarValor(sql,
                clsBaseDatos.mtdParametro("@Codigo", codigo),
                clsBaseDatos.mtdParametro("@Nombres", nombres),
                clsBaseDatos.mtdParametro("@Apellidos", apellidos),
                clsBaseDatos.mtdParametro("@Documento", documento),
                clsBaseDatos.mtdParametro("@Telefono", telefono),
                clsBaseDatos.mtdParametro("@Correo", correo),
                clsBaseDatos.mtdParametro("@IdRol", idRol),
                clsBaseDatos.mtdParametro("@Estado", estado));

            return Convert.ToInt32(id);
        }

        /// <summary>
        /// Actualiza solo si RowVersion no cambió (nadie más editó el registro).
        /// Devuelve 0 si otro usuario lo modificó antes o si ya no existe.
        /// </summary>
        public int mtdModificar(int idUsuario, string codigo, string nombres, string apellidos,
                                string documento, string telefono, string correo, int idRol,
                                byte[] rowVersion)
        {
            const string sql = @"
UPDATE dbo.Usuario
SET Codigo = @Codigo,
    Nombres = @Nombres,
    Apellidos = @Apellidos,
    Documento = @Documento,
    Telefono = @Telefono,
    Correo = @Correo,
    IdRol = @IdRol,
    FechaModificacion = SYSDATETIME()
WHERE IdUsuario = @IdUsuario
  AND RowVersion = @RowVersion;";

            return clsBaseDatos.mtdEjecutarComando(sql,
                clsBaseDatos.mtdParametro("@IdUsuario", idUsuario),
                clsBaseDatos.mtdParametro("@Codigo", codigo),
                clsBaseDatos.mtdParametro("@Nombres", nombres),
                clsBaseDatos.mtdParametro("@Apellidos", apellidos),
                clsBaseDatos.mtdParametro("@Documento", documento),
                clsBaseDatos.mtdParametro("@Telefono", telefono),
                clsBaseDatos.mtdParametro("@Correo", correo),
                clsBaseDatos.mtdParametro("@IdRol", idRol),
                clsBaseDatos.mtdParametro("@RowVersion", rowVersion));
        }

        public int mtdCambiarEstado(int idUsuario, bool estado)
        {
            const string sql = @"
UPDATE dbo.Usuario
SET Estado = @Estado,
    FechaModificacion = SYSDATETIME()
WHERE IdUsuario = @IdUsuario;";

            return clsBaseDatos.mtdEjecutarComando(sql,
                clsBaseDatos.mtdParametro("@IdUsuario", idUsuario),
                clsBaseDatos.mtdParametro("@Estado", estado));
        }

        // ---------------- Validación de duplicados ----------------
        // idExcluir: al modificar se pasa el Id del propio usuario para no chocar consigo mismo.

        public bool mtdExisteCodigo(string codigo, int? idExcluir = null)
        {
            return mtdExiste("Codigo", codigo, idExcluir);
        }

        public bool mtdExisteDocumento(string documento, int? idExcluir = null)
        {
            return mtdExiste("Documento", documento, idExcluir);
        }

        public bool mtdExisteCorreo(string correo, int? idExcluir = null)
        {
            return mtdExiste("Correo", correo, idExcluir);
        }

        // 'columna' solo llega desde los tres métodos de arriba (valores fijos),
        // nunca desde la interfaz; por eso es seguro ponerla dentro del texto SQL.
        private bool mtdExiste(string columna, string valor, int? idExcluir)
        {
            if (string.IsNullOrWhiteSpace(valor)) return false;

            string sql = "SELECT COUNT(1) FROM dbo.Usuario WHERE " + columna + " = @Valor" +
                         " AND (@IdExcluir IS NULL OR IdUsuario <> @IdExcluir);";

            object total = clsBaseDatos.mtdConsultarValor(sql,
                clsBaseDatos.mtdParametro("@Valor", valor),
                clsBaseDatos.mtdParametro("@IdExcluir", idExcluir));

            return Convert.ToInt32(total) > 0;
        }
    }
}
