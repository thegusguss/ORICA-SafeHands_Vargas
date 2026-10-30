using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    public class ComandoDisponible
    {
        public string Frase { get; set; }
        public string Descripcion { get; set; }
        public string Confirmacion { get; set; }
        public bool RequiereConfirmacion { get; set; }
    }
}
