using ORICA.Negocio;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    /// <summary>
    /// Acceso corto al enlace con el Arduino para las pantallas (cabeceras "● Arduino en COM3").
    /// Todo el trabajo (puerto serie, heartbeat, alertas) lo hace clsEnlaceArduino en ORICA.Negocio.
    /// </summary>
    public static class clsMonitorArduino
    {
        public static string Puerto { get { return clsEnlaceArduino.Puerto; } }

        /// <summary>true si el Arduino responde (puerto abierto y heartbeat al día).</summary>
        public static bool Conectado { get { return clsEnlaceArduino.Conectado; } }

        public static string TextoEstado { get { return clsEnlaceArduino.TextoEstado; } }

        /// <summary>Se llama una vez al iniciar la aplicación (Program.Main).</summary>
        public static void Iniciar() { clsEnlaceArduino.mtdIniciar(); }

        public static void Detener() { clsEnlaceArduino.mtdDetener(); }
    }
}
