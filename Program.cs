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
            clsMonitorArduino.Iniciar();   // vigila el puerto COM del Arduino y genera la alerta si se desconecta
            Application.Run(new frmLogin());
            clsMonitorArduino.Detener();
        }
    }
}
