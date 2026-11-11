using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    // Edita confirmaciones, timeout y descripción de un nivel de criticidad.
    public partial class FrmCriticidadEditar : Form
    {
        private readonly clsCriticidadNegocio criticidadNegocio = new clsCriticidadNegocio();
        private readonly NivelCriticidad nivel;

        public FrmCriticidadEditar(NivelCriticidad nivel)
        {
            InitializeComponent();
            this.nivel = nivel;
        }

        private void numConfirmaciones_ValueChanged(object sender, EventArgs e)
        {
            AjustarTimeout();
        }

        private void FrmCriticidadEditar_Load(object sender, EventArgs e)
        {
            lblTitulo.Text = "Nivel " + nivel.IdNivelCriticidad + " · " + nivel.Nombre;

            numConfirmaciones.Maximum = clsCriticidadNegocio.MaxConfirmaciones;
            numTimeout.Minimum = 0;
            numTimeout.Maximum = clsCriticidadNegocio.MaxTimeout;

            numConfirmaciones.Value = nivel.NumeroConfirmaciones;
            numTimeout.Value = Math.Min(nivel.TimeoutSegundos, clsCriticidadNegocio.MaxTimeout);
            txtDescripcion.Text = nivel.Descripcion;

            AjustarTimeout();
        }

        // Sin confirmaciones no hay timeout.
        private void AjustarTimeout()
        {
            bool usaTimeout = numConfirmaciones.Value > 0;
            numTimeout.Enabled = usaTimeout;
            if (!usaTimeout) numTimeout.Value = 0;
            else if (numTimeout.Value < clsCriticidadNegocio.MinTimeout) numTimeout.Value = clsCriticidadNegocio.MinTimeout;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            NivelCriticidad datos = new NivelCriticidad
            {
                IdNivelCriticidad = nivel.IdNivelCriticidad,
                Codigo = nivel.Codigo,
                Nombre = nivel.Nombre,
                NumeroConfirmaciones = (int)numConfirmaciones.Value,
                TimeoutSegundos = (int)numTimeout.Value,
                Descripcion = txtDescripcion.Text
            };

            Resultado resultado = criticidadNegocio.mtdModificar(datos);

            if (!resultado.Exito)
            {
                MessageBox.Show(resultado.Mensaje, "Revise los datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(resultado.Mensaje, "Criticidad",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
        }
    }
}
