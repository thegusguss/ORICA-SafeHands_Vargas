using System.Data;

namespace ORICA.Datos
{
    public class clsRolDatos
    {
        public DataTable mtdListarRolesActivos()
        {
            const string sql = @"
SELECT IdRol, Codigo, Nombre, Descripcion, Estado, FechaRegistro
FROM dbo.Rol
WHERE Estado = 1
ORDER BY IdRol;";

            return clsBaseDatos.mtdConsultar(sql);
        }
    }
}
