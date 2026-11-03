using System.Data;

namespace ORICA.Datos
{
    public class RolDatos
    {
        public DataTable ListarActivos()
        {
            const string sql = @"
SELECT IdRol, Codigo, Nombre, Descripcion, Estado, FechaRegistro
FROM dbo.Rol
WHERE Estado = 1
ORDER BY IdRol;";

            return BaseDatos.Consultar(sql);
        }
    }
}
