namespace ORICA.Negocio.Modelos
{
    public class TipoAccion
    {
        public int IdTipoAccion { get; set; }
        public string Codigo { get; set; }        // "INFORMATIVA", "OPERAR", "DETENER"...
        public string Nombre { get; set; }
        public bool EsAccionSegura { get; set; }  // lleva al equipo a un estado seguro (p. ej. detener)

        public override string ToString()
        {
            return Nombre;
        }
    }
}
