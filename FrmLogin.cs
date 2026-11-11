using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    public partial class frmLogin : Form
    {
        private readonly clsAutenticacionNegocio autenticacion = new clsAutenticacionNegocio();

        private bool moviendo;
        private Point puntoInicial;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            mtdCargarOperadores();
        }

        private void mtdCargarOperadores()
        {
            try
            {
                List<Usuario> operadores = autenticacion.mtdListarOperadores();

                cbxOperador.DataSource = operadores;
                cbxOperador.DisplayMember = "Etiqueta";
                cbxOperador.ValueMember = "IdUsuario";
                cbxOperador.SelectedIndex = -1;

                if (operadores.Count == 0)
                    MessageBox.Show("No hay operadores activos con credencial PIN en la base de datos.",
                        "ORICA SafeHands", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar la lista de operadores.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txtPIN.Clear();
        }

        

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (cbxOperador.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un operador.", "Datos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbxOperador.Focus();
                return;
            }

            Usuario operador = (Usuario)cbxOperador.SelectedItem;

            Cursor = Cursors.WaitCursor;
            Resultado resultado = autenticacion.mtdIniciarSesionConPin(operador, txtPIN.Text);
            Cursor = Cursors.Default;

            if (!resultado.Exito)
            {
                MessageBox.Show(resultado.Mensaje, "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPIN.Clear();
                txtPIN.Focus();
                return;
            }

            mtdMostrarIngreso(resultado.Mensaje);
        }

        private void mtdMostrarIngreso(string mensaje)
        {
            MessageBox.Show(mensaje + "\nCargo: " + Sesion.UsuarioActual.RolNombre,
                "Inicio de sesión exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtPIN.Clear();

            Form principal = null;
            try
            {
                if (Sesion.UsuarioActual.RolCodigo == "OPE")
                    principal = new FrmPrincipalOperador();
                else
                    principal = new frmPrincipal();

                Hide();
                principal.Show();
                principal.Activate();   // la trae al frente
                principal.FormClosed += principal_FormClosed;
            }
            catch (Exception ex)
            {
                
                if (principal != null) principal.Dispose();
                autenticacion.mtdCerrarSesion();
                Show();
                MessageBox.Show("No se pudo abrir la pantalla principal.\n\n" + ex.ToString(),
                    "Error al ingresar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void principal_FormClosed(object sender, FormClosedEventArgs e)
        {
            autenticacion.mtdCerrarSesion();

            bool volverAlLogin = false;
            if (sender is FrmPrincipalOperador)
                volverAlLogin = ((FrmPrincipalOperador)sender).CerrarSesionSolicitada;
            if (sender is frmPrincipal)
                volverAlLogin = ((frmPrincipal)sender).CerrarSesionSolicitada;

            if (volverAlLogin)
            {
                mtdCargarOperadores();
                Show();
            }
            else
            {
                Close();
            }
        }

        

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnMinimizar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                moviendo = true;
                puntoInicial = e.Location;
            }
        }

        private void btnMinimizar_MouseMove(object sender, MouseEventArgs e)
        {
            if (moviendo)
                Location = new Point(Left + e.X - puntoInicial.X, Top + e.Y - puntoInicial.Y);
        }

        private void btnMinimizar_MouseUp(object sender, MouseEventArgs e)
        {
            moviendo = false;
        }
    }
}
