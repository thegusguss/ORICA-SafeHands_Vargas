using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    // Ventana para registrar una credencial nueva (PIN, RFID o NFC) a un operador.
    public partial class FrmCredencialRegistrar : Form
    {
        private readonly clsCredencialNegocio credencialNegocio = new clsCredencialNegocio();

        public FrmCredencialRegistrar()
        {
            InitializeComponent();
        }

        private void FrmCredencialRegistrar_Load(object sender, EventArgs e)
        {
            try
            {
                // Solo operadores activos.
                cbxOperador.DataSource = new clsUsuarioNegocio().mtdListarUsuarios().Where(u => u.Estado).ToList();
                cbxOperador.DisplayMember = "Etiqueta";
                cbxOperador.ValueMember = "IdUsuario";
                cbxOperador.SelectedIndex = -1;

                cbxTipo.DataSource = credencialNegocio.mtdListarTipos();
                cbxTipo.DisplayMember = "Nombre";
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los datos.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            AjustarCampos();
        }

        private void cbxTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            AjustarCampos();
        }

        // PIN: campo oculto con asteriscos + confirmación. RFID/NFC: campo normal para el UID.
        private void AjustarCampos()
        {
            TipoCredencial tipo = cbxTipo.SelectedItem as TipoCredencial;
            bool esPin = tipo != null && tipo.EsPin;

            lblValor.Text = esPin ? "PIN (4 números) *" : "UID de la tarjeta *";
            txtValor.UseSystemPasswordChar = esPin;
            txtValor.MaxLength = esPin ? 4 : 30;
            lblConfirmar.Visible = esPin;
            txtConfirmar.Visible = esPin;

            txtValor.Clear();
            txtConfirmar.Clear();
        }

        // Las validaciones (formato del PIN, UID, tarjeta repetida) están en clsCredencialNegocio.
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            int idUsuario = cbxOperador.SelectedValue == null ? 0 : (int)cbxOperador.SelectedValue;
            TipoCredencial tipo = cbxTipo.SelectedItem as TipoCredencial;

            Resultado resultado = credencialNegocio.mtdRegistrar(idUsuario, tipo, txtValor.Text, txtConfirmar.Text);

            if (!resultado.Exito)
            {
                // Se queda abierta para que el usuario corrija el dato.
                MessageBox.Show(resultado.Mensaje, "Revise los datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(resultado.Mensaje, "Credenciales",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;  // cierra la ventana y avisa que hubo cambios
        }
    }
}
