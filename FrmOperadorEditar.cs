using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    public partial class FrmOperadorEditar : Form
    {
        private readonly clsUsuarioNegocio usuarioNegocio = new clsUsuarioNegocio();
        private readonly Usuario usuarioOriginal; 

        public FrmOperadorEditar(Usuario usuario)
        {
            InitializeComponent();
            usuarioOriginal = usuario;
        }

        // Llena los roles y, si es edición, los datos del operador.
        private void FrmOperadorEditar_Load(object sender, EventArgs e)
        {
            CargarRoles();

            Usuario usuario = usuarioOriginal;
            if (usuario == null)
            {
                lblTitulo.Text = "Nuevo operador";
            }
            else
            {
                lblTitulo.Text = "Editar operador";
                txtCodigo.Text = usuario.Codigo;
                txtNombres.Text = usuario.Nombres;
                txtApellidos.Text = usuario.Apellidos;
                txtDocumento.Text = usuario.Documento;
                txtTelefono.Text = usuario.Telefono;
                txtCorreo.Text = usuario.Correo;
                cbxRol.SelectedValue = usuario.IdRol;
            }
        }

        private void CargarRoles()
        {
            try
            {
                cbxRol.DataSource = new clsRolNegocio().mtdListarRolesActivos();
                cbxRol.DisplayMember = "Nombre";
                cbxRol.ValueMember = "IdRol";
                cbxRol.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los roles.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Las validaciones (obligatorios, correo, duplicados...) están en clsUsuarioNegocio.
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Usuario datos = new Usuario
            {
                Codigo = txtCodigo.Text,
                Nombres = txtNombres.Text,
                Apellidos = txtApellidos.Text,
                Documento = txtDocumento.Text,
                Telefono = txtTelefono.Text,
                Correo = txtCorreo.Text,
                IdRol = cbxRol.SelectedValue == null ? 0 : (int)cbxRol.SelectedValue
            };

            Resultado resultado;
            if (usuarioOriginal == null)
            {
                resultado = usuarioNegocio.mtdRegistrar(datos);
            }
            else
            {
                // Id y RowVersion le dicen a la BD qué registro es y si alguien lo cambió antes.
                datos.IdUsuario = usuarioOriginal.IdUsuario;
                datos.RowVersion = usuarioOriginal.RowVersion;
                resultado = usuarioNegocio.mtdModificar(datos);
            }

            if (!resultado.Exito)
            {
                // Se queda abierta para que el usuario corrija el dato.
                MessageBox.Show(resultado.Mensaje, "Revise los datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(resultado.Mensaje, "Operadores",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;  // cierra la ventana y avisa que hubo cambios
        }
    }
}
