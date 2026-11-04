using System;
using System.Data;
using System.Data.SqlClient;

namespace ORICA.Datos
{
    /// <summary>
    /// Acceso a datos de Usuario. Devuelve DataTable y recibe valores simples
    /// para evitar referencia circular con ORICA.Negocio (donde viven los modelos).
    /// </summary>
    public class clsUsuarioDatos
    {
        private const string SelectBase = @"
SELECT u.IdUsuario, u.Codigo, u.Nombres, u.Apellidos, u.Documento, u.Telefono, u.Correo,
       u.IdRol, r.Nombre AS RolNombre, u.Estado, u.FechaRegistro, u.FechaModificacion,
       u.UltimoAcceso, u.RowVersion
FROM dbo.Usuario u
INNER JOIN dbo.Rol r ON r.IdRol = u.IdRol";

        // ---------------- Consultas ----------------

        public DataTable mtdListarUsuarios()
        {
            return clsBaseDatos.mtdConsultar(SelectBase + " ORDER BY u.Codigo;");
        }

        public DataTable mtdBuscarUsuarios(string texto)
        {
            string sql = SelectBase + @"
WHERE u.Codigo    LIKE @Texto
   OR u.Nombres   LIKE @Texto
   OR u.Apellidos LIKE @Texto
   OR u.Documento LIKE @Texto
   OR r.Nombre    LIKE @Texto
ORDER BY u.Codigo;";

            return clsBaseDatos.mtdConsultar(sql, new SqlParameter("@Texto", "%" + texto + "%"));
        }

        public DataTable mtdObtenerPorId(int idUsuario)
        {
            return clsBaseDatos.mtdConsultar(SelectBase + " WHERE u.IdUsuario = @IdUsuario;",
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario });
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
                new SqlParameter("@Codigo", codigo),
                new SqlParameter("@Nombres", nombres),
                new SqlParameter("@Apellidos", apellidos),
                new SqlParameter("@Documento", clsBaseDatos.mtdTextoParaSql(documento)),
                new SqlParameter("@Telefono", clsBaseDatos.mtdTextoParaSql(telefono)),
                new SqlParameter("@Correo", clsBaseDatos.mtdTextoParaSql(correo)),
                new SqlParameter("@IdRol", SqlDbType.Int) { Value = idRol },
                new SqlParameter("@Estado", SqlDbType.Bit) { Value = estado });

            return Convert.ToInt32(id);
        }

        /// <summary>
        /// Actualiza usando RowVersion (concurrencia optimista).
        /// Devuelve 0 si otro usuario modificó el registro antes o si ya no existe.
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
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario },
                new SqlParameter("@Codigo", codigo),
                new SqlParameter("@Nombres", nombres),
                new SqlParameter("@Apellidos", apellidos),
                new SqlParameter("@Documento", clsBaseDatos.mtdTextoParaSql(documento)),
                new SqlParameter("@Telefono", clsBaseDatos.mtdTextoParaSql(telefono)),
                new SqlParameter("@Correo", clsBaseDatos.mtdTextoParaSql(correo)),
                new SqlParameter("@IdRol", SqlDbType.Int) { Value = idRol },
                new SqlParameter("@RowVersion", SqlDbType.Timestamp) { Value = rowVersion });
        }

        public int mtdCambiarEstado(int idUsuario, bool estado)
        {
            const string sql = @"
UPDATE dbo.Usuario
SET Estado = @Estado,
    FechaModificacion = SYSDATETIME()
WHERE IdUsuario = @IdUsuario;";

            return clsBaseDatos.mtdEjecutarComando(sql,
                new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = idUsuario },
                new SqlParameter("@Estado", SqlDbType.Bit) { Value = estado });
        }

        // ---------------- Validación de duplicados ----------------
        // idExcluir: al modificar, se pasa el Id del propio usuario para no chocar consigo mismo.

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

        /// <summary>
        /// 'columna' solo se recibe desde los métodos de arriba (valores fijos),
        /// nunca desde la interfaz, por eso es seguro concatenarla.
        /// </summary>
        private bool mtdExiste(string columna, string valor, int? idExcluir)
        {
            if (string.IsNullOrWhiteSpace(valor)) return false;

            string sql = $@"
SELECT COUNT(1)
FROM dbo.Usuario
WHERE {columna} = @Valor
  AND (@IdExcluir IS NULL OR IdUsuario <> @IdExcluir);";

            object total = clsBaseDatos.mtdConsultarValor(sql,
                new SqlParameter("@Valor", valor),
                new SqlParameter("@IdExcluir", SqlDbType.Int) { Value = (object)idExcluir ?? DBNull.Value });

            return Convert.ToInt32(total) > 0;
        }
    }
}
