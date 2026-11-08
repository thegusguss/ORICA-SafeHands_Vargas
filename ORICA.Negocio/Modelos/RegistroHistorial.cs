using System;

namespace ORICA.Negocio.Modelos
{
    // Una fila del historial: puede venir de dbo.Evento (operación) o de dbo.Auditoria (cambios administrativos).
    public class RegistroHistorial
    {
        public DateTime FechaHora { get; set; }
        public string Fuente { get; set; }          // "EVENTO" o "AUDITORIA"
        public string Tipo { get; set; }            // TipoEvento o Acción de auditoría
        public string Usuario { get; set; }         // "OP-001 - Lucía Ramos" o "Sistema"
        public string Descripcion { get; set; }     // texto ya armado para mostrar
        public decimal? Confianza { get; set; }
        public int? IdNivelCriticidad { get; set; }
        public string Criticidad { get; set; }
        public string Resultado { get; set; }
        public string Detalle { get; set; }         // texto completo (para el doble clic / CSV)

        public string ConfianzaTexto
        {
            get { return Confianza.HasValue ? Confianza.Value.ToString("0.#") + " %" : "—"; }
        }

        public string CriticidadTexto
        {
            get { return IdNivelCriticidad.HasValue ? "N" + IdNivelCriticidad + " · " + Criticidad : "—"; }
        }
    }
}
