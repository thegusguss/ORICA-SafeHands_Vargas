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
    public partial class frmPrincipal : Form
    {
        private Form formularioActivo;
        private Panel pnlVista;
        public frmPrincipal()
        {
            InitializeComponent();
            ColocarIcono(btnDashboard, "⌂");
            ColocarIcono(btnDiagnostico, "⚒");
            ColocarIcono(btnAlertas, "⚠");
            ColocarIcono(btnCriticidad, "◆");
            ColocarIcono(btnEventos, "☷");
            ColocarIcono(btnOperadores, "♟");
            ColocarIcono(btnCredenciales, "▣");
            ColocarIcono(btnEquipos, "▤");
            ColocarIcono(btnComandos, "▶");
            ColocarIcono(btnSincronizacion, "↻");
            ColocarIcono(btnReportes, "▥");
            ColocarIcono(btnConfiguracion, "⚙");

            pnlVista = new Panel();
            pnlVista.Dock = DockStyle.Fill;
            pnlVista.BackColor = Color.FromArgb(14, 19, 26);
            pnlVista.Visible = false;

            pnlContenido.Controls.Add(pnlVista);
            btnDashboard.Click += (s, e) => MostrarDashboard();

            btnDiagnostico.Click += (s, e) =>
                AbrirFormulario(new FrmDiagnostico());

            btnAlertas.Click += (s, e) =>
                AbrirFormulario(new FrmAlertas());

            btnCriticidad.Click += (s, e) =>
                AbrirFormulario(new FrmCriticidad());

            btnEventos.Click += (s, e) =>
                AbrirFormulario(new FrmEventos());

            btnOperadores.Click += (s, e) =>
                AbrirFormulario(new FrmOperadores());

            btnCredenciales.Click += (s, e) =>
                AbrirFormulario(new Credenciales());

            btnEquipos.Click += (s, e) =>
                AbrirFormulario(new FrmEquipos());

            btnComandos.Click += (s, e) =>
                AbrirFormulario(new FrmComandos());

            btnSincronizacion.Click += (s, e) =>
                AbrirFormulario(new FrmSincronizacion());

            btnReportes.Click += (s, e) =>
                AbrirFormulario(new FrmReportes());

            btnConfiguracion.Click += (s, e) =>
                AbrirFormulario(new FrmConfiguracion());
        }

        private void AbrirFormulario(Form formulario)
        {
            if (formularioActivo != null)
            {
                formularioActivo.Close();
                formularioActivo.Dispose();
            }

            Control[] menus = formulario.Controls.Find("flpMenu", true);

            if (menus.Length > 0)
            {
                Control menu = menus[0];

                if (menu.Parent == formulario)
                {
                    int espacioMenu = menu.Right;

                    Dictionary<Control, Rectangle> posiciones =
                  new Dictionary<Control, Rectangle>();

                    foreach (Control control in formulario.Controls)
                    {
                        if (control != menu)
                        {
                            posiciones.Add(control, control.Bounds);
                        }
                    }

                    formulario.SuspendLayout();

                    formulario.Controls.Remove(menu);
                    menu.Dispose();

                    formulario.ClientSize = new Size(
                        Math.Max(1, formulario.ClientSize.Width - espacioMenu),
                        formulario.ClientSize.Height);

                    foreach (KeyValuePair<Control, Rectangle> elemento in posiciones)
                    {
                        Rectangle posicion = elemento.Value;

                        elemento.Key.Bounds = new Rectangle(
                            Math.Max(0, posicion.X - espacioMenu),
                            posicion.Y,
                            posicion.Width,
                            posicion.Height);
                    }

                    formulario.ResumeLayout(true);
                }
            }

            formularioActivo = formulario;

            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;

            pnlVista.Controls.Add(formulario);

            pnlVista.Visible = true;
            pnlVista.BringToFront();

            formulario.Show();
            formulario.BringToFront();
        }

        private void MostrarDashboard()
        {
            if (formularioActivo != null)
            {
                formularioActivo.Close();
                formularioActivo.Dispose();
                formularioActivo = null;
            }

            pnlVista.Visible = false;
        }

        private void ColocarIcono(Button boton, string simbolo)
        {
            Bitmap imagen = new Bitmap(28, 35);

            using (Graphics dibujo = Graphics.FromImage(imagen))
            using (Font fuente = new Font("Segoe UI Symbol", 16))
            {
                dibujo.Clear(Color.Transparent);

                TextRenderer.DrawText(
                    dibujo,
                    simbolo,
                    fuente,
                    new Rectangle(0, 0, 28, 28),
                    Color.White,
                    TextFormatFlags.HorizontalCenter |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.NoPadding);
            }

            boton.Image = imagen;
            boton.ImageAlign = ContentAlignment.MiddleLeft;
            boton.TextAlign = ContentAlignment.MiddleLeft;
            boton.TextImageRelation = TextImageRelation.ImageBeforeText;
            boton.Padding = new Padding(10, 0, 0, 0);
        }
    }
}
