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
        private bool moviendo;
        private Point puntoInicial;
        private readonly clsAutenticacionNegocio autenticacion = new clsAutenticacionNegocio();

        public frmLogin()
        {
            InitializeComponent();
            cbxOperador.DropDownStyle = ComboBoxStyle.DropDownList;
            txtPIN.MaxLength = 4;
            this.AcceptButton = btnIngresar;
            this.Load += frmLogin_Load;
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
                {
                    MessageBox.Show("No hay operadores activos con credencial PIN en la base de datos.",
                        "ORICA SafeHands", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar la lista de operadores.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txtPIN.Clear();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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
            {
                this.Location = new Point(
                    this.Left + e.X - puntoInicial.X,
                    this.Top + e.Y - puntoInicial.Y);
            }
        }

        private void btnMinimizar_MouseUp(object sender, MouseEventArgs e)
        {
            moviendo = false;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (!mtdValidarIngreso())
                return;

            Usuario operador = (Usuario)cbxOperador.SelectedItem;

            Cursor = Cursors.WaitCursor;
            Resultado resultado;
            try
            {
                resultado = autenticacion.mtdIniciarSesionConPin(operador, txtPIN.Text);
            }
            finally
            {
                Cursor = Cursors.Default;
            }

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

        private bool mtdValidarIngreso()
        {
            if (cbxOperador.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un operador.", "Datos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                cbxOperador.Focus();
                return false;
            }

            string pin = txtPIN.Text;

            if (string.IsNullOrWhiteSpace(pin))
            {
                MessageBox.Show("Ingrese su PIN.", "Datos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                txtPIN.Focus();
                return false;
            }

            if (pin.Length != 4)
            {
                MessageBox.Show("El PIN debe tener 4 números.", "PIN inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                txtPIN.Focus();
                return false;
            }

            foreach (char caracter in pin)
            {
                if (caracter < '0' || caracter > '9')
                {
                    MessageBox.Show("El PIN solo debe contener números.",
                        "PIN inválido", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtPIN.SelectAll();
                    txtPIN.Focus();
                    return false;
                }
            }

            return true;
        }

        private void mtdMostrarIngreso(string mensaje)
        {
            MessageBox.Show(mensaje + "\nCargo: " + Sesion.UsuarioActual.RolNombre,
                "Inicio de sesión exitoso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtPIN.Clear();

            Form principal;
            if (Sesion.UsuarioActual.RolCodigo == "OPE")
                principal = new FrmPrincipalOperador();
            else
                principal = new frmPrincipal();

            principal.FormClosed += principal_FormClosed;
            this.Hide();
            principal.Show();
        }

        private void principal_FormClosed(object sender, FormClosedEventArgs e)
        {
            autenticacion.mtdCerrarSesion();

            FrmPrincipalOperador operador = sender as FrmPrincipalOperador;
            if (operador != null && operador.CerrarSesionSolicitada)
            {
                mtdCargarOperadores();
                this.Show();
            }
            else
            {
                this.Close();
            }
        }

        private void btnRFID_Click(object sender, EventArgs e)
        {
            try
            {
                string rutaConfig =
                    AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

                MessageBox.Show(
                    rutaConfig,
                    "Archivo de configuración utilizado"
                );

                Resultado resultado = clsSistemaNegocio.mtdProbarConexion();
                MessageBox.Show(resultado.Mensaje, "ORICA SafeHands", MessageBoxButtons.OK,
                    resultado.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

        }
    }
}
