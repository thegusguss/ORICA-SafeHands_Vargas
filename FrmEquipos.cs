using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    // Lista de equipos: crear, editar y activar/desactivar.
    public partial class FrmEquipos : Form
    {
        private readonly clsEquipoNegocio equipoNegocio = new clsEquipoNegocio();

        public FrmEquipos()
        {
            InitializeComponent();

            // Los eventos se conectan aquí para no tocar el diseñador.
            Load += FrmEquipos_Load;
            btnNuevoEquipo.Click += btnNuevoEquipo_Click;
            dgvEquipos.CellContentClick += dgvEquipos_CellContentClick;
        }

        private void FrmEquipos_Load(object sender, EventArgs e)
        {
            CargarEquipos();
        }

        private void btnNuevoEquipo_Click(object sender, EventArgs e)
        {
            AbrirEditor(null);
        }

        // Clic en un botón de la tabla: "Editar" o "Activar/Desactivar".
        private void dgvEquipos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;  // clic en el encabezado

            Equipo equipo = (Equipo)dgvEquipos.Rows[e.RowIndex].Tag;

            if (e.ColumnIndex == colEditarEquipo.Index)
                AbrirEditor(equipo);
            else if (e.ColumnIndex == colCambiarEstadoEquipo.Index)
                CambiarEstado(equipo);
        }

        private void CargarEquipos()
        {
            List<Equipo> equipos;
            try
            {
                equipos = equipoNegocio.mtdListar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar la lista de equipos.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgvEquipos.Rows.Clear();

            foreach (Equipo eq in equipos)
            {
                int fila = dgvEquipos.Rows.Add(
                    eq.Codigo,
                    eq.Nombre,
                    eq.TipoNombre,
                    eq.EstadoTexto,
                    eq.HeartbeatTexto,
                    "Editar",
                    eq.Estado ? "Desactivar" : "Activar");

                // Se guarda el equipo completo en la fila para usarlo al hacer clic.
                dgvEquipos.Rows[fila].Tag = eq;
            }

            dgvEquipos.ClearSelection();
        }

        // equipo = null → crear uno nuevo; si no, editar el que llega.
        private void AbrirEditor(Equipo equipo)
        {
            using (FrmEquipoEditar editor = new FrmEquipoEditar(equipo))
            {
                if (editor.ShowDialog(this) == DialogResult.OK)
                    CargarEquipos();
            }
        }

        private void CambiarEstado(Equipo equipo)
        {
            bool activar = !equipo.Estado;
            string accion = activar ? "activar" : "desactivar";

            DialogResult respuesta = MessageBox.Show(
                "¿Desea " + accion + " el equipo " + equipo.Etiqueta + "?" +
                (activar ? "" : "\nNo podrá recibir comandos mientras esté inactivo."),
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes) return;

            Resultado resultado = equipoNegocio.mtdCambiarEstado(equipo.IdEquipo, activar);
            MessageBox.Show(resultado.Mensaje, "Equipos", MessageBoxButtons.OK,
                resultado.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (resultado.Exito)
                CargarEquipos();
        }
    }
}
