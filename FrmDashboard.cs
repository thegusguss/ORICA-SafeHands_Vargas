using System;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    /// <summary>Ventana principal del administrador: menú a la izquierda y pantallas a la derecha.</summary>
    public partial class frmPrincipal : Form
    {
        private Form formularioActivo;  // pantalla que se está mostrando ahora
        private Panel pnlVista;          // panel donde se muestran las pantallas

        public frmPrincipal()
        {
            InitializeComponent();

            pnlVista = new Panel();
            pnlVista.Dock = DockStyle.Fill;
            pnlVista.BackColor = Color.FromArgb(14, 19, 26);
            pnlVista.Visible = false;
            pnlContenido.Controls.Add(pnlVista);

            ColocarIcono(btnDashboard, "⌂");
            btnDashboard.Click += (s, e) => MostrarDashboard();

            // Cada botón del menú: su ícono y la pantalla que abre.
            ConfigurarBoton(btnDiagnostico, "⚒", () => new FrmDiagnostico());
            ConfigurarBoton(btnAlertas, "⚠", () => new FrmAlertas());
            ConfigurarBoton(btnCriticidad, "◆", () => new FrmCriticidad());
            ConfigurarBoton(btnEventos, "☷", () => new FrmEventos());
            ConfigurarBoton(btnOperadores, "♟", () => new FrmOperadores());
            ConfigurarBoton(btnCredenciales, "▣", () => new Credenciales());
            ConfigurarBoton(btnEquipos, "▤", () => new FrmEquipos());
            ConfigurarBoton(btnComandos, "▶", () => new FrmComandos());
            ConfigurarBoton(btnSincronizacion, "↻", () => new FrmSincronizacion());
            ConfigurarBoton(btnReportes, "▥", () => new FrmReportes());
            ConfigurarBoton(btnConfiguracion, "⚙", () => new FrmConfiguracion());
        }

        // crearPantalla se ejecuta en cada clic, así siempre se abre una pantalla nueva.
        private void ConfigurarBoton(Button boton, string icono, Func<Form> crearPantalla)
        {
            ColocarIcono(boton, icono);
            boton.Click += (s, e) => AbrirFormulario(crearPantalla());
        }

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
