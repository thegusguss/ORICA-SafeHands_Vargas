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
        public string Equipo { get; set; }          // "EQ-001 - Simulador principal" o null
        public string Comando { get; set; }         // frase del comando, lo que se escuchó o el tipo de evento
        public string TextoDetectado { get; set; }  // lo que entendió el reconocedor

        /// <summary>Resultado en palabras para el operador.</summary>
        public string ResultadoTexto
        {
            get
            {
                switch (Resultado)
                {
                    case "EJECUTADO": return "Ejecutado";
                    case "CANCELADO": return Detalle != null && Detalle.IndexOf("tiempo", System.StringComparison.OrdinalIgnoreCase) >= 0
                                             ? "Sin respuesta" : "Cancelado";
                    case "RECHAZADO": return "Rechazado";
                    case "ERROR": return "Error";
                    case "IGNORADO": return "Ignorado";
                    case "INFORMATIVO": return "Informativo";
                    default: return Resultado;
                }
            }
        }

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
