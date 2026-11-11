using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    // Catálogo de comandos de voz: crear, editar (con equipos asignados) y activar/desactivar.
    public partial class FrmComandos : Form
    {
        private readonly clsComandoNegocio comandoNegocio = new clsComandoNegocio();

        public FrmComandos()
        {
            InitializeComponent();
        }

        private void FrmComandos_Load(object sender, EventArgs e)
        {
            CargarComandos();
        }

        private void btnNuevoComando_Click(object sender, EventArgs e)
        {
            AbrirEditor(null);
        }

        private void dgvComandos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Comando comando = (Comando)dgvComandos.Rows[e.RowIndex].Tag;

            if (e.ColumnIndex == colEditarComando.Index)
                AbrirEditor(comando);
            else if (e.ColumnIndex == colCambiarEstadoComando.Index)
                CambiarEstado(comando);
        }

        private void CargarComandos()
        {
            List<Comando> comandos;
            try
            {
                comandos = comandoNegocio.mtdListar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el catálogo de comandos.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgvComandos.Rows.Clear();

            foreach (Comando c in comandos)
            {
                int fila = dgvComandos.Rows.Add(
                    c.Frase,
                    c.CriticidadTexto,
                    c.ConfianzaMinima.ToString("0.##") + " %",
                    c.RequierePTT ? "Sí" : "No",
                    c.NumeroConfirmaciones,
                    c.EstadoTexto,
                    "Editar",
                    c.Estado ? "Desactivar" : "Activar");

                DataGridViewRow row = dgvComandos.Rows[fila];
                row.Tag = c;
                row.Cells[colCriticidadComando.Index].Style.ForeColor = FrmCriticidad.ColorNivel(c.IdNivelCriticidad);

                // La columna de estado parece un enlace para que se note que se puede pulsar.
                DataGridViewCell celdaEstado = row.Cells[colCambiarEstadoComando.Index];
                celdaEstado.Style.ForeColor = c.Estado ? Color.Salmon : Color.MediumAquamarine;
                celdaEstado.Style.Font = new Font(dgvComandos.Font, FontStyle.Underline);
            }

            dgvComandos.ClearSelection();
        }

        private void AbrirEditor(Comando comando)
        {
            using (FrmComandoEditar editor = new FrmComandoEditar(comando))
            {
                if (editor.ShowDialog(this) == DialogResult.OK)
                    CargarComandos();
            }
        }

        private void CambiarEstado(Comando comando)
        {
            bool activar = !comando.Estado;
            string accion = activar ? "activar" : "desactivar";

            DialogResult respuesta = MessageBox.Show(
                "¿Desea " + accion + " el comando \"" + comando.Frase + "\"?" +
                (activar ? "" : "\nLos operadores ya no podrán usarlo."),
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes) return;

            Resultado resultado = comandoNegocio.mtdCambiarEstado(comando.IdComando, activar);
            MessageBox.Show(resultado.Mensaje, "Comandos", MessageBoxButtons.OK,
                resultado.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (resultado.Exito)
                CargarComandos();
        }
    }
}
