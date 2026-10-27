using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS.Entidades
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public string Codigo { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string Documento { get; set; }

        public string Telefono { get; set; }

        public string Correo { get; set; }

        public int IdRol { get; set; }

        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public DateTime? UltimoAcceso { get; set; }

        public byte[] RowVersion { get; set; }
        public string RolNombre { get; set; }


        public string NombreCompleto
        {
            get
            {
                return Nombres + " " + Apellidos;
            }
        }

        public string EstadoTexto
        {
            get
            {
                return Estado ? "Activo" : "Inactivo";
            }
        }
    }
}
