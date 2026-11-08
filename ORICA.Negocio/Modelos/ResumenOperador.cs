using System;

namespace ORICA.Negocio.Modelos
{
    // Datos de la pantalla de inicio de un operador.
    public class ResumenOperador
    {
        // Sesión actual
        public int AccionesSesion { get; set; }
        public int EjecutadasSesion { get; set; }
        public int ConfirmadasSesion { get; set; }
        public int RechazadasSesion { get; set; }
        public int CanceladasSesion { get; set; }

        // Histórico
        public int EjecutadasTotal { get; set; }
        public DateTime? SesionAnterior { get; set; }

        // Lo que puede usar
        public int EquiposDisponibles { get; set; }
        public int ComandosDisponibles { get; set; }
        public string CodigosEquipos { get; set; }   // "EQ-001, EQ-002"

        // Última acción (de cualquier sesión)
        public DateTime? UltimaAccionFecha { get; set; }
        public string UltimaAccionTexto { get; set; }      // "\"ESTADO DEL EQUIPO\" en EQ-001"
        public string UltimaAccionResultado { get; set; }  // EJECUTADO, RECHAZADO...
    }
}
