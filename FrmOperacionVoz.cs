using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    public partial class FrmOperacionVoz : Form
    {
        public FrmOperacionVoz()
        {
            InitializeComponent();

            using (var forma = new System.Drawing.Drawing2D.GraphicsPath())
            {
                forma.AddEllipse(0, 0,
                    btnPTTVoz.Width,
                    btnPTTVoz.Height
                );

                btnPTTVoz.Region = new System.Drawing.Region(forma);
            }
        }

        private void lnkComandosDisponibles_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmComandosDisponibles frmComandos = new FrmComandosDisponibles();
            frmComandos.Show();
        }
    }
}
