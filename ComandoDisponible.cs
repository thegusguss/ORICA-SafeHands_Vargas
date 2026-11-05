namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    /// <summary>Un comando de voz que se muestra en la ventana "Comandos disponibles".</summary>
    public class ComandoDisponible
    {
        public string Frase { get; set; }
        public string Descripcion { get; set; }
        public string Confirmacion { get; set; }
        public bool RequiereConfirmacion { get; set; }
    }
}
