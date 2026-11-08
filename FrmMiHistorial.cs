using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    // Historial del operador que inició sesión: sus comandos de voz, paradas y sesiones (dbo.Evento).
    public partial class FrmMiHistorial : Form
    {
        private const int MaximoRegistros = 200;

        // Opciones del filtro "Resultado" → valor guardado en dbo.Evento (null = todos).
        private static readonly string[][] Filtros =
        {
            new[] { "Todos los resultados", null },
            new[] { "Ejecutados", "EJECUTADO" },
            new[] { "Cancelados / sin respuesta", "CANCELADO" },
            new[] { "Rechazados", "RECHAZADO" },
            new[] { "Sesiones (inicio / cierre)", "INFORMATIVO" },
            new[] { "Errores", "ERROR" }
        };

        private readonly clsEventoNegocio eventoNegocio = new clsEventoNegocio();
        private List<RegistroHistorial> registros = new List<RegistroHistorial>();

        public FrmMiHistorial()
        {
            InitializeComponent();

            lblTituloAccionesHistorial.Text = "Mis acciones recientes";

            cbxResultadoHistorial.Items.Clear();
            foreach (string[] f in Filtros) cbxResultadoHistorial.Items.Add(f[0]);
            cbxResultadoHistorial.SelectedIndex = 0;

            // Los eventos se conectan aquí para no tocar el diseñador.
            Load += (s, e) => CargarHistorial();
            txtBuscarHistorial.TextChanged += (s, e) => MostrarRegistros();
            cbxResultadoHistorial.SelectedIndexChanged += (s, e) => MostrarRegistros();
            dgvMiHistorial.CellContentClick += dgvMiHistorial_CellContentClick;
            dgvMiHistorial.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) MostrarDetalle(e.RowIndex); };
        }

        // Lee de la BD solo los eventos del operador en sesión.
        private void CargarHistorial()
        {
            if (!Sesion.Activa)
            {
                lblCantidadRegistrosHistorial.Text = "Sin sesión";
                return;
            }

            try
            {
                registros = eventoNegocio.mtdListarHistorialUsuario(Sesion.UsuarioActual.IdUsuario, MaximoRegistros);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar tu historial.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MostrarRegistros();
        }

        // Aplica el buscador y el filtro de resultado sobre lo ya cargado (sin volver a la BD).
        private void MostrarRegistros()
        {
            string busqueda = txtBuscarHistorial.Text.Trim();
            int indice = Math.Max(0, cbxResultadoHistorial.SelectedIndex);
            string resultado = indice < Filtros.Length ? Filtros[indice][1] : null;

            IEnumerable<RegistroHistorial> lista = registros;

            if (resultado != null)
                lista = lista.Where(r => r.Resultado == resultado);

            if (busqueda.Length > 0)
                lista = lista.Where(r => Contiene(r.Comando, busqueda) || Contiene(r.Equipo, busqueda) ||
                                         Contiene(r.Descripcion, busqueda));

            dgvMiHistorial.Rows.Clear();

            foreach (RegistroHistorial r in lista)
            {
                int fila = dgvMiHistorial.Rows.Add(
                    r.FechaHora.ToString("dd/MM HH:mm:ss"),
                    r.Equipo ?? "—",
                    r.Comando,
                    r.ResultadoTexto);
                // La columna "Detalle" muestra "Ver detalle" sola (UseColumnTextForButtonValue).

                DataGridViewRow row = dgvMiHistorial.Rows[fila];
                row.Tag = r;
                row.Cells[colResultadoHistorial.Index].Style.ForeColor = ColorResultado(r.Resultado);
            }

            dgvMiHistorial.ClearSelection();

            int total = dgvMiHistorial.Rows.Count;
            lblCantidadRegistrosHistorial.Text = total + (total == 1 ? " registro" : " registros");
        }

        private void dgvMiHistorial_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == colDetalleHistorial.Index)
                MostrarDetalle(e.RowIndex);
        }

        private void MostrarDetalle(int fila)
        {
            RegistroHistorial r = dgvMiHistorial.Rows[fila].Tag as RegistroHistorial;
            if (r == null) return;

            string texto =
                "Fecha: " + r.FechaHora.ToString("dd/MM/yyyy HH:mm:ss") + "\n" +
                "Equipo: " + (r.Equipo ?? "—") + "\n" +
                "Comando: " + r.Comando + "\n" +
                (string.IsNullOrEmpty(r.TextoDetectado) ? "" : "Se escuchó: \"" + r.TextoDetectado + "\"\n") +
                (r.Confianza.HasValue ? "Confianza: " + r.ConfianzaTexto + "\n" : "") +
                (r.IdNivelCriticidad.HasValue ? "Criticidad: " + r.CriticidadTexto + "\n" : "") +
                "Resultado: " + r.ResultadoTexto + "\n\n" +
                r.Detalle;

            MessageBox.Show(texto, "Detalle de la acción", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static bool Contiene(string texto, string busqueda)
        {
            return texto != null && texto.IndexOf(busqueda, StringComparison.CurrentCultureIgnoreCase) >= 0;
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
                default: return Color.LightSkyBlue;
            }
        }
    }
}
