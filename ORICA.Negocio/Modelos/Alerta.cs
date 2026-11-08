using System;

namespace ORICA.Negocio.Modelos
{
    // Una alerta de dbo.Alerta. Estados: ACTIVA → RECONOCIDA → CERRADA.
    public class Alerta
    {
        public Guid IdAlerta { get; set; }
        public Guid? IdEvento { get; set; }
        public string Codigo { get; set; }
        public string Tipo { get; set; }            // COMANDO_RECHAZADO, CONFIRMACION_VENCIDA, PARADA_EQUIPO, ENLACE_ARDUINO...
        public string Severidad { get; set; }       // CRITICA, ALTA, MEDIA, BAJA, INFO
        public string Descripcion { get; set; }
        public string Estado { get; set; }          // ACTIVA, RECONOCIDA, CERRADA
        public DateTime FechaHora { get; set; }
        public DateTime? FechaReconocida { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string UsuarioReconoce { get; set; }
        public string UsuarioCierra { get; set; }
        public string UsuarioOrigen { get; set; }   // operador que provocó el evento (si hay)
        public string Equipo { get; set; }

        public bool EstaActiva { get { return Estado == "ACTIVA"; } }
        public bool EstaReconocida { get { return Estado == "RECONOCIDA"; } }
        public bool EstaCerrada { get { return Estado == "CERRADA"; } }

        public string EstadoTexto
        {
            get
            {
                switch (Estado)
                {
                    case "ACTIVA": return "Activa";
                    case "RECONOCIDA": return "Reconocida";
                    case "CERRADA": return "Cerrada";
                    default: return Estado;
                }
            }
        }

        public string SeveridadTexto
        {
            get
            {
                switch (Severidad)
                {
                    case "CRITICA": return "Crítica";
                    case "ALTA": return "Alta";
                    case "MEDIA": return "Media";
                    case "BAJA": return "Baja";
                    default: return "Info";
                }
            }
        }

        public string TipoTexto
        {
            get
            {
                switch (Tipo)
                {
                    case "COMANDO_RECHAZADO": return "Comando rechazado";
                    case "COMANDO_CANCELADO": return "Comando cancelado";
                    case "CONFIRMACION_VENCIDA": return "Confirmación vencida";
                    case "ERROR_EJECUCION": return "Error de ejecución";
                    case "PARADA_EQUIPO": return "Parada de equipo";
                    case "ENLACE_ARDUINO": return "Arduino desconectado";
                    default: return Tipo;
                }
            }
        }
    }

    // Números para las tarjetas de la pantalla de alertas.
    public class ResumenAlertas
    {
        public int Activas { get; set; }
        public int Reconocidas { get; set; }
        public DateTime? UltimaActiva { get; set; }
        public int RechazosConfianzaHoy { get; set; }
        public int FallasEnlaceHoy { get; set; }
        public int FallasEnlaceAbiertas { get; set; }
    }
}
