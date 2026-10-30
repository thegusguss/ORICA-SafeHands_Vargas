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
    public partial class FrmPrincipalOperador : Form
    {
        private Panel panelPantallas;
        private Form pantallaActual;

        public FrmPrincipalOperador()
        {
            InitializeComponent();


            EstiloMenuOperador.Aplicar(
                pnlMenuOperador,
                pnlSesionOperador,
                btnInicioOperador,
                btnControlVozOperador,
                btnHistorialOperador,
                btnCerrarSesionOperador,
                btnInicioOperador);

            panelPantallas = new Panel();
            panelPantallas.Name = "pnlPantallasOperador";
            panelPantallas.BackColor = Color.FromArgb(14, 19, 26);
            panelPantallas.Visible = false;

            Controls.Add(panelPantallas);

            AjustarPanelPantallas();

            ClientSizeChanged += (s, e) => AjustarPanelPantallas();

            btnInicioOperador.Click += (s, e) => MostrarInicio();

            btnControlVozOperador.Click += (s, e) =>
                MostrarPantalla(
                    new FrmOperacionVoz(),
                    btnControlVozOperador);

            btnHistorialOperador.Click += (s, e) =>
                MostrarPantalla(
                    new FrmMiHistorial(),
                    btnHistorialOperador);

            btnIrControlVoz.Click += (s, e) =>
                MostrarPantalla(
                    new FrmOperacionVoz(),
                    btnControlVozOperador);
        }

        private void AjustarPanelPantallas()
        {
            int inicioContenido = pnlMenuOperador.Right;

            panelPantallas.SetBounds(
                inicioContenido,
                0,
                Math.Max(1, ClientSize.Width - inicioContenido),
                ClientSize.Height);
        }

        private void MarcarMenu(Button seleccionado)
        {
            Color fondo = Color.FromArgb(17, 24, 34);

            btnInicioOperador.BackColor = fondo;
            btnControlVozOperador.BackColor = fondo;
            btnHistorialOperador.BackColor = fondo;

            seleccionado.BackColor = Color.FromArgb(50, 43, 96);
        }

        private void MostrarInicio()
        {
            CerrarPantallaActual();

            panelPantallas.Visible = false;

            MarcarMenu(btnInicioOperador);
        }

        private void MostrarPantalla(Form pantalla, Button seleccionado)
        {
            CerrarPantallaActual();

            PrepararContenido(pantalla);

            pantalla.TopLevel = false;
            pantalla.FormBorderStyle = FormBorderStyle.None;
            pantalla.Dock = DockStyle.Fill;

            pantallaActual = pantalla;

            panelPantallas.Controls.Add(pantalla);
            panelPantallas.Visible = true;
            panelPantallas.BringToFront();

            pantalla.Show();

            MarcarMenu(seleccionado);
        }

        private void CerrarPantallaActual()
        {
            if (pantallaActual == null)
            {
                return;
            }

            panelPantallas.Controls.Remove(pantallaActual);
            pantallaActual.Close();
            pantallaActual.Dispose();
            pantallaActual = null;
        }

        private void PrepararContenido(Form pantalla)
        {
            Control[] encontrados =
                pantalla.Controls.Find("pnlMenuOperador", true);

            if (encontrados.Length == 0)
            {
                return;
            }

            Control menuRepetido = encontrados[0];

            if (menuRepetido.Parent != pantalla)
            {
                return;
            }

            int espacioMenu = menuRepetido.Right;

            var posiciones = new Dictionary<Control, Rectangle>();

            foreach (Control control in pantalla.Controls)
            {
                if (control != menuRepetido)
                {
                    posiciones.Add(control, control.Bounds);
                }
            }

            pantalla.SuspendLayout();

            pantalla.Controls.Remove(menuRepetido);
            menuRepetido.Dispose();

            pantalla.ClientSize = new Size(
                Math.Max(1, pantalla.ClientSize.Width - espacioMenu),
                pantalla.ClientSize.Height);

            foreach (var elemento in posiciones)
            {
                Control control = elemento.Key;
                Rectangle posicion = elemento.Value;

                if (control.Dock == DockStyle.None)
                {
                    control.Bounds = new Rectangle(
                        Math.Max(0, posicion.X - espacioMenu),
                        posicion.Y,
                        posicion.Width,
                        posicion.Height);
                }
            }

            pantalla.ResumeLayout(true);
        }


        private void btnVerComandosInicio_Click(object sender, EventArgs e)
        {
            FrmComandosDisponibles frmComandos = new FrmComandosDisponibles();
            frmComandos.ShowDialog();
        }

        private void btnIrControlVoz_Click(object sender, EventArgs e)
        {
            btnControlVozOperador.PerformClick();
        }

        private void btnControlVozOperador_Click(object sender, EventArgs e)
        {
            MostrarPantalla(new FrmMiHistorial(), btnHistorialOperador);
        }
    }
}
