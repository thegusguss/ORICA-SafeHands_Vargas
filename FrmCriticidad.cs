using System;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    public partial class FrmCriticidad : Form
    {
        public FrmCriticidad()
        {
            InitializeComponent();
        }

        // Llena la tabla con los 4 niveles de criticidad y pinta cada nivel con su color.
        private void FrmCriticidad_Load(object sender, EventArgs e)
        {
            dgvCriticidad.Rows.Clear();

            AgregarNivel("Nivel 1", "Informativo", 0, "—", "Consulta o lectura de estado.", Color.MediumAquamarine);
            AgregarNivel("Nivel 2", "Bajo", 1, "5 s", "Acción reversible de bajo impacto.", Color.LightSkyBlue);
            AgregarNivel("Nivel 3", "Alto", 1, "5 s", "Requiere confirmación explícita.", Color.Goldenrod);
            AgregarNivel("Nivel 4", "Crítico", 2, "5 s", "Doble confirmación y timeout.", Color.Salmon);

            dgvCriticidad.ClearSelection();
        }

        private void AgregarNivel(string nivel, string nombre, int confirmaciones,
                                  string tiempo, string descripcion, Color color)
        {
            int fila = dgvCriticidad.Rows.Add(nivel, nombre, confirmaciones, tiempo, descripcion);
            dgvCriticidad.Rows[fila].Cells["colNivelCriticidad"].Style.ForeColor = color;
        }
    }
}
