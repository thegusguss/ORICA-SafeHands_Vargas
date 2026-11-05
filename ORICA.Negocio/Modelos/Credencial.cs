using System;

namespace ORICA.Negocio.Modelos
{
    // Una credencial (PIN, RFID o NFC) de un usuario. El hash nunca sale de la base.
    public class Credencial
    {
        public int IdCredencial { get; set; }
        public string Codigo { get; set; }
        public int IdUsuario { get; set; }
        public int IdTipoCredencial { get; set; }
        public string UsuarioCodigo { get; set; }
        public string Operador { get; set; }
        public string Tipo { get; set; }          // "PIN", "RFID" o "NFC"
        public string Referencia { get; set; }    // texto visible, ej. "PIN ****" o "UID ****A1B2"
        public DateTime? FechaRegistro { get; set; }
        public bool Estado { get; set; }
        public int IntentosFallidos { get; set; }

        // Propiedades auxiliares para la interfaz
        public bool Bloqueada
        {
            get { return Estado && IntentosFallidos >= clsAutenticacionNegocio.MaxIntentosFallidos; }
        }

        public string EstadoTexto
        {
            get
            {
                if (Bloqueada) return "Bloqueada";
                return Estado ? "Activa" : "Inactiva";
            }
        }
    }
}
