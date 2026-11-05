using ORICA.Negocio;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    // Ventana principal del operador: menú a la izquierda y pantallas a la derecha (pnlPantallasOperador).
    // Los clics de los botones se conectan en el diseñador (FrmPrincipalOperador.Designer.cs).
    public partial class FrmPrincipalOperador : Form
    {
        private Form pantallaActual;  // pantalla que se está mostrando ahora

        // True cuando el operador pulsó "Cerrar sesión" (volver al login en vez de salir).
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

            // Colores e íconos del menú (se dibujan por código).
            EstiloMenuOperador.Aplicar(pnlMenuOperador, pnlSesionOperador, btnInicioOperador,
                btnControlVozOperador, btnHistorialOperador, btnCerrarSesionOperador, btnInicioOperador);
        }

        // ---------------- Botones del menú ----------------

        private void btnInicioOperador_Click(object sender, EventArgs e)
        {
            clsPantallas.mtdCerrar(pantallaActual);
            pantallaActual = null;
            pnlPantallasOperador.Visible = false;
            MarcarMenu(btnInicioOperador);
        }

        private void btnControlVozOperador_Click(object sender, EventArgs e)
        {
            MostrarPantalla(new FrmOperacionVoz(), btnControlVozOperador);
        }

        private void btnHistorialOperador_Click(object sender, EventArgs e)
        {
            MostrarPantalla(new FrmMiHistorial(), btnHistorialOperador);
        }

        // Botón "Ir a control por voz" del inicio.
        private void btnIrControlVoz_Click(object sender, EventArgs e)
        {
            MostrarPantalla(new FrmOperacionVoz(), btnControlVozOperador);
        }

        private void btnCerrarSesionOperador_Click(object sender, EventArgs e)
        {
            CerrarSesionSolicitada = true;
            Close();
        }

        private void btnVerComandosInicio_Click(object sender, EventArgs e)
        {
            new FrmComandosDisponibles().ShowDialog();
        }

        // ---------------- Pantallas ----------------

        private void MostrarPantalla(Form pantalla, Button seleccionado)
        {
            clsPantallas.mtdCerrar(pantallaActual);
            pantallaActual = pantalla;
            clsPantallas.mtdMostrarEnPanel(pnlPantallasOperador, pantalla, "pnlMenuOperador");
            MarcarMenu(seleccionado);
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
    }
}
