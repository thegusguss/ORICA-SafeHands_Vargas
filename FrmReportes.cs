using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    // Reportes de actividad: eventos de voz y paradas filtrados por fecha, operador, resultado, criticidad y equipo.
    // Muestra totales, dos gráficos y genera un PDF que se abre con el programa predeterminado.
    public partial class FrmReportes : Form
    {
        private const int MaximoComandosGrafico = 8;   // comandos que entran en el gráfico de confianza

        private static readonly Color ColorPanel = Color.FromArgb(22, 29, 37);
        private static readonly Color ColorLinea = Color.FromArgb(45, 55, 68);

        private readonly clsReporteNegocio reporteNegocio = new clsReporteNegocio();
        private ResultadoReporte ultimoReporte;   // lo último generado (es lo que se imprime)

        // El diseñador no trae rango de fechas: se agregan por código en el panel de filtros.
        private readonly DateTimePicker dtpDesde = new DateTimePicker();
        private readonly DateTimePicker dtpHasta = new DateTimePicker();

        // Opción de un ComboBox de filtro: el texto que se ve y el valor que se envía (null = todos).
        private class Opcion
        {
            public string Texto;
            public object Valor;
            public override string ToString() { return Texto; }
        }

        public FrmReportes()
        {
            InitializeComponent();

            AgregarFechas();
            PrepararGrafico(chtComandosResultado);
            PrepararGrafico(chtConfianzaComando);

            // El gráfico de confianza se ve mejor en barras (en el diseñador estaba como línea).
            Series confianza = chtConfianzaComando.Series[0];
            confianza.ChartType = SeriesChartType.Column;
            confianza.LabelFormat = "0.#";
            chtConfianzaComando.ChartAreas[0].AxisY.Minimum = 0;
            chtConfianzaComando.ChartAreas[0].AxisY.Maximum = 100;

            btnImprimirReporte.Text = "Generar PDF";   // por ahora solo PDF; se imprime desde el lector de PDF

            lblDiagEstadoApp.Text = "● App en línea";
            lblDiagEstadoApp.ForeColor = Color.MediumAquamarine;
            lblConexionESP32.Text = clsMonitorArduino.TextoEstado;
            lblConexionESP32.ForeColor = clsMonitorArduino.Conectado ? Color.MediumAquamarine : Color.Silver;
        }

        private void FrmReportes_Load(object sender, EventArgs e)
        {
            if (CargarFiltros()) Generar();
        }

        private void btnActualizarReportes_Click(object sender, EventArgs e)
        {
            Generar();
        }

        // ---------------- Filtros ----------------

        private void AgregarFechas()
        {
            Label lblDesde = new Label { Text = "Desde", AutoSize = true, ForeColor = Color.Silver, Location = new Point(300, 26) };
            Label lblHasta = new Label { Text = "Hasta", AutoSize = true, ForeColor = Color.Silver, Location = new Point(530, 26) };

            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpDesde.Bounds = new Rectangle(350, 22, 150, 22);
            dtpHasta.Bounds = new Rectangle(580, 22, 150, 22);

            // Por defecto: los últimos 30 días.
            dtpHasta.Value = DateTime.Today;
            dtpDesde.Value = DateTime.Today.AddDays(-29);

            pnlFiltrosReportes.Controls.AddRange(new Control[] { lblDesde, dtpDesde, lblHasta, dtpHasta });
        }

        // Llena los ComboBox desde la BD. Cada uno empieza con "Todos".
        private bool CargarFiltros()
        {
            try
            {
                Llenar(cbxOperadorReporte, new clsUsuarioNegocio().mtdListarUsuarios()
                    .Select(u => new Opcion { Texto = u.Codigo + " - " + u.NombreCompleto, Valor = u.IdUsuario }));

                Llenar(cbxCriticidadReporte, new clsCriticidadNegocio().mtdListar()
                    .Select(n => new Opcion { Texto = n.Codigo + " - " + n.Nombre, Valor = n.IdNivelCriticidad }));

                Llenar(cbxEquipoReporte, new clsEquipoNegocio().mtdListar()
                    .Select(eq => new Opcion { Texto = eq.Codigo + " - " + eq.Nombre, Valor = eq.IdEquipo }));
            }
            catch (Exception ex)
            {
                MostrarConexion(false);
                MessageBox.Show("No se pudieron cargar los filtros.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Los resultados son fijos (los mismos que guarda clsEventoNegocio).
            string[] resultados =
            {
                clsEventoNegocio.ResultadoEjecutado, clsEventoNegocio.ResultadoRechazado,
                clsEventoNegocio.ResultadoCancelado, clsEventoNegocio.ResultadoError, clsEventoNegocio.ResultadoIgnorado
            };
            Llenar(cbxResultadoReporte, resultados.Select(r => new Opcion { Texto = clsReporteNegocio.mtdTextoResultado(r), Valor = r }));

            return true;
        }

        private static void Llenar(ComboBox combo, IEnumerable<Opcion> opciones)
        {
            combo.Items.Clear();
            combo.Items.Add(new Opcion { Texto = "Todos", Valor = null });
            foreach (Opcion o in opciones) combo.Items.Add(o);
            combo.SelectedIndex = 0;
        }

        // Valor elegido en un combo (null si es "Todos" o no hay selección).
        private static object Valor(ComboBox combo)
        {
            Opcion o = combo.SelectedItem as Opcion;
            return o == null ? null : o.Valor;
        }

        private static string Texto(ComboBox combo)
        {
            Opcion o = combo.SelectedItem as Opcion;
            return o == null ? "Todos" : o.Texto;
        }

        private FiltroReporte LeerFiltro()
        {
            return new FiltroReporte
            {
                Desde = dtpDesde.Value.Date,
                Hasta = dtpHasta.Value.Date,
                IdUsuario = (int?)Valor(cbxOperadorReporte),
                Resultado = (string)Valor(cbxResultadoReporte),
                IdNivel = (int?)Valor(cbxCriticidadReporte),
                IdEquipo = (int?)Valor(cbxEquipoReporte),
                OperadorTexto = Texto(cbxOperadorReporte),
                ResultadoTexto = Texto(cbxResultadoReporte),
                NivelTexto = Texto(cbxCriticidadReporte),
                EquipoTexto = Texto(cbxEquipoReporte)
            };
        }

        // ---------------- Generar ----------------

        private void Generar()
        {
            FiltroReporte filtro = LeerFiltro();

            string error = reporteNegocio.mtdValidar(filtro);
            if (error != null)
            {
                MessageBox.Show(error, "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                ultimoReporte = reporteNegocio.mtdGenerar(filtro);
            }
            catch (Exception ex)
            {
                MostrarConexion(false);
                MessageBox.Show("No se pudo generar el reporte.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            MostrarConexion(true);
            MostrarResumen(ultimoReporte);
            MostrarGraficoResultados(ultimoReporte);
            MostrarGraficoConfianza(ultimoReporte);

            if (ultimoReporte.Recortado)
                MessageBox.Show("Hay más de " + clsReporteNegocio.MaximoFilas + " eventos en el rango. " +
                    "Se muestran los más recientes; reduzca el rango de fechas para verlos todos.",
                    "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MostrarConexion(bool conectado)
        {
            lblEstadoDatos.Text = conectado ? "● SQL Server" : "● Sin BD";
            lblEstadoDatos.ForeColor = conectado ? Color.MediumAquamarine : Color.Salmon;
        }

        private void MostrarResumen(ResultadoReporte r)
        {
            // En la tarjeta solo cabe el código del operador (o "Todos").
            lblOperadorResumen.Text = r.Filtro.IdUsuario.HasValue ? r.Filtro.OperadorTexto.Split(' ')[0] : "Todos";
            lblCantidadEventosResumen.Text = r.Total.ToString();
            lblCantidadEjecutadosResumen.Text = r.Ejecutados.ToString();
            lblCantidadRechazadosResumen.Text = r.Rechazados.ToString();

            lblCantidadEjecutadosResumen.ForeColor = Color.MediumAquamarine;
            lblCantidadRechazadosResumen.ForeColor = r.Rechazados > 0 ? Color.Salmon : Color.MediumAquamarine;

            lblTituloResumenAuditoria.Text = "Resumen para auditoría · " +
                r.Filtro.Desde.ToString("dd/MM/yyyy") + " al " + r.Filtro.Hasta.ToString("dd/MM/yyyy");
        }

        private void MostrarGraficoResultados(ResultadoReporte r)
        {
            Series serie = chtComandosResultado.Series[0];
            serie.Points.Clear();

            foreach (KeyValuePair<string, int> item in r.PorResultado)
            {
                int i = serie.Points.AddXY(item.Key, item.Value);
                serie.Points[i].Color = ColorResultado(item.Key);
            }

            MarcarVacio(chtComandosResultado, r.PorResultado.Count == 0);
        }

        private void MostrarGraficoConfianza(ResultadoReporte r)
        {
            Series serie = chtConfianzaComando.Series[0];
            serie.Points.Clear();

            foreach (ConfianzaComando c in r.ConfianzaPorComando.Take(MaximoComandosGrafico))
            {
                int i = serie.Points.AddXY(c.Comando, c.Promedio);
                serie.Points[i].ToolTip = c.Comando + ": " + c.Promedio + " % (" + c.Cantidad + " uso(s))";
                serie.Points[i].Color = c.Promedio >= 85m ? Color.MediumAquamarine
                                      : c.Promedio >= 70m ? Color.Goldenrod : Color.Salmon;
            }

            MarcarVacio(chtConfianzaComando, r.ConfianzaPorComando.Count == 0);
        }

        // ---------------- Gráficos (estilo oscuro) ----------------

        private static void PrepararGrafico(Chart grafico)
        {
            grafico.BackColor = ColorPanel;
            grafico.Legends[0].Enabled = false;   // hay una sola serie: la leyenda sobra

            ChartArea area = grafico.ChartAreas[0];
            area.BackColor = ColorPanel;
            foreach (Axis eje in new[] { area.AxisX, area.AxisY })
            {
                eje.LabelStyle.ForeColor = Color.Silver;
                eje.LineColor = ColorLinea;
                eje.MajorGrid.LineColor = ColorLinea;
                eje.MajorTickMark.LineColor = ColorLinea;
            }
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.Interval = 1;               // que se vea el nombre de cada barra
            area.AxisX.LabelStyle.Angle = -30;

            Series serie = grafico.Series[0];
            serie.LabelForeColor = Color.White;
            serie.IsValueShownAsLabel = true;
        }

        // Muestra "Sin datos" encima del gráfico cuando no hay nada que dibujar.
        private static void MarcarVacio(Chart grafico, bool vacio)
        {
            grafico.Titles.Clear();
            if (vacio)
                grafico.Titles.Add(new Title("Sin datos para los filtros elegidos", Docking.Top,
                                             new Font("Segoe UI", 10), Color.Silver));
        }

        private static Color ColorResultado(string texto)
        {
            switch (texto)
            {
                case "Ejecutado": return Color.MediumAquamarine;
                case "Rechazado":
                case "Error": return Color.Salmon;
                case "Cancelado":
                case "Ignorado": return Color.Goldenrod;
                default: return Color.LightSkyBlue;
            }
        }

        // ---------------- Generar PDF ----------------

        // Pide dónde guardar, genera el PDF y lo abre con el programa predeterminado (navegador o lector de PDF).
        // Desde ese programa se puede imprimir.
        private void btnImprimirReporte_Click(object sender, EventArgs e)
        {
            if (ultimoReporte == null || ultimoReporte.Total == 0)
            {
                MessageBox.Show("No hay datos para el PDF. Pulse Actualizar con otros filtros.",
                    "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string ruta;
            using (SaveFileDialog dialogo = new SaveFileDialog())
            {
                dialogo.Filter = "Documento PDF (*.pdf)|*.pdf";
                dialogo.FileName = "ORICA_Reporte_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".pdf";
                if (dialogo.ShowDialog(this) != DialogResult.OK) return;
                ruta = dialogo.FileName;
            }

            Cursor = Cursors.WaitCursor;
            Resultado resultado = clsImpresionReporte.mtdGuardarPdf(ultimoReporte, ruta);
            Cursor = Cursors.Default;

            if (!resultado.Exito)
            {
                MessageBox.Show(resultado.Mensaje, "Generar PDF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });   // lo abre el programa predeterminado
            }
            catch (Exception)
            {
                MessageBox.Show(resultado.Mensaje + "\n\nNo hay un programa para abrir PDF; ábralo desde esa carpeta.",
                    "Generar PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
