using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    // Dibuja el reporte en hojas A4 y lo guarda como PDF.
    // Página 1: encabezado, filtros, resumen y tablas; después, el detalle de eventos (en tantas páginas como haga falta).
    public class clsImpresionReporte
    {
        private const string ImpresoraPdf = "Microsoft Print to PDF";

        private const int AltoFila = 18;
        private const int MaximoComandosTabla = 10;

        // Columnas del detalle: título y ancho (en centésimas de pulgada).
        private static readonly string[] Titulos = { "Fecha", "Operador", "Comando", "Equipo", "Nivel", "Confianza", "Resultado" };
        private static readonly int[] Anchos = { 95, 140, 215, 60, 40, 65, 75 };

        private readonly ResultadoReporte reporte;
        private readonly Font fuenteTitulo = new Font("Segoe UI", 14, FontStyle.Bold);
        private readonly Font fuenteSeccion = new Font("Segoe UI", 10, FontStyle.Bold);
        private readonly Font fuente = new Font("Segoe UI", 8);
        private readonly Font fuenteNegrita = new Font("Segoe UI", 8, FontStyle.Bold);

        private int filaActual;   // siguiente fila del detalle a dibujar
        private int pagina;

        private clsImpresionReporte(ResultadoReporte reporte)
        {
            this.reporte = reporte;
        }

        // Guarda el reporte como PDF en la ruta indicada, sin mostrar ninguna ventana.
        // Usa la impresora virtual "Microsoft Print to PDF" que trae Windows 10/11, enviando la salida a un archivo.
        public static Resultado mtdGuardarPdf(ResultadoReporte reporte, string ruta)
        {
            if (!PrinterSettings.InstalledPrinters.Cast<string>().Contains(ImpresoraPdf))
                return Resultado.Error("No se encontró la impresora \"" + ImpresoraPdf + "\" en Windows.\n" +
                    "Actívela en Configuración → Impresoras y escáneres.");

            clsImpresionReporte impresion = new clsImpresionReporte(reporte);
            try
            {
                if (File.Exists(ruta)) File.Delete(ruta);   // se reemplaza el archivo anterior

                using (PrintDocument documento = new PrintDocument())
                {
                    documento.DocumentName = Path.GetFileNameWithoutExtension(ruta);
                    documento.PrinterSettings.PrinterName = ImpresoraPdf;
                    documento.PrinterSettings.PrintToFile = true;          // no pregunta dónde guardar
                    documento.PrinterSettings.PrintFileName = ruta;
                    documento.PrintController = new StandardPrintController();   // sin la ventanita de "Imprimiendo..."
                    documento.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);
                    documento.PrintPage += impresion.DibujarPagina;
                    documento.Print();
                }

                // Windows termina de escribir el archivo un instante después: se espera a que exista.
                if (!EsperarArchivo(ruta))
                    return Resultado.Error("Windows no terminó de crear el PDF. Intente de nuevo.");

                return Resultado.Ok("PDF guardado en:\n" + ruta);
            }
            catch (Exception ex)
            {
                return Resultado.Error("No se pudo generar el PDF.\n" + ex.Message);
            }
            finally
            {
                impresion.LiberarFuentes();
            }
        }

        // Espera hasta 15 segundos a que el PDF exista y se pueda abrir (ya no lo esté escribiendo Windows).
        private static bool EsperarArchivo(string ruta)
        {
            for (int intento = 0; intento < 60; intento++)
            {
                try
                {
                    if (File.Exists(ruta) && new FileInfo(ruta).Length > 0)
                    {
                        using (File.Open(ruta, FileMode.Open, FileAccess.Read, FileShare.None)) { }
                        return true;
                    }
                }
                catch (IOException) { /* todavía se está escribiendo */ }

                Thread.Sleep(250);
            }
            return false;
        }

        private void DibujarPagina(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle area = e.MarginBounds;
            int y = area.Top;
            pagina++;

            if (pagina == 1)
                y = DibujarPortada(g, area, y);

            // Detalle de eventos
            g.DrawString(pagina == 1 ? "Detalle de eventos" : "Detalle de eventos (continuación)", fuenteSeccion, Brushes.Black, area.Left, y);
            y += 22;
            y = DibujarFila(g, area.Left, y, Titulos, fuenteNegrita, true);

            while (filaActual < reporte.Filas.Count && y + AltoFila <= area.Bottom - 20)
            {
                FilaReporte f = reporte.Filas[filaActual];
                y = DibujarFila(g, area.Left, y, new[]
                {
                    f.FechaHora.ToString("dd/MM/yy HH:mm"), f.Operador, f.Comando, f.Equipo, f.Nivel,
                    f.ConfianzaTexto, clsReporteNegocio.mtdTextoResultado(f.Resultado)
                }, fuente, false);
                filaActual++;
            }

            // Pie de página
            g.DrawString("ORICA SafeHands · Página " + pagina, fuente, Brushes.Gray, area.Left, area.Bottom);

            e.HasMorePages = filaActual < reporte.Filas.Count;
        }

        // Encabezado, filtros, resumen y tablas de la primera página. Devuelve la nueva posición Y.
        private int DibujarPortada(Graphics g, Rectangle area, int y)
        {
            FiltroReporte f = reporte.Filtro;
            string usuario = Sesion.Activa ? Sesion.UsuarioActual.Codigo + " - " + Sesion.UsuarioActual.NombreCompleto : "—";

            g.DrawString("ORICA SafeHands · Reporte de actividad", fuenteTitulo, Brushes.Black, area.Left, y);
            y += 30;
            g.DrawString("Generado el " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " por " + usuario, fuente, Brushes.Gray, area.Left, y);
            y += 25;

            // Filtros
            g.DrawString("Filtros", fuenteSeccion, Brushes.Black, area.Left, y);
            y += 20;
            y = Linea(g, area.Left, y, "Periodo: " + f.Desde.ToString("dd/MM/yyyy") + " al " + f.Hasta.ToString("dd/MM/yyyy"));
            y = Linea(g, area.Left, y, "Operador: " + f.OperadorTexto + "    Resultado: " + f.ResultadoTexto);
            y = Linea(g, area.Left, y, "Criticidad: " + f.NivelTexto + "    Equipo: " + f.EquipoTexto);
            y += 10;

            // Resumen
            g.DrawString("Resumen", fuenteSeccion, Brushes.Black, area.Left, y);
            y += 20;
            y = Linea(g, area.Left, y, "Eventos: " + reporte.Total + "    Ejecutados: " + reporte.Ejecutados +
                                       "    Rechazados: " + reporte.Rechazados + "    Cancelados: " + reporte.Cancelados);
            if (reporte.Recortado)
                y = Linea(g, area.Left, y, "Atención: se incluyen solo los " + clsReporteNegocio.MaximoFilas + " eventos más recientes.");
            y += 10;

            // Tabla: eventos por resultado
            g.DrawString("Eventos por resultado", fuenteSeccion, Brushes.Black, area.Left, y);
            y += 20;
            foreach (var item in reporte.PorResultado)
            {
                int porcentaje = reporte.Total == 0 ? 0 : item.Value * 100 / reporte.Total;
                y = Linea(g, area.Left, y, item.Key + ": " + item.Value + " (" + porcentaje + " %)");
            }
            y += 10;

            // Tabla: confianza por comando
            if (reporte.ConfianzaPorComando.Count > 0)
            {
                g.DrawString("Confianza promedio por comando", fuenteSeccion, Brushes.Black, area.Left, y);
                y += 20;
                foreach (ConfianzaComando c in reporte.ConfianzaPorComando.Take(MaximoComandosTabla))
                    y = Linea(g, area.Left, y, c.Comando + ": " + c.Promedio + " % en " + c.Cantidad + " uso(s)");
                y += 10;
            }

            return y;
        }

        private int Linea(Graphics g, int x, int y, string texto)
        {
            g.DrawString(texto, fuente, Brushes.Black, x, y);
            return y + 16;
        }

        // Dibuja una fila de la tabla; si el texto no cabe en su columna, se corta con "…".
        private int DibujarFila(Graphics g, int x, int y, string[] valores, Font letra, bool esTitulo)
        {
            using (StringFormat formato = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap })
            {
                int ancho = Anchos.Sum();
                if (esTitulo) g.FillRectangle(Brushes.Gainsboro, x, y, ancho, AltoFila);

                int columnaX = x;
                for (int i = 0; i < valores.Length; i++)
                {
                    g.DrawString(valores[i] ?? "", letra, Brushes.Black, new RectangleF(columnaX + 2, y + 2, Anchos[i] - 4, AltoFila - 2), formato);
                    columnaX += Anchos[i];
                }

                g.DrawLine(Pens.LightGray, x, y + AltoFila, x + ancho, y + AltoFila);
            }

            return y + AltoFila;
        }

        private void LiberarFuentes()
        {
            fuenteTitulo.Dispose();
            fuenteSeccion.Dispose();
            fuente.Dispose();
            fuenteNegrita.Dispose();
        }
    }
}
