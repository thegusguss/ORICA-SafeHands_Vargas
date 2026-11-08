using System;

namespace ORICA.Negocio.Modelos
{
    // Números del panel principal del administrador.
    public class ResumenDashboard
    {
        public int Operadores { get; set; }
        public int Credenciales { get; set; }
        public int Comandos { get; set; }
        public int Equipos { get; set; }

        public int IntentosHoy { get; set; }
        public int EjecutadosHoy { get; set; }
        public int RechazadosHoy { get; set; }
        public decimal? ConfianzaHoy { get; set; }
        public decimal? Confianza7Dias { get; set; }

        public int AlertasPendientes { get; set; }
        public int AlertasActivas { get; set; }
        public int SesionesAbiertas { get; set; }

        // Estado simulado del prototipo (deducido de los últimos comandos ejecutados)
        public bool MotorEnMarcha { get; set; }
        public DateTime? MotorDesde { get; set; }
        public bool LedEncendido { get; set; }
        public DateTime? LedDesde { get; set; }
        public bool BuzzerActivo { get; set; }
        public DateTime? BuzzerDesde { get; set; }
        public bool ParadaActiva { get; set; }
        public DateTime? UltimaParada { get; set; }

        /// <summary>Porcentaje de comandos de hoy que se ejecutaron (null si hoy no hubo comandos).</summary>
        public int? PorcentajeExito
        {
            get { return IntentosHoy == 0 ? (int?)null : (int)Math.Round(EjecutadosHoy * 100.0 / IntentosHoy); }
        }
    }
}
