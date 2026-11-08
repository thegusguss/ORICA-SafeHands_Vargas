using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    /// <summary>
    /// Ventana para crear o editar un comando de voz y elegir en qué equipos se puede usar.
    /// Si recibe null es "Nuevo"; si recibe un comando, es "Editar".
    /// </summary>
    public partial class FrmComandoEditar : Form
    {
        private readonly clsComandoNegocio comandoNegocio = new clsComandoNegocio();
        private readonly Comando comandoOriginal;  // null cuando es nuevo

        public FrmComandoEditar(Comando comando)
        {
            InitializeComponent();
            comandoOriginal = comando;
        }

        private void FrmComandoEditar_Load(object sender, EventArgs e)
        {
            numConfianza.Minimum = clsComandoNegocio.MinConfianza;
            numConfianza.Maximum = clsComandoNegocio.MaxConfianza;
            numConfianza.Value = 85;
            chkPTT.Checked = true;

            List<int> asignados = new List<int>();

            try
            {
                cbxCriticidad.DataSource = new clsCriticidadNegocio().mtdListarActivos();
                cbxCriticidad.DisplayMember = "Etiqueta";
                cbxCriticidad.ValueMember = "IdNivelCriticidad";
                cbxCriticidad.SelectedIndex = -1;

                cbxTipoAccion.DataSource = comandoNegocio.mtdListarTiposAccion();
                cbxTipoAccion.DisplayMember = "Nombre";
                cbxTipoAccion.ValueMember = "IdTipoAccion";
                cbxTipoAccion.SelectedIndex = -1;

                if (comandoOriginal != null)
                    asignados = comandoNegocio.mtdListarEquiposAsignados(comandoOriginal.IdComando);

                // Solo equipos activos, más los inactivos que ya estaban asignados (para no perderlos al guardar).
                clbEquipos.Items.Clear();
                foreach (Equipo eq in new clsEquipoNegocio().mtdListar())
                {
                    bool marcado = asignados.Contains(eq.IdEquipo);
                    if (eq.Estado || marcado)
                        clbEquipos.Items.Add(eq, marcado);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los datos.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Comando c = comandoOriginal;
            if (c == null)
            {
                lblTitulo.Text = "Nuevo comando";
            }
            else
            {
                lblTitulo.Text = "Editar comando";
                txtCodigo.Text = c.Codigo;
                txtNombre.Text = c.Nombre;
                txtFrase.Text = c.Frase;
                cbxCriticidad.SelectedValue = c.IdNivelCriticidad;
                cbxTipoAccion.SelectedValue = c.IdTipoAccion;
                numConfianza.Value = Math.Max(numConfianza.Minimum, Math.Min(numConfianza.Maximum, c.ConfianzaMinima));
                chkPTT.Checked = c.RequierePTT;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            List<int> equipos = new List<int>();
            foreach (object item in clbEquipos.CheckedItems)
                equipos.Add(((Equipo)item).IdEquipo);

            Comando datos = new Comando
            {
                Codigo = txtCodigo.Text,
                Nombre = txtNombre.Text,
                Frase = txtFrase.Text,
                IdNivelCriticidad = cbxCriticidad.SelectedValue == null ? 0 : (int)cbxCriticidad.SelectedValue,
                IdTipoAccion = cbxTipoAccion.SelectedValue == null ? 0 : (int)cbxTipoAccion.SelectedValue,
                ConfianzaMinima = numConfianza.Value,
                RequierePTT = chkPTT.Checked,
                IdsEquipos = equipos
            };

            Resultado resultado;
            if (comandoOriginal == null)
            {
                resultado = comandoNegocio.mtdRegistrar(datos);
            }
            else
            {
                datos.IdComando = comandoOriginal.IdComando;
                datos.RowVersion = comandoOriginal.RowVersion;
                resultado = comandoNegocio.mtdModificar(datos);
            }

            if (!resultado.Exito)
            {
                MessageBox.Show(resultado.Mensaje, "Revise los datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(resultado.Mensaje, "Comandos",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
        }
    }
}
