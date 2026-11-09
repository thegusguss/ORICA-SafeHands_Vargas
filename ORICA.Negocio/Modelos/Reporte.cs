using System;
using System.Collections.Generic;

namespace ORICA.Negocio.Modelos
{
    // Filtros que se eligen en la pantalla de Reportes. Un valor null significa "todos".
    public class FiltroReporte
    {
        public DateTime Desde { get; set; }       // día inicial (se usa solo la fecha)
        public DateTime Hasta { get; set; }       // día final, incluido
        public int? IdUsuario { get; set; }
        public string Resultado { get; set; }     // "EJECUTADO", "RECHAZADO"...
        public int? IdNivel { get; set; }
        public int? IdEquipo { get; set; }

        // Textos de los filtros elegidos, para mostrarlos en el reporte impreso.
        public string OperadorTexto { get; set; } = "Todos";
        public string ResultadoTexto { get; set; } = "Todos";
        public string NivelTexto { get; set; } = "Todos";
        public string EquipoTexto { get; set; } = "Todos";
    }

    // Una fila del reporte (un evento de voz o una parada).
    public class FilaReporte
    {
        public DateTime FechaHora { get; set; }
        public string Tipo { get; set; }          // TipoEvento
        public string Operador { get; set; }      // "OP-001 - Lucía Ramos" o "Sistema"
        public string Comando { get; set; }       // frase, texto escuchado o "Parada"
        public string Equipo { get; set; }        // "EQ-001" o "—"
        public string Nivel { get; set; }         // "N3" o "—"
        public decimal? Confianza { get; set; }   // 0 a 100
        public string Resultado { get; set; }

        public string ConfianzaTexto
        {
            get { return Confianza.HasValue ? Confianza.Value.ToString("0.#") + " %" : "—"; }
        }
    }

    // Confianza promedio de un comando (para el gráfico y la tabla impresa).
    public class ConfianzaComando
    {
        public string Comando { get; set; }
        public decimal Promedio { get; set; }
        public int Cantidad { get; set; }
    }

    // Todo lo que muestra la pantalla de Reportes.
    public class ResultadoReporte
    {
        public FiltroReporte Filtro { get; set; }
        public List<FilaReporte> Filas { get; set; } = new List<FilaReporte>();
        public bool Recortado { get; set; }       // true si había más filas que el máximo

        public int Total { get; set; }
        public int Ejecutados { get; set; }
        public int Rechazados { get; set; }
        public int Cancelados { get; set; }

        // Cantidad por resultado, en orden: Ejecutado, Rechazado, Cancelado, Error, Ignorado...
        public List<KeyValuePair<string, int>> PorResultado { get; set; } = new List<KeyValuePair<string, int>>();
        public List<ConfianzaComando> ConfianzaPorComando { get; set; } = new List<ConfianzaComando>();
    }
}
