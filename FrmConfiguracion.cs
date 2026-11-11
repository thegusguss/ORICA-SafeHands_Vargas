using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Windows.Forms;
using C = ORICA.Negocio.clsConfiguracionNegocio;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    public partial class FrmConfiguracion : Form
    {
        private readonly clsConfiguracionNegocio configuracionNegocio =
            new clsConfiguracionNegocio();

        public FrmConfiguracion()
        {
            InitializeComponent();

            trkUmbralConfianza.Minimum = C.MinUmbral;
            trkUmbralConfianza.Maximum = C.MaxUmbral;

            nudTimeoutConfirmacion.Minimum = C.MinTimeout;
            nudTimeoutConfirmacion.Maximum = C.MaxTimeout;

            nudIntervaloHeartbeat.Minimum = C.MinHeartbeat;
            nudIntervaloHeartbeat.Maximum = C.MaxHeartbeat;

            nudMaxHeartbeatPerdidos.Minimum = C.MinHeartbeatPerdidos;
            nudMaxHeartbeatPerdidos.Maximum = C.MaxHeartbeatPerdidosPermitidos;

            // Restricciones de seguridad obligatorias.
            chkParadaPrioritaria.Checked = true;
            chkEstadoSeguroError.Checked = true;

            chkParadaPrioritaria.Enabled = false;
            chkEstadoSeguroError.Enabled = false;

            // Textos de los botones.
            btnSimularEnlace.Text = "Buscar puertos";
            btnSimularRedSQLConfig.Text = "Probar conexión SQL Server";

            MostrarUmbral();
        }

        // EVENTO LOAD 

        private void FrmConfiguracion_Load(object sender, EventArgs e)
        {
            CargarValores();
        }

        // EVENTO VALUECHANGED 

        private void trkUmbralConfianza_ValueChanged(object sender, EventArgs e)
        {
            MostrarUmbral();
        }

        private void MostrarUmbral()
        {
            lblUmbralConfianza.Text =
                "Umbral de confianza: " + trkUmbralConfianza.Value + "%";
        }

        // CARGAR CONFIGURACIÓN 

        private void CargarValores()
        {
            Dictionary<string, ParametroConfiguracion> p;

            try
            {
                p = configuracionNegocio.mtdListar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo cargar la configuración.\n" + ex.Message,
                    "Error de conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // Configuración de voz.
            trkUmbralConfianza.Value = Limitar(
                Entero(p, C.UmbralConfianza),
                trkUmbralConfianza.Minimum,
                trkUmbralConfianza.Maximum);

            nudTimeoutConfirmacion.Value = Limitar(
                Entero(p, C.TimeoutConfirmacion),
                (int)nudTimeoutConfirmacion.Minimum,
                (int)nudTimeoutConfirmacion.Maximum);

            chkGramaticaCerrada.Checked =
                Booleano(p, C.GramaticaCerrada);

            chkPTTObligatorio.Checked =
                Booleano(p, C.PttObligatorio);

            chkFeedbackVoz.Checked =
                Booleano(p, C.FeedbackVoz);

            // Configuración de comunicación ESP32.
            CargarPuertos(Texto(p, C.PuertoCom));

            string baud = Texto(p, C.BaudRate);

            cbxBaudRate.SelectedItem =
                cbxBaudRate.Items.Contains(baud) ? baud : "115200";

            nudIntervaloHeartbeat.Value = Limitar(
                Entero(p, C.HeartbeatMs),
                (int)nudIntervaloHeartbeat.Minimum,
                (int)nudIntervaloHeartbeat.Maximum);

            nudMaxHeartbeatPerdidos.Value = Limitar(
                Entero(p, C.MaxHeartbeatPerdidos),
                (int)nudMaxHeartbeatPerdidos.Minimum,
                (int)nudMaxHeartbeatPerdidos.Maximum);

            chkCRCObligatorio.Checked =
                Booleano(p, C.CrcObligatorio);

            chkValidarSecuencia.Checked =
                Booleano(p, C.SecuenciaObligatoria);

            // Configuración de persistencia.
            chkModoOfflineFirst.Checked =
                Booleano(p, C.ModoOffline);

            chkSincronizarSQLServer.Checked =
                Booleano(p, C.SincronizacionAutomatica);

            // Configuración de seguridad.
            chkWatchdogESP32.Checked =
                Booleano(p, C.WatchdogActivo);

            chkRearmeExplicito.Checked =
                Booleano(p, C.RearmeRequerido);

            chkEstadoSeguroError.Checked = true;
            chkParadaPrioritaria.Checked = true;

            MostrarUmbral();
        }

        // ================= BUSCAR PUERTOS COM =================

        private void CargarPuertos(string seleccionado)
        {
            cbxPuertoESP32.Items.Clear();

            try
            {
                foreach (string puerto in SerialPort.GetPortNames())
                {
                    if (!cbxPuertoESP32.Items.Contains(puerto))
                    {
                        cbxPuertoESP32.Items.Add(puerto);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron buscar los puertos COM.\n" + ex.Message,
                    "Comunicación serial",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            // Conserva el puerto registrado en la configuración.
            if (!string.IsNullOrWhiteSpace(seleccionado) &&
                !cbxPuertoESP32.Items.Contains(seleccionado))
            {
                cbxPuertoESP32.Items.Add(seleccionado);
            }

            // Puerto predeterminado.
            if (cbxPuertoESP32.Items.Count == 0)
            {
                cbxPuertoESP32.Items.Add("COM3");
            }

            cbxPuertoESP32.SelectedItem =
                !string.IsNullOrWhiteSpace(seleccionado)
                ? seleccionado
                : cbxPuertoESP32.Items[0];
        }

        // ================= EVENTO BUSCAR PUERTOS =================

        private void btnSimularEnlace_Click(object sender, EventArgs e)
        {
            CargarPuertos(cbxPuertoESP32.SelectedItem as string);
        }

        // ================= EVENTO GUARDAR =================

        private void btnGuardarConfiguracion_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> valores =
                new Dictionary<string, string>
            {
                {
                    C.UmbralConfianza,
                    C.mtdATexto((decimal)trkUmbralConfianza.Value)
                },
                {
                    C.TimeoutConfirmacion,
                    C.mtdATexto((int)nudTimeoutConfirmacion.Value)
                },
                {
                    C.GramaticaCerrada,
                    C.mtdATexto(chkGramaticaCerrada.Checked)
                },
                {
                    C.PttObligatorio,
                    C.mtdATexto(chkPTTObligatorio.Checked)
                },
                {
                    C.FeedbackVoz,
                    C.mtdATexto(chkFeedbackVoz.Checked)
                },
                {
                    C.PuertoCom,
                    cbxPuertoESP32.SelectedItem as string ?? string.Empty
                },
                {
                    C.BaudRate,
                    cbxBaudRate.SelectedItem as string ?? string.Empty
                },
                {
                    C.HeartbeatMs,
                    C.mtdATexto((int)nudIntervaloHeartbeat.Value)
                },
                {
                    C.MaxHeartbeatPerdidos,
                    C.mtdATexto((int)nudMaxHeartbeatPerdidos.Value)
                },
                {
                    C.CrcObligatorio,
                    C.mtdATexto(chkCRCObligatorio.Checked)
                },
                {
                    C.SecuenciaObligatoria,
                    C.mtdATexto(chkValidarSecuencia.Checked)
                },
                {
                    C.ModoOffline,
                    C.mtdATexto(chkModoOfflineFirst.Checked)
                },
                {
                    C.SincronizacionAutomatica,
                    C.mtdATexto(chkSincronizarSQLServer.Checked)
                },
                {
                    C.WatchdogActivo,
                    C.mtdATexto(chkWatchdogESP32.Checked)
                },
                {
                    C.RearmeRequerido,
                    C.mtdATexto(chkRearmeExplicito.Checked)
                },
                {
                    C.EstadoSeguroError,
                    C.mtdATexto(true)
                },
                {
                    C.ParadaPrioritaria,
                    C.mtdATexto(true)
                }
            };

            Resultado resultado = configuracionNegocio.mtdGuardar(valores);

            MessageBox.Show(
                resultado.Mensaje,
                "Configuración",
                MessageBoxButtons.OK,
                resultado.Exito
                    ? MessageBoxIcon.Information
                    : MessageBoxIcon.Warning);

            if (resultado.Exito)
            {
                CargarValores();
            }
        }

        //EVENTO PROBAR SQL SERVER

        private void btnSimularRedSQLConfig_Click(object sender, EventArgs e)
        {
            Resultado resultado = clsSistemaNegocio.mtdProbarConexion();

            MessageBox.Show(
                resultado.Mensaje,
                "SQL Server",
                MessageBoxButtons.OK,
                resultado.Exito
                    ? MessageBoxIcon.Information
                    : MessageBoxIcon.Warning);
        }

        //MÉTODOS AUXILIARES 

        private static string Texto(
            Dictionary<string, ParametroConfiguracion> p,
            string clave)
        {
            ParametroConfiguracion valor;

            return p.TryGetValue(clave, out valor) && valor.Valor != null
                ? valor.Valor.Trim()
                : string.Empty;
        }

        private static int Entero(
            Dictionary<string, ParametroConfiguracion> p,
            string clave)
        {
            decimal valor;

            return decimal.TryParse(
                Texto(p, clave),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out valor)
                ? (int)Math.Round(valor)
                : 0;
        }

        private static bool Booleano(
            Dictionary<string, ParametroConfiguracion> p,
            string clave)
        {
            return string.Equals(
                Texto(p, clave),
                "true",
                StringComparison.OrdinalIgnoreCase);
        }

        private static int Limitar(int valor, int min, int max)
        {
            return Math.Max(min, Math.Min(max, valor));
        }
    }
}
