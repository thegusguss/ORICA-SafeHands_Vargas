using PROYECTO_ORICA_SAFEHANDS_VARGAS.Datos;
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

        public frmLogin()
        {
            InitializeComponent();
            cbxOperador.SelectedIndex = -1;
            txtPIN.Clear();
            this.AcceptButton = btnIngresar;
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
                MessageBox.Show("Seleccione su cargo.", "Datos incompletos",
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

            if (pin != "1234")
            {
                MessageBox.Show("El PIN es incorrecto.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtPIN.Clear();
                txtPIN.Focus();
                return;
            }

            MostrarIngreso();

        }

        private void MostrarIngreso()
            {
            string cargo = cbxOperador.SelectedItem.ToString();

            MessageBox.Show("Bienvenido.\nCargo: " + cargo,
                "Inicio de sesión exitoso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtPIN.Clear();
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

                if (Conexion.ProbarConexion())
                {
                    MessageBox.Show(
                        "Conexión correcta con ORICA_SafeHandsDB",
                        "ORICA SafeHands",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
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
