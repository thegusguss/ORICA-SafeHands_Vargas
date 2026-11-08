using System;

namespace ORICA.Negocio.Modelos
{
    // Un equipo (activo simulado) que puede recibir comandos.
    public class Equipo
    {
        public int IdEquipo { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int IdTipoEquipo { get; set; }
        public string TipoNombre { get; set; }
        public string Descripcion { get; set; }
        public string IdentificadorHardware { get; set; }
        public string UbicacionReferencia { get; set; }
        public bool Estado { get; set; }
        public DateTime? UltimoHeartbeat { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public byte[] RowVersion { get; set; }

        // Propiedades auxiliares para la interfaz
        public string EstadoTexto
        {
            get { return Estado ? "Activo" : "Inactivo"; }
        }

        public string HeartbeatTexto
        {
            get { return UltimoHeartbeat.HasValue ? UltimoHeartbeat.Value.ToString("dd/MM/yyyy HH:mm:ss") : "Sin señal"; }
        }

        /// <summary>Texto para listas de selección, p. ej. "EQ-001 - Simulador principal".</summary>
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
