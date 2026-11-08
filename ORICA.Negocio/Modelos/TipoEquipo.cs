namespace ORICA.Negocio.Modelos
{
    public class TipoEquipo
    {
        public int IdTipoEquipo { get; set; }
        public string Codigo { get; set; }     // "CTRL", "ACT", "IND", "SIM"...
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
