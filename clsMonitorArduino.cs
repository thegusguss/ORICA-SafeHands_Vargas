using ORICA.Negocio;
using System;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    /// <summary>
    /// Vigila mientras la app está abierta si el puerto COM del Arduino (Configuración → PUERTO_COM) sigue existiendo.
    /// Si el puerto estaba y desaparece (se desconectó el cable USB), genera la alerta "Arduino desconectado".
    /// Cuando exista el enlace serial completo, ese módulo podrá avisar también fallas de heartbeat.
    /// </summary>
    public static class clsMonitorArduino
    {
        private const int IntervaloMs = 3000;          // cada cuánto se revisa el puerto
        private const int RecargarPuertoCada = 10;     // cada 10 revisiones (~30 s) se vuelve a leer la configuración

        private static Timer temporizador;
        private static int revisiones;
        private static bool estabaConectado;

        /// <summary>Puerto configurado, por ejemplo "COM3".</summary>
        public static string Puerto { get; private set; }

        /// <summary>true si el puerto configurado existe ahora en Windows.</summary>
        public static bool Conectado { get; private set; }

        /// <summary>Texto corto para las cabeceras: "● Arduino en COM3" / "● Arduino desconectado (COM3)".</summary>
        public static string TextoEstado
        {
            get
            {
                if (string.IsNullOrEmpty(Puerto)) return "● Arduino sin configurar";
                return Conectado ? "● Arduino en " + Puerto : "● Arduino desconectado (" + Puerto + ")";
            }
        }

        /// <summary>Se llama una vez al iniciar la aplicación (Program.Main).</summary>
        public static void Iniciar()
        {
            if (temporizador != null) return;

            LeerPuerto();
            Conectado = PuertoExiste();
            estabaConectado = Conectado;

            temporizador = new Timer { Interval = IntervaloMs };
            temporizador.Tick += (s, e) => Revisar();
            temporizador.Start();
        }

        public static void Detener()
        {
            if (temporizador == null) return;
            temporizador.Stop();
            temporizador.Dispose();
            temporizador = null;
        }

        private static void Revisar()
        {
            revisiones++;
            if (revisiones % RecargarPuertoCada == 0)
            {
                string anterior = Puerto;
                LeerPuerto();
                if (!string.Equals(anterior, Puerto, StringComparison.OrdinalIgnoreCase))
                    estabaConectado = PuertoExiste();   // cambiaron el puerto en Configuración: se empieza de cero
            }

            Conectado = PuertoExiste();

            // Solo se alerta en el paso de "conectado" a "desconectado" (no si nunca se conectó).
            if (estabaConectado && !Conectado)
                new clsAlertaNegocio().mtdAlertaEnlace(Puerto, "el puerto desapareció (cable USB desconectado o Arduino apagado)");

            estabaConectado = Conectado;
        }

        private static void LeerPuerto()
        {
            try
            {
                Puerto = new clsConfiguracionNegocio().mtdObtenerTexto(clsConfiguracionNegocio.PuertoCom);
            }
            catch (Exception)
            {
                // Sin BD se mantiene el puerto que ya se conocía.
            }
        }

        private static bool PuertoExiste()
        {
            if (string.IsNullOrEmpty(Puerto)) return false;
            try
            {
                return SerialPort.GetPortNames().Any(p => string.Equals(p, Puerto, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
