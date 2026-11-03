using System.Collections.Generic;
using System.Data;
using ORICA.Datos;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    public class RolNegocio
    {
        private readonly RolDatos _datos = new RolDatos();

        public List<Rol> ListarActivos()
        {
            List<Rol> lista = new List<Rol>();

            foreach (DataRow r in _datos.ListarActivos().Rows)
            {
                lista.Add(new Rol
                {
                    IdRol = Lectura.Entero(r, "IdRol"),
                    Codigo = Lectura.Texto(r, "Codigo"),
                    Nombre = Lectura.Texto(r, "Nombre"),
                    Descripcion = Lectura.Texto(r, "Descripcion"),
                    Estado = Lectura.Booleano(r, "Estado"),
                    FechaRegistro = Lectura.Fecha(r, "FechaRegistro")
                });
            }

            return lista;
        }

        public bool EsRolValido(int idRol)
        {
            return idRol > 0 && ListarActivos().Exists(r => r.IdRol == idRol);
        }
    }
}
