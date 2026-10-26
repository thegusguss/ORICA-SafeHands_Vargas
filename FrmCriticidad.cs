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
    public partial class FrmCriticidad : Form
    {
        public FrmCriticidad()
        {
            InitializeComponent();
        }

        private void FrmCriticidad_Load(object sender, EventArgs e)
        {
            dgvCriticidad.Rows.Clear();

            dgvCriticidad.Rows.Add(
                "Nivel 1", "Informativo", 0, "—",
                "Consulta o lectura de estado.");

            dgvCriticidad.Rows.Add(
                "Nivel 2", "Bajo", 1, "5 s",
                "Acción reversible de bajo impacto.");

            dgvCriticidad.Rows.Add(
                "Nivel 3", "Alto", 1, "5 s",
                "Requiere confirmación explícita.");

            dgvCriticidad.Rows.Add(
                "Nivel 4", "Crítico", 2, "5 s",
                "Doble confirmación y timeout.");

            dgvCriticidad.Rows[0].Cells["colNivelCriticidad"].Style.ForeColor = Color.MediumAquamarine;
            dgvCriticidad.Rows[1].Cells["colNivelCriticidad"].Style.ForeColor = Color.LightSkyBlue;
            dgvCriticidad.Rows[2].Cells["colNivelCriticidad"].Style.ForeColor = Color.Goldenrod;
            dgvCriticidad.Rows[3].Cells["colNivelCriticidad"].Style.ForeColor = Color.Salmon;

            dgvCriticidad.ClearSelection();
        }
    }
}
