using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    /// <summary>
    /// Alertas automáticas (dbo.Alerta). Se generan solas en clsOperacionVozNegocio / clsAlertaNegocio:
    /// comandos N3/N4 rechazados, cancelados, vencidos o con error, paradas de equipo y desconexión del Arduino.
    /// Aquí se ven, se reconocen (ACTIVA → RECONOCIDA) y se cierran (RECONOCIDA → CERRADA).
    /// La lista se actualiza sola cada 15 segundos.
    /// </summary>
    public partial class FrmAlertas : Form
    {
        private static readonly Color ColorActiva = Color.Salmon;
        private static readonly Color ColorReconocida = Color.Goldenrod;
        private static readonly Color ColorCerrada = Color.Gray;

        private readonly clsAlertaNegocio alertaNegocio = new clsAlertaNegocio();
        private readonly Timer actualizacion = new Timer { Interval = 15000 };

        public FrmAlertas()
        {
            InitializeComponent();

            lblConexionESP32.Text = clsMonitorArduino.TextoEstado;
            lblDetalleRechazosConfianza.Text = "Hoy";

            // Texto de los botones de la tabla (en el diseñador estaban vacíos).
            colReconocerAlerta.Text = "Reconocer";
            colReconocerAlerta.UseColumnTextForButtonValue = true;
            colCerrarAlerta.Text = "Cerrar";
            colCerrarAlerta.UseColumnTextForButtonValue = true;

            // Los eventos se conectan aquí para no tocar el diseñador.
            Load += (s, e) => { Cargar(); actualizacion.Start(); };
            FormClosed += (s, e) => { actualizacion.Stop(); actualizacion.Dispose(); };
            actualizacion.Tick += (s, e) => Cargar();
            dgvAlertas.CellContentClick += dgvAlertas_CellContentClick;
            dgvAlertas.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) MostrarDetalle(AlertaDeFila(e.RowIndex)); };
        }

        // ---------------- Cargar ----------------

        private void Cargar()
        {
            ResumenAlertas resumen;
            List<Alerta> alertas;

            try
            {
                resumen = alertaNegocio.mtdResumen();
                alertas = alertaNegocio.mtdListar();
            }
            catch (Exception ex)
            {
                actualizacion.Stop();   // para no repetir el mensaje cada 15 segundos
                MessageBox.Show("No se pudieron cargar las alertas.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MostrarResumen(resumen);
            MostrarAlertas(alertas);
        }

        private void MostrarResumen(ResumenAlertas r)
        {
            lblConexionESP32.Text = clsMonitorArduino.TextoEstado;
            lblConexionESP32.ForeColor = clsMonitorArduino.Conectado ? Color.MediumAquamarine : Color.Silver;

            lblCantidadAlertasActivas.Text = r.Activas.ToString();
            lblCantidadAlertasActivas.ForeColor = r.Activas > 0 ? ColorActiva : Color.MediumAquamarine;
            lblDetalleAlertasActivas.Text =
                (r.Activas > 0 ? "Última: " + r.UltimaActiva.Value.ToString("dd/MM HH:mm") + " · " : "Sin alertas activas · ") +
                r.Reconocidas + " reconocida(s) pendiente(s) de cierre";

            lblCantidadRechazosConfianza.Text = r.RechazosConfianzaHoy.ToString();
            lblCantidadRechazosConfianza.ForeColor = r.RechazosConfianzaHoy > 0 ? ColorReconocida : Color.MediumAquamarine;

            lblCantidadFallasEnlace.Text = r.FallasEnlaceHoy.ToString();
            lblCantidadFallasEnlace.ForeColor = r.FallasEnlaceAbiertas > 0 ? ColorActiva : Color.MediumAquamarine;
            lblDetalleFallasEnlace.Text = "Hoy · " + (r.FallasEnlaceAbiertas > 0
                ? r.FallasEnlaceAbiertas + " sin cerrar"
                : "sin fallas abiertas");
        }

        private void MostrarAlertas(List<Alerta> alertas)
        {
            // Se recuerda la fila visible para que la actualización automática no mueva la lista.
            int primeraFila = dgvAlertas.FirstDisplayedScrollingRowIndex;

            dgvAlertas.Rows.Clear();

            foreach (Alerta a in alertas)
            {
                int fila = dgvAlertas.Rows.Add(
                    a.FechaHora.ToString("dd/MM HH:mm:ss"),
                    a.TipoTexto,
                    a.SeveridadTexto,
                    a.Descripcion,
                    a.EstadoTexto);
                // Las columnas RECONOCER y CERRAR muestran su texto solas (UseColumnTextForButtonValue).

                DataGridViewRow row = dgvAlertas.Rows[fila];
                row.Tag = a;
                row.Cells[colNivelAlerta.Index].Style.ForeColor = ColorSeveridad(a.Severidad);
                row.Cells[colEstadoAlerta.Index].Style.ForeColor =
                    a.EstaActiva ? ColorActiva : a.EstaReconocida ? ColorReconocida : ColorCerrada;

                // Botón que no aplica en ese estado: se ve apagado.
                if (!a.EstaActiva) row.Cells[colReconocerAlerta.Index].Style.ForeColor = ColorCerrada;
                if (!a.EstaReconocida) row.Cells[colCerrarAlerta.Index].Style.ForeColor = ColorCerrada;

                if (a.EstaCerrada)
                {
                    row.Cells[colDescripcionAlerta.Index].Style.ForeColor = ColorCerrada;
                    row.Cells[colTipoAlerta.Index].Style.ForeColor = ColorCerrada;
                }
            }

            dgvAlertas.ClearSelection();
            if (primeraFila >= 0 && primeraFila < dgvAlertas.Rows.Count)
            {
                try { dgvAlertas.FirstDisplayedScrollingRowIndex = primeraFila; }
                catch (InvalidOperationException) { /* la tabla aún no tiene tamaño visible */ }
            }

            lblTituloRegistroAlertas.Text = "Registro de alertas (" + alertas.Count + ")";
        }

        // ---------------- Reconocer / Cerrar ----------------

        private void dgvAlertas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Alerta alerta = AlertaDeFila(e.RowIndex);
            if (alerta == null) return;

            if (e.ColumnIndex == colReconocerAlerta.Index)
            {
                Mostrar(alertaNegocio.mtdReconocer(alerta));
            }
            else if (e.ColumnIndex == colCerrarAlerta.Index)
            {
                if (alerta.EstaReconocida &&
                    MessageBox.Show("¿Cerrar la alerta " + alerta.Codigo + "?\nSolo ciérrela si el incidente ya fue atendido.",
                        "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                Mostrar(alertaNegocio.mtdCerrar(alerta));
            }
        }

        private void Mostrar(Resultado resultado)
        {
            if (!resultado.Exito)
                MessageBox.Show(resultado.Mensaje, "Alertas", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            Cargar();   // se actualiza siempre: si otra persona la cambió, se ve el estado real
        }

        private Alerta AlertaDeFila(int fila)
        {
            return fila >= 0 && fila < dgvAlertas.Rows.Count ? dgvAlertas.Rows[fila].Tag as Alerta : null;
        }

        private void MostrarDetalle(Alerta a)
        {
            if (a == null) return;

            MessageBox.Show(
                "Código: " + a.Codigo + "\n" +
                "Tipo: " + a.TipoTexto + "\n" +
                "Severidad: " + a.SeveridadTexto + "\n" +
                "Estado: " + a.EstadoTexto + "\n" +
                "Fecha: " + a.FechaHora.ToString("dd/MM/yyyy HH:mm:ss") + "\n" +
                (a.Equipo == null ? "" : "Equipo: " + a.Equipo + "\n") +
                (a.UsuarioOrigen == null ? "" : "Operador: " + a.UsuarioOrigen + "\n") +
                (a.FechaReconocida.HasValue ? "Reconocida: " + a.FechaReconocida.Value.ToString("dd/MM/yyyy HH:mm") +
                                              " por " + (a.UsuarioReconoce ?? "—") + "\n" : "") +
                (a.FechaCierre.HasValue ? "Cerrada: " + a.FechaCierre.Value.ToString("dd/MM/yyyy HH:mm") +
                                          " por " + (a.UsuarioCierra ?? "—") + "\n" : "") +
                "\n" + a.Descripcion,
                "Detalle de la alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static Color ColorSeveridad(string severidad)
        {
            switch (severidad)
            {
                case clsAlertaNegocio.SeveridadCritica: return Color.Salmon;
                case clsAlertaNegocio.SeveridadAlta: return Color.Goldenrod;
                case clsAlertaNegocio.SeveridadMedia: return Color.LightSkyBlue;
                default: return Color.Silver;
            }
        }
    }
}
