using System;
using System.Data;

namespace ORICA.Datos
{
    // Acceso a datos de los equipos (activos simulados) y sus tipos.
    public class clsEquipoDatos
    {
        private const string SelectEquipos = @"
SELECT e.IdEquipo, e.Codigo, e.Nombre, e.IdTipoEquipo, te.Nombre AS TipoNombre,
       e.Descripcion, e.IdentificadorHardware, e.UbicacionReferencia, e.Estado,
       e.UltimoHeartbeat, e.FechaRegistro, e.FechaModificacion, e.RowVersion
FROM dbo.Equipo e
INNER JOIN dbo.TipoEquipo te ON te.IdTipoEquipo = e.IdTipoEquipo";

        // ---------------- Consultas ----------------

        public DataTable mtdListar()
        {
            return clsBaseDatos.mtdConsultar(SelectEquipos + " ORDER BY e.Codigo;");
        }

        public DataTable mtdBuscar(string texto)
        {
            string sql = SelectEquipos + @"
WHERE e.Codigo LIKE @Texto
   OR e.Nombre LIKE @Texto
   OR te.Nombre LIKE @Texto
   OR e.UbicacionReferencia LIKE @Texto
ORDER BY e.Codigo;";

            return clsBaseDatos.mtdConsultar(sql,
                clsBaseDatos.mtdParametro("@Texto", "%" + texto + "%"));
        }

        public DataTable mtdListarTipos()
        {
            const string sql = @"
SELECT IdTipoEquipo, Codigo, Nombre, Descripcion
FROM dbo.TipoEquipo
WHERE Estado = 1
ORDER BY Nombre;";

            return clsBaseDatos.mtdConsultar(sql);
        }

        // ---------------- Escritura ----------------

        public int mtdRegistrar(string codigo, string nombre, int idTipo, string descripcion,
                                string identificadorHardware, string ubicacion)
        {
            const string sql = @"
INSERT INTO dbo.Equipo (Codigo, Nombre, IdTipoEquipo, Descripcion, IdentificadorHardware, UbicacionReferencia)
OUTPUT INSERTED.IdEquipo
VALUES (@Codigo, @Nombre, @IdTipo, @Descripcion, @Hardware, @Ubicacion);";

            object id = clsBaseDatos.mtdConsultarValor(sql,
                clsBaseDatos.mtdParametro("@Codigo", codigo),
                clsBaseDatos.mtdParametro("@Nombre", nombre),
                clsBaseDatos.mtdParametro("@IdTipo", idTipo),
                clsBaseDatos.mtdParametro("@Descripcion", descripcion),
                clsBaseDatos.mtdParametro("@Hardware", identificadorHardware),
                clsBaseDatos.mtdParametro("@Ubicacion", ubicacion));

            return Convert.ToInt32(id);
        }

        /// <summary>Actualiza solo si RowVersion no cambió. Devuelve 0 si alguien lo modificó antes.</summary>
        public int mtdModificar(int idEquipo, string codigo, string nombre, int idTipo, string descripcion,
                                string identificadorHardware, string ubicacion, byte[] rowVersion)
        {
            const string sql = @"
UPDATE dbo.Equipo
SET Codigo = @Codigo,
    Nombre = @Nombre,
    IdTipoEquipo = @IdTipo,
    Descripcion = @Descripcion,
    IdentificadorHardware = @Hardware,
    UbicacionReferencia = @Ubicacion,
    FechaModificacion = dbo.fnAhora()
WHERE IdEquipo = @IdEquipo
  AND RowVersion = @RowVersion;";

            return clsBaseDatos.mtdEjecutarComando(sql,
                clsBaseDatos.mtdParametro("@IdEquipo", idEquipo),
                clsBaseDatos.mtdParametro("@Codigo", codigo),
                clsBaseDatos.mtdParametro("@Nombre", nombre),
                clsBaseDatos.mtdParametro("@IdTipo", idTipo),
                clsBaseDatos.mtdParametro("@Descripcion", descripcion),
                clsBaseDatos.mtdParametro("@Hardware", identificadorHardware),
                clsBaseDatos.mtdParametro("@Ubicacion", ubicacion),
                clsBaseDatos.mtdParametro("@RowVersion", rowVersion));
        }

        public int mtdCambiarEstado(int idEquipo, bool estado)
        {
            const string sql = @"
UPDATE dbo.Equipo
SET Estado = @Estado, FechaModificacion = dbo.fnAhora()
WHERE IdEquipo = @IdEquipo;";

            return clsBaseDatos.mtdEjecutarComando(sql,
                clsBaseDatos.mtdParametro("@IdEquipo", idEquipo),
                clsBaseDatos.mtdParametro("@Estado", estado));
        }

        // ---------------- Validación de duplicados ----------------

        public bool mtdExisteCodigo(string codigo, int? idExcluir)
        {
            return mtdExiste("Codigo", codigo, idExcluir);
        }

        public bool mtdExisteHardware(string identificador, int? idExcluir)
        {
            return mtdExiste("IdentificadorHardware", identificador, idExcluir);
        }

        // 'columna' solo llega desde los métodos de arriba (valores fijos), nunca desde la interfaz.
        private bool mtdExiste(string columna, string valor, int? idExcluir)
        {
            if (string.IsNullOrWhiteSpace(valor)) return false;

            string sql = "SELECT COUNT(1) FROM dbo.Equipo WHERE " + columna + " = @Valor" +
                         " AND (@IdExcluir IS NULL OR IdEquipo <> @IdExcluir);";

            object total = clsBaseDatos.mtdConsultarValor(sql,
                clsBaseDatos.mtdParametro("@Valor", valor),
                clsBaseDatos.mtdParametro("@IdExcluir", idExcluir));

            return Convert.ToInt32(total) > 0;
        }
    }
}
