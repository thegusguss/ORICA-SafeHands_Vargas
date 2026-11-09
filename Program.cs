using ORICA.Negocio;
using System;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    internal static class Program
    {
        /// <summary>Punto de entrada: la aplicación empieza en el login.</summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Cualquier error no controlado se muestra en un mensaje en vez de cerrar la app en silencio.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) =>
                MessageBox.Show(e.Exception.ToString(), "Error inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);

            clsSincronizacionNegocio.mtdIniciar();   // conecta con Azure y, si no hay internet, trabaja con la copia local
            clsMonitorArduino.Iniciar();   // vigila el puerto COM del Arduino y genera la alerta si se desconecta
            Application.Run(new frmLogin());
            clsMonitorArduino.Detener();
            clsSincronizacionNegocio.mtdDetener();
        }
    }
}
