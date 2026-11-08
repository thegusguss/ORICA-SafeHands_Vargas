using System;
using System.Collections.Generic;

namespace ORICA.Negocio.Modelos
{
    /// <summary>Resultado de comparar lo que se escuchó con la gramática cerrada del equipo.</summary>
    public class InterpretacionVoz
    {
        public bool Aceptado { get; set; }
        public Comando Comando { get; set; }            // null si la frase no es un comando del equipo
        public string TextoReconocido { get; set; }      // tal como lo devolvió el reconocedor
        public decimal Confianza { get; set; }           // 0 a 100
        public decimal ConfianzaRequerida { get; set; }  // la mayor entre la del comando y el umbral global
        public string Motivo { get; set; }               // por qué se rechazó (si Aceptado = false)
    }

    /// <summary>Respuesta a un paso de confirmación (se guarda en dbo.Confirmacion).</summary>
    public class RespuestaConfirmacion
    {
        public int NumeroPaso { get; set; }        // 1 o 2
        public string Respuesta { get; set; }      // CONFIRMAR, CANCELAR o TIMEOUT
        public int TiempoRespuestaMs { get; set; }
    }

    /// <summary>Comando reconocido que espera confirmación del operador.</summary>
    public class OperacionPendiente
    {
        public Comando Comando { get; set; }
        public Equipo Equipo { get; set; }
        public string TextoReconocido { get; set; }
        public decimal Confianza { get; set; }
        public int TimeoutSegundos { get; set; }
        public int PasoActual { get; set; } = 1;
        public DateTime InicioPaso { get; set; } = DateTime.Now;
        public List<RespuestaConfirmacion> Respuestas { get; } = new List<RespuestaConfirmacion>();

        public int TotalPasos
        {
            get { return Comando == null ? 0 : Comando.NumeroConfirmaciones; }
        }

        public bool RequiereConfirmacion
        {
            get { return TotalPasos > 0; }
        }

        /// <summary>Segundos que le quedan al operador para responder el paso actual.</summary>
        public int SegundosRestantes
        {
            get { return Math.Max(0, TimeoutSegundos - (int)(DateTime.Now - InicioPaso).TotalSeconds); }
        }

        public bool Vencida
        {
            get { return RequiereConfirmacion && (DateTime.Now - InicioPaso).TotalSeconds >= TimeoutSegundos; }
        }
    }

    /// <summary>Qué pasa después de responder un paso de confirmación.</summary>
    public enum EstadoConfirmacion
    {
        EsperandoSiguientePaso,
        ListaParaEjecutar,
        Cancelada
    }
}
