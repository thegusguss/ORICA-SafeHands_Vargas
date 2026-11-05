using System;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    public partial class FrmConfiguracion : Form
    {
        public FrmConfiguracion()
        {
            InitializeComponent();
            MostrarUmbral();
        }

        private void trkUmbralConfianza_ValueChanged(object sender, EventArgs e)
        {
            MostrarUmbral();
        }

        // Muestra en la etiqueta el valor actual de la barra de umbral.
        private void MostrarUmbral()
        {
            lblUmbralConfianza.Text = "Umbral de confianza: " + trkUmbralConfianza.Value + "%";
        }
    }
}
