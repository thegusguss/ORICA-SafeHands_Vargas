using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    /// <summary>
    /// Estado técnico: enlace con el Arduino (heartbeat, watchdog, rearme), micrófono, voz, bases de datos y parada.
    /// Abajo se ve el monitor serial (lo que se envía y recibe del Arduino). Se actualiza cada medio segundo.
    /// </summary>
    public partial class FrmDiagnostico : Form
    {
        private static readonly Color ColorOk = Color.MediumAquamarine;
        private static readonly Color ColorAviso = Color.Goldenrod;
        private static readonly Color ColorError = Color.Salmon;
        private static readonly Color ColorApagado = Color.Silver;

        private readonly clsOperacionVozNegocio voz = new clsOperacionVozNegocio();

        // Monitor serial y opciones configurados en tiempo de ejecución.
        private readonly Panel pnlMonitor = new Panel();
        private readonly ListBox lstMonitor = new ListBox();
        private readonly CheckBox chkSimular = new CheckBox();
        private readonly Button btnProbarEstado = new Button();

        private string ultimaLineaLog = "";
        private string resultadoMicrofono;   // null = no se probó todavía
        private int vueltas;

        public FrmDiagnostico()
        {
            InitializeComponent();

            lblTituloDiagESP32.Text = "ARDUINO UNO";
            lblTituloDiagSQLite.Text = "COPIA LOCAL";
            lblTituloDiagSQLServer.Text = "AZURE SQL";
            lblTituloDiagParada.Text = "PARADA";
            lblSubtituloDiagnostico.Text = "Enlace con el Arduino y estado de la aplicación";
            btnSimularPerdidaHeartbeat.Text = "Perder heartbeat";

            CrearMonitor();
        }

        private void FrmDiagnostico_Load(object sender, EventArgs e)
        {
            Actualizar();
            actualizacion.Start();
        }

        private void FrmDiagnostico_Disposed(object sender, EventArgs e)
        {
            actualizacion.Stop();
            actualizacion.Dispose();
        }

        private void actualizacion_Tick(object sender, EventArgs e)
        {
            if (!IsDisposed) Actualizar();
        }

        private async void btnProbarMicrofono_Click(object sender, EventArgs e)
        {
            await ProbarMicrofono();
        }

        private void btnSimularPerdidaHeartbeat_Click(object sender, EventArgs e)
        {
            PerderHeartbeat();
        }

        private void btnRecuperarEnlace_Click(object sender, EventArgs e)
        {
            clsEnlaceArduino.mtdReconectar();
            Actualizar();
        }

        private void btnRearmarSistema_Click(object sender, EventArgs e)
        {
            Rearmar();
        }

        private void btnParadaSimulada_Click(object sender, EventArgs e)
        {
            ParadaGeneral();
        }

        private void btnProbarEstado_Click(object sender, EventArgs e)
        {
            ProbarEstado();
        }

        private void chkSimular_Click(object sender, EventArgs e)
        {
            GuardarSimulacion();
        }

        // Panel debajo del diagnóstico integral con el monitor serial.
        private void CrearMonitor()
        {
            pnlMonitor.Location = new Point(pnlDiagnosticoIntegral.Left, pnlDiagnosticoIntegral.Bottom + 16);
            pnlMonitor.Size = new Size(pnlDiagnosticoIntegral.Width, Math.Max(150, ClientSize.Height - pnlDiagnosticoIntegral.Bottom - 32));
            pnlMonitor.BackColor = pnlDiagnosticoIntegral.BackColor;
            pnlMonitor.BorderStyle = BorderStyle.FixedSingle;
            pnlMonitor.Anchor = pnlDiagnosticoIntegral.Anchor;

            Label titulo = new Label
            {
                Text = "Monitor serial  (→ enviado · ← recibido · ! problema)",
                Font = lblTituloDiagnosticoIntegral.Font,
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(19, 12)
            };

            chkSimular.Text = "Simular comandos si no hay Arduino";
            chkSimular.ForeColor = Color.Silver;
            chkSimular.AutoSize = true;
            chkSimular.Location = new Point(pnlMonitor.Width - 470, 14);
            chkSimular.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkSimular.Checked = new clsConfiguracionNegocio().mtdObtenerBooleano(clsConfiguracionNegocio.SimularSinArduino);

            btnProbarEstado.Text = "Pedir ESTADO";
            btnProbarEstado.Size = new Size(155, 30);
            btnProbarEstado.Location = new Point(pnlMonitor.Width - 175, 8);
            btnProbarEstado.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnProbarEstado.BackColor = btnRecuperarEnlace.BackColor;
            btnProbarEstado.ForeColor = Color.White;
            btnProbarEstado.FlatStyle = FlatStyle.Flat;
            btnProbarEstado.Cursor = Cursors.Hand;

            lstMonitor.Location = new Point(19, 46);
            lstMonitor.Size = new Size(pnlMonitor.Width - 40, pnlMonitor.Height - 60);
            lstMonitor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstMonitor.BackColor = Color.FromArgb(13, 17, 23);
            lstMonitor.ForeColor = Color.Gainsboro;
            lstMonitor.BorderStyle = BorderStyle.None;
            lstMonitor.Font = new Font("Consolas", 9F);
            lstMonitor.IntegralHeight = false;

            pnlMonitor.Controls.AddRange(new Control[] { titulo, chkSimular, btnProbarEstado, lstMonitor });
            Controls.Add(pnlMonitor);

            btnProbarEstado.Click += new System.EventHandler(btnProbarEstado_Click);
            chkSimular.Click += new System.EventHandler(chkSimular_Click);
        }

        // ---------------- Mostrar ----------------

        private void Actualizar()
        {
            MostrarArduino();
            MostrarMonitor();

            // Lo que consulta la BD se actualiza cada 5 s, no cada medio segundo.
            if (vueltas++ % 10 == 0) MostrarAplicacion();
        }

        private void MostrarArduino()
        {
            clsEnlaceArduino.EstadoEnlace estado = clsEnlaceArduino.Estado;

            // Cabecera
            lblConexionESP32.Text = clsEnlaceArduino.TextoEstado;
            lblConexionESP32.ForeColor = clsEnlaceArduino.Conectado ? (clsEnlaceArduino.RequiereRearme ? ColorAviso : ColorOk) : ColorApagado;
            lblDiagEstadoApp.Text = "● App en línea";
            lblDiagEstadoApp.ForeColor = ColorOk;

            // Arduino
            switch (estado)
            {
                case clsEnlaceArduino.EstadoEnlace.Conectado: Pintar(lblEstadoDiagESP32, "Conectado", ColorOk); break;
                case clsEnlaceArduino.EstadoEnlace.Conectando: Pintar(lblEstadoDiagESP32, "Conectando...", ColorAviso); break;
                case clsEnlaceArduino.EstadoEnlace.SinHeartbeat: Pintar(lblEstadoDiagESP32, "Sin respuesta", ColorError); break;
                case clsEnlaceArduino.EstadoEnlace.SinConfigurar: Pintar(lblEstadoDiagESP32, "Sin puerto", ColorApagado); break;
                default: Pintar(lblEstadoDiagESP32, "Desconectado", ColorError); break;
            }
            lblDetalleDiagESP32.Text = (clsEnlaceArduino.Puerto ?? "—") + " · " + clsEnlaceArduino.Baudios + " baud" +
                (clsEnlaceArduino.Conectado ? " · " + clsEnlaceArduino.LatenciaMs + " ms" : "");

            // Heartbeat
            Pintar(lblValorDiagHeartbeat, clsEnlaceArduino.Perdidos + " / " + clsEnlaceArduino.MaxPerdidos,
                clsEnlaceArduino.Perdidos == 0 ? ColorOk : clsEnlaceArduino.Perdidos < clsEnlaceArduino.MaxPerdidos ? ColorAviso : ColorError);
            lblDetalleDiagHeartbeat.Text = clsEnlaceArduino.HeartbeatPausado
                ? "Detenido para la prueba" : "Perdidos seguidos · cada " + clsEnlaceArduino.HeartbeatMs + " ms";
            btnSimularPerdidaHeartbeat.Text = clsEnlaceArduino.HeartbeatPausado ? "Reanudar heartbeat" : "Perder heartbeat";

            // Watchdog y rearme (lo que informa el Arduino)
            string modo = clsEnlaceArduino.Modo;
            if (!clsEnlaceArduino.Conectado && estado != clsEnlaceArduino.EstadoEnlace.SinHeartbeat)
                Pintar(lblEstadoDiagWatchdog, "Sin enlace", ColorApagado);
            else if (modo == "WATCHDOG")
                Pintar(lblEstadoDiagWatchdog, "Disparado", ColorError);
            else
                Pintar(lblEstadoDiagWatchdog, "Vigilando", ColorOk);
            lblDetalleDiagWatchdog.Text = "Estado seguro si no hay enlace";

            if (clsEnlaceArduino.RequiereRearme) Pintar(lblEstadoDiagRearme, "Requerido", ColorAviso);
            else if (clsEnlaceArduino.Conectado) Pintar(lblEstadoDiagRearme, "No requerido", ColorOk);
            else Pintar(lblEstadoDiagRearme, "Sin enlace", ColorApagado);
            lblDetalleDiagRearme.Text = string.IsNullOrEmpty(modo) ? "Recuperación explícita" : "Modo del Arduino: " + modo;
            btnRearmarSistema.Enabled = clsEnlaceArduino.Conectado;

            // Parada
            if (modo == "PARADA") Pintar(lblEstadoDiagParada, "Activa", ColorError);
            else if (clsEnlaceArduino.Conectado) Pintar(lblEstadoDiagParada, "Lista", ColorOk);
            else Pintar(lblEstadoDiagParada, "Sin Arduino", ColorApagado);
            lblDetalleDiagParada.Text = "Botón en pantalla y pulsador físico";
            btnProbarEstado.Enabled = clsEnlaceArduino.Conectado;
        }

        private void MostrarAplicacion()
        {
            // Micrófono y voz
            string idioma = clsReconocedorVoz.BuscarIdiomaEspanol();
            if (idioma == null) Pintar(lblEstadoDiagSTT, "Sin español", ColorError);
            else Pintar(lblEstadoDiagSTT, "Listo", ColorOk);
            lblDetalleDiagSTT.Text = idioma == null ? "Instalar reconocimiento de voz" : "Reconocedor " + idioma + " · sin internet";

            if (resultadoMicrofono == null)
            {
                Pintar(lblEstadoDiagMicrofono, "Sin probar", ColorApagado);
                lblDetalleDiagMicrofono.Text = "Pulse \"Probar micrófono\"";
            }

            // Bases de datos
            Pintar(lblEstadoDiagSQLServer, clsSincronizacionNegocio.EnLinea ? "Conectado" : "Sin conexión",
                   clsSincronizacionNegocio.EnLinea ? ColorOk : ColorAviso);
            lblDetalleDiagSQLServer.Text = clsSincronizacionNegocio.EnLinea ? "Base principal" : "Se trabaja con la copia local";

            try
            {
                EstadoSincronizacion s = clsSincronizacionNegocio.mtdEstado();
                if (!s.Ocupado)
                {
                    Pintar(lblEstadoDiagSQLite, s.FechaCopia.HasValue ? "Lista" : "Sin copia", s.FechaCopia.HasValue ? ColorOk : ColorAviso);
                    lblDetalleDiagSQLite.Text = s.Pendientes + " pendiente(s) por subir";
                }
            }
            catch (Exception)
            {
                Pintar(lblEstadoDiagSQLite, "Error", ColorError);
            }

            // PTT
            bool ptt = voz.mtdPttObligatorio();
            Pintar(lblEstadoDiagPTT, ptt ? "Obligatorio" : "Opcional", ColorOk);
            lblDetalleDiagPTT.Text = "Mantener pulsado para hablar";

            // Resumen
            List<string> problemas = new List<string>();
            if (!clsEnlaceArduino.Conectado) problemas.Add("Arduino no conectado");
            if (clsEnlaceArduino.RequiereRearme) problemas.Add("equipo requiere rearme");
            if (idioma == null) problemas.Add("falta reconocimiento de voz en español");
            if (!clsSincronizacionNegocio.EnLinea) problemas.Add("sin conexión con Azure");
            lblResumenDiagnostico.Text = problemas.Count == 0 ? "Todo en orden." : "Revisar: " + string.Join(", ", problemas) + ".";
            lblResumenDiagnostico.ForeColor = problemas.Count == 0 ? ColorOk : ColorAviso;
        }

        // Agrega al monitor solo las líneas nuevas.
        private void MostrarMonitor()
        {
            string[] log = clsEnlaceArduino.mtdLog();
            if (log.Length == 0 || log[log.Length - 1] == ultimaLineaLog) return;

            int desde = Array.LastIndexOf(log, ultimaLineaLog) + 1;   // 0 si la última que se mostró ya salió del log
            lstMonitor.BeginUpdate();
            foreach (string linea in log.Skip(desde)) lstMonitor.Items.Add(linea);
            while (lstMonitor.Items.Count > 300) lstMonitor.Items.RemoveAt(0);
            lstMonitor.TopIndex = Math.Max(0, lstMonitor.Items.Count - 1);
            lstMonitor.EndUpdate();
            ultimaLineaLog = log[log.Length - 1];
        }

        private static void Pintar(Label etiqueta, string texto, Color color)
        {
            etiqueta.Text = texto;
            etiqueta.ForeColor = color;
        }

        // ---------------- Botones ----------------

        // Prepara el reconocedor y abre el micrófono un momento.
        private async System.Threading.Tasks.Task ProbarMicrofono()
        {
            btnProbarMicrofono.Enabled = false;
            Pintar(lblEstadoDiagMicrofono, "Probando...", ColorAviso);

            using (clsReconocedorVoz reconocedor = new clsReconocedorVoz())
            {
                string error = await reconocedor.PrepararAsync(new[] { "PRUEBA DE MICROFONO" });
                if (error == null) error = await reconocedor.IniciarAsync();
                if (error == null) await reconocedor.DetenerAsync();
                resultadoMicrofono = error ?? "";
            }

            if (IsDisposed) return;
            btnProbarMicrofono.Enabled = true;
            if (resultadoMicrofono == "")
            {
                Pintar(lblEstadoDiagMicrofono, "Funciona", ColorOk);
                lblDetalleDiagMicrofono.Text = "Se pudo abrir el micrófono";
            }
            else
            {
                Pintar(lblEstadoDiagMicrofono, "Con problema", ColorError);
                lblDetalleDiagMicrofono.Text = resultadoMicrofono.Length > 45 ? resultadoMicrofono.Substring(0, 42) + "..." : resultadoMicrofono;
                MessageBox.Show(resultadoMicrofono, "Micrófono", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Prueba de seguridad: la PC deja de enviar heartbeat → el Arduino debe apagar todo (watchdog).
        private void PerderHeartbeat()
        {
            if (clsEnlaceArduino.HeartbeatPausado)
            {
                clsEnlaceArduino.mtdPausarHeartbeat(false);
                return;
            }

            if (MessageBox.Show("La PC dejará de enviar heartbeat al Arduino.\n" +
                                "En unos instantes el watchdog del Arduino debe apagar todas las salidas y quedar esperando REARME.\n¿Continuar?",
                    "Prueba de watchdog", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                clsEnlaceArduino.mtdPausarHeartbeat(true);
        }

        private void Rearmar()
        {
            if (MessageBox.Show("¿Rearmar el equipo? Volverá a aceptar órdenes.\nVerifique antes que sea seguro operar.",
                    "Rearmar sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            Cursor = Cursors.WaitCursor;
            Resultado r = voz.mtdRearmar();
            Cursor = Cursors.Default;
            MessageBox.Show(r.Mensaje, "Rearmar sistema", MessageBoxButtons.OK, r.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        private void ProbarEstado()
        {
            Cursor = Cursors.WaitCursor;
            RespuestaArduino r = clsEnlaceArduino.mtdEnviarOrden("ESTADO", "*");
            Cursor = Cursors.Default;

            string texto = !r.Exito ? "Sin respuesta: " + r.MotivoTexto
                : "Modo: " + r.Modo + "\nIndicador: " + (r.Indicador ? "encendido" : "apagado") +
                  "\nSimulador: " + (r.Simulador ? "en marcha" : "detenido") +
                  "\nOperación crítica: " + (r.Critica ? "activa" : "apagada") +
                  "\nRespuesta en " + r.LatenciaMs + " ms";
            MessageBox.Show(texto, "Estado del Arduino", MessageBoxButtons.OK, r.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        private void ParadaGeneral()
        {
            if (MessageBox.Show("¿Enviar PARADA a todos los equipos activos?", "Parada",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            Resultado r = new clsDashboardNegocio().mtdParadaGeneral();
            MessageBox.Show(r.Mensaje, "Parada", MessageBoxButtons.OK, r.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        // Guarda SIMULAR_SIN_ARDUINO en Configuración.
        private void GuardarSimulacion()
        {
            Resultado r = new clsConfiguracionNegocio().mtdGuardar(new Dictionary<string, string>
            {
                { clsConfiguracionNegocio.SimularSinArduino, chkSimular.Checked ? "true" : "false" }
            });

            if (!r.Exito)
            {
                chkSimular.Checked = !chkSimular.Checked;
                MessageBox.Show(r.Mensaje, "Configuración", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
