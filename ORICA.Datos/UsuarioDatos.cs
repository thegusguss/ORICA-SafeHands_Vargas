using System;
using System.Data;
using System.Data.SqlClient;

namespace ORICA.Datos
{
    /// <summary>
    /// Acceso a datos de Usuario. Devuelve DataTable y recibe valores simples
    /// para evitar referencia circular con ORICA.Negocio (donde viven los modelos).
    /// </summary>
    public class UsuarioDatos
    {
        private const string SelectBase = @"
SELECT u.IdUsuario, u.Codigo, u.Nombres, u.Apellidos, u.Documento, u.Telefono, u.Correo,
       u.IdRol, r.Nombre AS RolNombre, u.Estado, u.FechaRegistro, u.FechaModificacion,
       u.UltimoAcceso, u.RowVersion
FROM dbo.Usuario u
INNER JOIN dbo.Rol r ON r.IdRol = u.IdRol";

        // ---------------- Consultas ----------------

        public DataTable Listar()
        {
            return BaseDatos.Consultar(SelectBase + " ORDER BY u.Codigo;");
        }

        public DataTable Buscar(string texto)
        {
            string sql = SelectBase + @"
WHERE u.Codigo    LIKE @Texto
   OR u.Nombres   LIKE @Texto
   OR u.Apellidos LIKE @Texto
   OR u.Documento LIKE @Texto
   OR r.Nombre    LIKE @Texto
ORDER BY u.Codigo;";

            return BaseDatos.Consultar(sql, new SqlParameter("@Texto", "%" + texto + "%"));
        }

        public DataTable ObtenerPorId(int idUsuario)
        {
            return BaseDatos.Consultar(SelectBase + " WHERE u.IdUsuario = @IdUsuario;",
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario });
        }

        // ---------------- Escritura ----------------

        /// <summary>Inserta y devuelve el IdUsuario generado.</summary>
        public int Registrar(string codigo, string nombres, string apellidos, string documento,
                             string telefono, string correo, int idRol, bool estado)
        {
            const string sql = @"
INSERT INTO dbo.Usuario (Codigo, Nombres, Apellidos, Documento, Telefono, Correo, IdRol, Estado, FechaRegistro)
OUTPUT INSERTED.IdUsuario
VALUES (@Codigo, @Nombres, @Apellidos, @Documento, @Telefono, @Correo, @IdRol, @Estado, SYSDATETIME());";

            object id = BaseDatos.Escalar(sql,
                new SqlParameter("@Codigo", codigo),
                new SqlParameter("@Nombres", nombres),
                new SqlParameter("@Apellidos", apellidos),
                new SqlParameter("@Documento", BaseDatos.TextoONulo(documento)),
                new SqlParameter("@Telefono", BaseDatos.TextoONulo(telefono)),
                new SqlParameter("@Correo", BaseDatos.TextoONulo(correo)),
                new SqlParameter("@IdRol", SqlDbType.Int) { Value = idRol },
                new SqlParameter("@Estado", SqlDbType.Bit) { Value = estado });

            return Convert.ToInt32(id);
        }

        /// <summary>
        /// Actualiza usando RowVersion (concurrencia optimista).
        /// Devuelve 0 si otro usuario modificó el registro antes o si ya no existe.
        /// </summary>
        public int Modificar(int idUsuario, string codigo, string nombres, string apellidos,
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

            return BaseDatos.Ejecutar(sql,
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario },
                new SqlParameter("@Codigo", codigo),
                new SqlParameter("@Nombres", nombres),
                new SqlParameter("@Apellidos", apellidos),
                new SqlParameter("@Documento", BaseDatos.TextoONulo(documento)),
                new SqlParameter("@Telefono", BaseDatos.TextoONulo(telefono)),
                new SqlParameter("@Correo", BaseDatos.TextoONulo(correo)),
                new SqlParameter("@IdRol", SqlDbType.Int) { Value = idRol },
                new SqlParameter("@RowVersion", SqlDbType.Timestamp) { Value = rowVersion });
        }

        public int CambiarEstado(int idUsuario, bool estado)
        {
            const string sql = @"
UPDATE dbo.Usuario
SET Estado = @Estado,
    FechaModificacion = SYSDATETIME()
WHERE IdUsuario = @IdUsuario;";

            return BaseDatos.Ejecutar(sql,
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario },
                new SqlParameter("@Estado", SqlDbType.Bit) { Value = estado });
        }

        // ---------------- Validación de duplicados ----------------
        // idExcluir: al modificar, se pasa el Id del propio usuario para no chocar consigo mismo.

        public bool ExisteCodigo(string codigo, int? idExcluir = null)
        {
            return Existe("Codigo", codigo, idExcluir);
        }

        public bool ExisteDocumento(string documento, int? idExcluir = null)
        {
            return Existe("Documento", documento, idExcluir);
        }

        public bool ExisteCorreo(string correo, int? idExcluir = null)
        {
            return Existe("Correo", correo, idExcluir);
        }

        /// <summary>
        /// 'columna' solo se recibe desde los métodos de arriba (valores fijos),
        /// nunca desde la interfaz, por eso es seguro concatenarla.
        /// </summary>
        private bool Existe(string columna, string valor, int? idExcluir)
        {
            if (string.IsNullOrWhiteSpace(valor)) return false;

            string sql = $@"
SELECT COUNT(1)
FROM dbo.Usuario
WHERE {columna} = @Valor
  AND (@IdExcluir IS NULL OR IdUsuario <> @IdExcluir);";

            object total = BaseDatos.Escalar(sql,
                new SqlParameter("@Valor", valor),
                new SqlParameter("@IdExcluir", SqlDbType.Int) { Value = (object)idExcluir ?? DBNull.Value });

            return Convert.ToInt32(total) > 0;
        }
    }
}
