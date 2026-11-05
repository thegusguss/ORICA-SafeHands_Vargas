using System;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    // Ventana principal del administrador: menú a la izquierda y pantallas a la derecha.
    // Los clics de los botones se conectan en el diseñador (FrmDashboard.Designer.cs).
    public partial class frmPrincipal : Form
    {
        private Form formularioActivo;  // pantalla que se está mostrando ahora en pnlVista

        public frmPrincipal()
        {
            InitializeComponent();

            // Los íconos se dibujan por código porque son imágenes hechas con una fuente.
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
        }

        // ---------------- Botones del menú ----------------

        private void btnDashboard_Click(object sender, EventArgs e) { MostrarDashboard(); }
        private void btnDiagnostico_Click(object sender, EventArgs e) { AbrirFormulario(new FrmDiagnostico()); }
        private void btnAlertas_Click(object sender, EventArgs e) { AbrirFormulario(new FrmAlertas()); }
        private void btnCriticidad_Click(object sender, EventArgs e) { AbrirFormulario(new FrmCriticidad()); }
        private void btnEventos_Click(object sender, EventArgs e) { AbrirFormulario(new FrmEventos()); }
        private void btnOperadores_Click(object sender, EventArgs e) { AbrirFormulario(new FrmOperadores()); }
        private void btnCredenciales_Click(object sender, EventArgs e) { AbrirFormulario(new Credenciales()); }
        private void btnEquipos_Click(object sender, EventArgs e) { AbrirFormulario(new FrmEquipos()); }
        private void btnComandos_Click(object sender, EventArgs e) { AbrirFormulario(new FrmComandos()); }
        private void btnSincronizacion_Click(object sender, EventArgs e) { AbrirFormulario(new FrmSincronizacion()); }
        private void btnReportes_Click(object sender, EventArgs e) { AbrirFormulario(new FrmReportes()); }
        private void btnConfiguracion_Click(object sender, EventArgs e) { AbrirFormulario(new FrmConfiguracion()); }

        // ---------------- Pantallas ----------------

        private void AbrirFormulario(Form formulario)
        {
            clsPantallas.mtdCerrar(formularioActivo);
            formularioActivo = formulario;
            clsPantallas.mtdMostrarEnPanel(pnlVista, formulario, "flpMenu");
        }

        // Vuelve al dashboard: cierra la pantalla abierta y oculta el panel.
        private void MostrarDashboard()
        {
            clsPantallas.mtdCerrar(formularioActivo);
            formularioActivo = null;
            pnlVista.Visible = false;
        }

        // Dibuja el símbolo como imagen a la izquierda del texto del botón.
        // Ojo: usa su propio tamaño (28x35) y fuente; no es igual al ícono del menú del operador.
        private void ColocarIcono(Button boton, string simbolo)
        {
            Bitmap imagen = new Bitmap(28, 35);

            using (Graphics dibujo = Graphics.FromImage(imagen))
            using (Font fuente = new Font("Segoe UI Symbol", 16))
            {
                dibujo.Clear(Color.Transparent);

                TextRenderer.DrawText(dibujo, simbolo, fuente, new Rectangle(0, 0, 28, 28), Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }

            boton.Image = imagen;
            boton.ImageAlign = ContentAlignment.MiddleLeft;
            boton.TextAlign = ContentAlignment.MiddleLeft;
            boton.TextImageRelation = TextImageRelation.ImageBeforeText;
            boton.Padding = new Padding(10, 0, 0, 0);
        }
    }
}
