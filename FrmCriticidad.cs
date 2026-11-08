using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    // Niveles de criticidad leídos desde dbo.NivelCriticidad; el botón ACCIÓN abre el editor.
    public partial class FrmCriticidad : Form
    {
        private readonly clsCriticidadNegocio criticidadNegocio = new clsCriticidadNegocio();

        public FrmCriticidad()
        {
            InitializeComponent();
            dgvCriticidad.CellContentClick += dgvCriticidad_CellContentClick;
        }

        private void FrmCriticidad_Load(object sender, EventArgs e)
        {
            CargarNiveles();
        }

        private void CargarNiveles()
        {
            List<NivelCriticidad> niveles;
            try
            {
                niveles = criticidadNegocio.mtdListar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los niveles de criticidad.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgvCriticidad.Rows.Clear();

            foreach (NivelCriticidad n in niveles)
            {
                int fila = dgvCriticidad.Rows.Add(n.NivelTexto, n.Nombre, n.NumeroConfirmaciones,
                                                  n.TimeoutTexto, n.Descripcion, "Editar");

                dgvCriticidad.Rows[fila].Tag = n;
                dgvCriticidad.Rows[fila].Cells["colNivelCriticidad"].Style.ForeColor = ColorNivel(n.IdNivelCriticidad);
            }

            dgvCriticidad.ClearSelection();
        }

        private void dgvCriticidad_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colEditarCriticidad.Index) return;

            NivelCriticidad nivel = (NivelCriticidad)dgvCriticidad.Rows[e.RowIndex].Tag;

            using (FrmCriticidadEditar editor = new FrmCriticidadEditar(nivel))
            {
                if (editor.ShowDialog(this) == DialogResult.OK)
                    CargarNiveles();
            }
        }

        // Mismo color por nivel en todas las pantallas (también lo usa Comandos).
        public static Color ColorNivel(int idNivel)
        {
            switch (idNivel)
            {
                case 1: return Color.MediumAquamarine;
                case 2: return Color.LightSkyBlue;
                case 3: return Color.Goldenrod;
                case 4: return Color.Salmon;
                default: return Color.White;
            }
        }
    }
}
