using ORICA.Negocio;
using ORICA.Negocio.Modelos;
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
    public partial class frmLogin : Form
    {
        private bool moviendo = false;
        private Point puntoInicial;
        private readonly AutenticacionNegocio _autenticacion = new AutenticacionNegocio();

        public frmLogin()
        {
            InitializeComponent();
            cbxOperador.DropDownStyle = ComboBoxStyle.DropDownList;
            txtPIN.MaxLength = 4;
            this.AcceptButton = btnIngresar;
            this.Load += (s, e) => CargarOperadores();
        }

        private void CargarOperadores()
        {
            try
            {
                List<Usuario> operadores = _autenticacion.ListarOperadores();

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
            if (cbxOperador.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un operador.", "Datos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                cbxOperador.Focus();
                return;
            }

            string pin = txtPIN.Text;

            if (string.IsNullOrWhiteSpace(pin))
            {
                MessageBox.Show("Ingrese su PIN.", "Datos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                txtPIN.Focus();
                return;
            }

            if (pin.Length != 4)
            {
                MessageBox.Show("El PIN debe tener 4 números.", "PIN inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                txtPIN.Focus();
                return;
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
                    return;
                }
            }

            Usuario operador = (Usuario)cbxOperador.SelectedItem;

            Cursor = Cursors.WaitCursor;
            Resultado r = _autenticacion.IngresarConPin(operador, pin);
            Cursor = Cursors.Default;

            if (!r.Exito)
            {
                MessageBox.Show(r.Mensaje, "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtPIN.Clear();
                txtPIN.Focus();
                return;
            }

            MostrarIngreso(r.Mensaje);
        }

        private void MostrarIngreso(string mensaje)
        {
            MessageBox.Show(mensaje + "\nCargo: " + Sesion.UsuarioActual.RolNombre,
                "Inicio de sesión exitoso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtPIN.Clear();

            frmPrincipal principal = new frmPrincipal();
            principal.FormClosed += (s, e) =>
            {
                _autenticacion.CerrarSesion();
                this.Close();
            };

            this.Hide();
            principal.Show();
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

                Resultado r = SistemaNegocio.ProbarConexion();
                MessageBox.Show(r.Mensaje, "ORICA SafeHands", MessageBoxButtons.OK,
                    r.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
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

            //if (cbxOperador.SelectedIndex == -1)
            //{
            //    MessageBox.Show("Seleccione un cargo para simular la credencial.",
            //        "Datos incompletos", MessageBoxButtons.OK,
            //        MessageBoxIcon.Warning);

            //    cbxOperador.Focus();
            //    return;
            //}

            //MostrarIngreso();
        }
    }
}
