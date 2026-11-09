using System;
using System.Collections.Generic;

namespace ORICA.Negocio.Modelos
{
    // Comando de voz autorizado (gramática cerrada) y su política de criticidad.
    public class Comando
    {
        public int IdComando { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Frase { get; set; }               // en MAYÚSCULAS y sin tildes, ej. "ENCENDER INDICADOR"
        public int IdNivelCriticidad { get; set; }
        public string CriticidadNombre { get; set; }
        public int NumeroConfirmaciones { get; set; }   // viene del nivel de criticidad
        public int TimeoutSegundos { get; set; }        // viene del nivel de criticidad
        public int IdTipoAccion { get; set; }
        public string TipoAccionNombre { get; set; }
        public string TipoAccionCodigo { get; set; }    // INFORMATIVA, INDICADOR, OPERAR, DETENER, CRITICA, ESTADO_SEGURO
        public decimal ConfianzaMinima { get; set; }    // 0 a 100
        public bool RequierePTT { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public byte[] RowVersion { get; set; }

        // Equipos a los que está asignado (solo se llena cuando se edita).
        public List<int> IdsEquipos { get; set; } = new List<int>();

        // Propiedades auxiliares para la interfaz
        public string EstadoTexto
        {
            get { return Estado ? "Activo" : "Inactivo"; }
        }

        public string CriticidadTexto
        {
            get { return "N" + IdNivelCriticidad + " · " + CriticidadNombre; }
        }

        public string ConfirmacionTexto
        {
            get
            {
                if (NumeroConfirmaciones <= 0) return "Sin confirmación";
                if (NumeroConfirmaciones == 1) return "Requiere confirmación";
                return "Requiere doble confirmación";
            }
        }

        public override string ToString()
        {
            return Frase;
        }
    }
}
