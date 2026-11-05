using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    public partial class FrmOperacionVoz : Form
    {
        public FrmOperacionVoz()
        {
            InitializeComponent();

            // Recorta el botón de "pulsar para hablar" para que se vea redondo.
            using (GraphicsPath circulo = new GraphicsPath())
            {
                circulo.AddEllipse(0, 0, btnPTTVoz.Width, btnPTTVoz.Height);
                btnPTTVoz.Region = new Region(circulo);
            }
        }

        private void lnkComandosDisponibles_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new FrmComandosDisponibles().Show();
        }
    }
}
