using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    /// <summary>
    /// Ventana para crear o editar un equipo.
    /// Si recibe null es "Nuevo"; si recibe un equipo, es "Editar".
    /// </summary>
    public partial class FrmEquipoEditar : Form
    {
        private readonly clsEquipoNegocio equipoNegocio = new clsEquipoNegocio();
        private readonly Equipo equipoOriginal;  // null cuando es nuevo

        public FrmEquipoEditar(Equipo equipo)
        {
            InitializeComponent();
            equipoOriginal = equipo;
        }

        private void FrmEquipoEditar_Load(object sender, EventArgs e)
        {
            CargarTipos();

            Equipo equipo = equipoOriginal;
            if (equipo == null)
            {
                lblTitulo.Text = "Nuevo equipo";
            }
            else
            {
                lblTitulo.Text = "Editar equipo";
                txtCodigo.Text = equipo.Codigo;
                txtNombre.Text = equipo.Nombre;
                cbxTipo.SelectedValue = equipo.IdTipoEquipo;
                txtHardware.Text = equipo.IdentificadorHardware;
                txtUbicacion.Text = equipo.UbicacionReferencia;
                txtDescripcion.Text = equipo.Descripcion;
            }
        }

        private void CargarTipos()
        {
            try
            {
                cbxTipo.DataSource = equipoNegocio.mtdListarTipos();
                cbxTipo.DisplayMember = "Nombre";
                cbxTipo.ValueMember = "IdTipoEquipo";
                cbxTipo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los tipos de equipo.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Las validaciones (obligatorios, longitudes, duplicados) están en clsEquipoNegocio.
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Equipo datos = new Equipo
            {
                Codigo = txtCodigo.Text,
                Nombre = txtNombre.Text,
                IdTipoEquipo = cbxTipo.SelectedValue == null ? 0 : (int)cbxTipo.SelectedValue,
                IdentificadorHardware = txtHardware.Text,
                UbicacionReferencia = txtUbicacion.Text,
                Descripcion = txtDescripcion.Text
            };

            Resultado resultado;
            if (equipoOriginal == null)
            {
                resultado = equipoNegocio.mtdRegistrar(datos);
            }
            else
            {
                datos.IdEquipo = equipoOriginal.IdEquipo;
                datos.RowVersion = equipoOriginal.RowVersion;
                resultado = equipoNegocio.mtdModificar(datos);
            }

            if (!resultado.Exito)
            {
                MessageBox.Show(resultado.Mensaje, "Revise los datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(resultado.Mensaje, "Equipos",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
        }
    }
}
