using System;

namespace ORICA.Negocio.Modelos
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
