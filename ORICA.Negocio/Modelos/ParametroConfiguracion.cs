using System;

namespace ORICA.Negocio.Modelos
{
    // Un parámetro de dbo.Configuracion (clave/valor).
    public class ParametroConfiguracion
    {
        public int IdConfiguracion { get; set; }
        public string Categoria { get; set; }     // VOZ, COMUNICACION, SEGURIDAD, PERSISTENCIA
        public string Clave { get; set; }         // UMBRAL_CONFIANZA, BAUD_RATE...
        public string Valor { get; set; }
        public string TipoDato { get; set; }      // STRING, INTEGER, DECIMAL, BOOLEAN
        public string Descripcion { get; set; }
        public bool EsEditable { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
