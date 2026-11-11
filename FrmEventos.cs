using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    // Historial combinado: eventos de operación (dbo.Evento) + cambios administrativos (dbo.Auditoria).
    // Nota: en el diseñador la tabla se llama dgvCriticidad (nombre heredado); muestra eventos.
    public partial class FrmEventos : Form
    {
        private const int MaximoRegistros = 500;

        private readonly clsEventoNegocio eventoNegocio = new clsEventoNegocio();
        private List<RegistroHistorial> registros = new List<RegistroHistorial>();

        public FrmEventos()
        {
            InitializeComponent();

            btnLimpiarDemoEventos.Text = "Actualizar";
        }

        private void FrmEventos_Load(object sender, EventArgs e)
        {
            CargarHistorial();
        }

        private void btnLimpiarDemoEventos_Click(object sender, EventArgs e)
        {
            CargarHistorial();
        }

        private void dgvCriticidad_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) MostrarDetalle(e.RowIndex);
        }

        private void CargarHistorial()
        {
            try
            {
                registros = eventoNegocio.mtdListarHistorial(MaximoRegistros);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el historial.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgvCriticidad.Rows.Clear();

            foreach (RegistroHistorial r in registros)
            {
                int fila = dgvCriticidad.Rows.Add(
                    r.FechaHora.ToString("dd/MM/yyyy HH:mm:ss"),
                    r.Usuario,
                    r.Descripcion,
                    r.ConfianzaTexto,
                    r.CriticidadTexto,
                    r.Resultado,
                    "BD");   // origen del registro en la base de datos

                DataGridViewRow row = dgvCriticidad.Rows[fila];
                row.Tag = r;
                row.Cells[colResultadoEvento.Index].Style.ForeColor = ColorResultado(r.Resultado);
                if (r.IdNivelCriticidad.HasValue)
                    row.Cells[colCriticidadEvento.Index].Style.ForeColor = FrmCriticidad.ColorNivel(r.IdNivelCriticidad.Value);
            }

            dgvCriticidad.ClearSelection();
            lblTituloRegistroEventos.Text = "Eventos y auditoría (" + registros.Count +
                (registros.Count == MaximoRegistros ? " más recientes)" : " registros)");
        }

        // El botón de la columna RESULTADO abre el detalle completo.
        private void dgvEventos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == colResultadoEvento.Index)
                MostrarDetalle(e.RowIndex);
        }

        private void MostrarDetalle(int fila)
        {
            RegistroHistorial r = dgvCriticidad.Rows[fila].Tag as RegistroHistorial;
            if (r == null) return;

            MessageBox.Show(
                "Fecha: " + r.FechaHora.ToString("dd/MM/yyyy HH:mm:ss") + "\n" +
                "Usuario: " + r.Usuario + "\n" +
                "Origen: " + (r.Fuente == "AUDITORIA" ? "Auditoría" : "Evento de operación") + "\n" +
                "Resultado: " + r.Resultado + "\n" +
                (r.Confianza.HasValue ? "Confianza: " + r.ConfianzaTexto + "\n" : "") +
                (r.IdNivelCriticidad.HasValue ? "Criticidad: " + r.CriticidadTexto + "\n" : "") +
                "\n" + r.Detalle,
                "Detalle del registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static Color ColorResultado(string resultado)
        {
            switch (resultado)
            {
                case clsEventoNegocio.ResultadoEjecutado: return Color.MediumAquamarine;
                case clsEventoNegocio.ResultadoRechazado:
                case clsEventoNegocio.ResultadoError: return Color.Salmon;
                case clsEventoNegocio.ResultadoCancelado:
                case clsEventoNegocio.ResultadoIgnorado: return Color.Goldenrod;
                case "AUDITORIA": return Color.MediumPurple;
                default: return Color.LightSkyBlue;
            }
        }

        // ---------------- Exportar ----------------

        private void btnExportarEventosCSV_Click(object sender, EventArgs e)
        {
            if (registros.Count == 0)
            {
                MessageBox.Show("No hay registros para exportar.", "Exportar CSV",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog dialogo = new SaveFileDialog())
            {
                dialogo.Filter = "Archivo CSV (*.csv)|*.csv";
                dialogo.FileName = "ORICA_Eventos_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv";
                if (dialogo.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    StringBuilder csv = new StringBuilder();
                    csv.AppendLine("FechaHora;Origen;Tipo;Usuario;Descripcion;Confianza;Criticidad;Resultado;Detalle");

                    foreach (RegistroHistorial r in registros)
                    {
                        csv.AppendLine(string.Join(";", new[]
                        {
                            r.FechaHora.ToString("yyyy-MM-dd HH:mm:ss"), r.Fuente, r.Tipo, r.Usuario, r.Descripcion,
                            r.Confianza.HasValue ? r.ConfianzaTexto : "", r.IdNivelCriticidad.HasValue ? r.CriticidadTexto : "",
                            r.Resultado, r.Detalle
                        }.Select(v => Escapar(v))));
                    }

                    // UTF-8 con BOM para que Excel muestre bien las tildes.
                    File.WriteAllText(dialogo.FileName, csv.ToString(), new UTF8Encoding(true));

                    MessageBox.Show("Se exportaron " + registros.Count + " registros.", "Exportar CSV",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo guardar el archivo.\n" + ex.Message, "Exportar CSV",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Encierra el valor entre comillas y duplica las comillas internas (formato CSV estándar).
        private static string Escapar(string valor)
        {
            return "\"" + (valor ?? string.Empty).Replace("\"", "\"\"").Replace("\r", " ").Replace("\n", " | ") + "\"";
        }
    }
}
