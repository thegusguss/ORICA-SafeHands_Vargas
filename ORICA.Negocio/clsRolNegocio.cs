using System.Collections.Generic;
using System.Data;
using ORICA.Datos;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    public class clsRolNegocio
    {
        private readonly clsRolDatos _datos = new clsRolDatos();

        public List<Rol> mtdListarRolesActivos()
        {
            List<Rol> lista = new List<Rol>();

            foreach (DataRow r in _datos.mtdListarRolesActivos().Rows)
            {
                lista.Add(new Rol
                {
                    IdRol = clsLectura.mtdLeerEntero(r, "IdRol"),
                    Codigo = clsLectura.mtdLeerTexto(r, "Codigo"),
                    Nombre = clsLectura.mtdLeerTexto(r, "Nombre"),
                    Descripcion = clsLectura.mtdLeerTexto(r, "Descripcion"),
                    Estado = clsLectura.mtdLeerBooleano(r, "Estado"),
                    FechaRegistro = clsLectura.mtdLeerFecha(r, "FechaRegistro")
                });
            }

            return lista;
        }

        public bool mtdEsRolValido(int idRol)
        {
            return idRol > 0 && mtdListarRolesActivos().Exists(r => r.IdRol == idRol);
        }
    }
}
