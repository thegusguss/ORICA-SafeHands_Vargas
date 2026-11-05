namespace ORICA.Negocio.Modelos
{
    public class TipoCredencial
    {
        public int IdTipoCredencial { get; set; }
        public string Codigo { get; set; }     // "PIN", "RFID", "NFC"
        public string Nombre { get; set; }
        public bool RequiereSalt { get; set; }

        public bool EsPin
        {
            get { return Codigo == "PIN"; }
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
