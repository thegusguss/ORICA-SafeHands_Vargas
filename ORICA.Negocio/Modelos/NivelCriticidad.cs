namespace ORICA.Negocio.Modelos
{
    // Nivel de criticidad (N1 a N4): cuántas confirmaciones pide un comando y cuánto se espera cada una.
    public class NivelCriticidad
    {
        public int IdNivelCriticidad { get; set; }   // 1 a 4 (fijo en la BD)
        public string Codigo { get; set; }           // "N1".."N4"
        public string Nombre { get; set; }
        public int NumeroConfirmaciones { get; set; } // 0, 1 o 2
        public int TimeoutSegundos { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }

        // Propiedades auxiliares para la interfaz
        public string NivelTexto
        {
            get { return "Nivel " + IdNivelCriticidad; }
        }

        public string TimeoutTexto
        {
            get { return NumeroConfirmaciones == 0 || TimeoutSegundos == 0 ? "—" : TimeoutSegundos + " s"; }
        }

        public string Etiqueta
        {
            get { return Codigo + " - " + Nombre; }
        }

        public override string ToString()
        {
            return Etiqueta;
        }
    }
}
