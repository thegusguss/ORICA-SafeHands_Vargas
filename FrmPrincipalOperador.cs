using ORICA.Negocio;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    /// <summary>Ventana principal del operador: menú a la izquierda y pantallas a la derecha.</summary>
    public partial class FrmPrincipalOperador : Form
    {
        private Panel panelPantallas;  // panel donde se muestran las pantallas
        private Form pantallaActual;   // pantalla que se está mostrando ahora

        /// <summary>True cuando el operador pulsó "Cerrar sesión" (volver al login en vez de salir).</summary>
        public bool CerrarSesionSolicitada { get; private set; }

        public FrmPrincipalOperador()
        {
            InitializeComponent();

            // Datos del operador que inició sesión.
            if (Sesion.Activa)
            {
                lblNombreOperador.Text = Sesion.UsuarioActual.NombreCompleto;
                lblRolOperador.Text = Sesion.UsuarioActual.RolNombre;
            }

            EstiloMenuOperador.Aplicar(pnlMenuOperador, pnlSesionOperador, btnInicioOperador,
                btnControlVozOperador, btnHistorialOperador, btnCerrarSesionOperador, btnInicioOperador);

            // El panel ocupa todo lo que queda a la derecha del menú.
            panelPantallas = new Panel();
            panelPantallas.Name = "pnlPantallasOperador";
            panelPantallas.BackColor = Color.FromArgb(14, 19, 26);
            panelPantallas.Visible = false;
            Controls.Add(panelPantallas);
            AjustarPanelPantallas();
            ClientSizeChanged += (s, e) => AjustarPanelPantallas();

            // Botones del menú.
            btnInicioOperador.Click += (s, e) => MostrarInicio();
            btnControlVozOperador.Click += (s, e) => MostrarPantalla(new FrmOperacionVoz(), btnControlVozOperador);
            btnHistorialOperador.Click += (s, e) => MostrarPantalla(new FrmMiHistorial(), btnHistorialOperador);
            btnIrControlVoz.Click += (s, e) => MostrarPantalla(new FrmOperacionVoz(), btnControlVozOperador);
            btnCerrarSesionOperador.Click += (s, e) =>
            {
                CerrarSesionSolicitada = true;
                Close();
            };
        }

        private void AjustarPanelPantallas()
        {
            int inicio = pnlMenuOperador.Right;
            panelPantallas.SetBounds(inicio, 0, Math.Max(1, ClientSize.Width - inicio), ClientSize.Height);
        }

        // Pinta de morado el botón elegido y deja los demás con el fondo normal.
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
            clsPantallas.mtdCerrar(pantallaActual);
            pantallaActual = null;
            panelPantallas.Visible = false;
            MarcarMenu(btnInicioOperador);
        }

        private void MostrarPantalla(Form pantalla, Button seleccionado)
        {
            clsPantallas.mtdCerrar(pantallaActual);
            pantallaActual = pantalla;
            clsPantallas.mtdMostrarEnPanel(panelPantallas, pantalla, "pnlMenuOperador");
            MarcarMenu(seleccionado);
        }

        private void btnVerComandosInicio_Click(object sender, EventArgs e)
        {
            new FrmComandosDisponibles().ShowDialog();
        }
    }
}
