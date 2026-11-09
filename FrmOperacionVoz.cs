using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    /// <summary>
    /// Control por voz del operador.
    /// 1) Elige el equipo → se arma la gramática cerrada con sus comandos.
    /// 2) Mantiene pulsado el botón (PTT) y habla → se interpreta la frase y la confianza.
    /// 3) Si el nivel de criticidad lo pide, confirma (1 o 2 veces) por voz o con el botón, antes del timeout.
    /// Las reglas están en clsOperacionVozNegocio; aquí solo va el micrófono y la pantalla.
    /// El reconocimiento usa el motor moderno de Windows (clsReconocedorVoz); System.Speech solo se usa para hablar.
    /// Clic derecho en el botón = escribir la frase (sirve para probar sin micrófono).
    /// </summary>
    public partial class FrmOperacionVoz : Form
    {
        private static readonly Color ColorOk = Color.MediumAquamarine;
        private static readonly Color ColorError = Color.Salmon;
        private static readonly Color ColorAviso = Color.Goldenrod;

        private readonly clsOperacionVozNegocio voz = new clsOperacionVozNegocio();
        private readonly Timer temporizador = new Timer { Interval = 250 };

        private clsReconocedorVoz reconocedor;         // null = Windows no tiene voz en español
        private bool gramaticaLista;                   // la gramática del equipo ya se cargó
        private int versionGramatica;                  // si se piden varias seguidas, vale solo la última
        private bool cargandoEquipos;                  // mientras se llena el combo no se reacciona a sus cambios
        private SpeechSynthesizer sintetizador;        // null = sin respuesta hablada

        private List<Comando> comandos = new List<Comando>();
        private Equipo equipoActual;
        private OperacionPendiente pendiente;          // comando esperando confirmación
        private bool escuchando;
        private bool pttObligatorio = true;
        private decimal umbralGlobal = 85m;

        private readonly Color colorPtt;
        private readonly string textoPtt;

        public FrmOperacionVoz()
        {
            InitializeComponent();

            // Recorta el botón de "pulsar para hablar" para que se vea redondo.
            using (GraphicsPath circulo = new GraphicsPath())
            {
                circulo.AddEllipse(0, 0, btnPTTVoz.Width, btnPTTVoz.Height);
                btnPTTVoz.Region = new Region(circulo);
            }

            colorPtt = btnPTTVoz.BackColor;
            textoPtt = btnPTTVoz.Text;

            // Los eventos se conectan aquí para no tocar el diseñador.
            Load += FrmOperacionVoz_Load;
            // Se libera en los dos casos: al cambiar de pantalla (FormClosed) y al cerrar la ventana
            // principal con esta pantalla abierta (ahí solo llega Disposed). Liberar() se puede llamar dos veces.
            FormClosed += (s, e) => Liberar();
            Disposed += (s, e) => Liberar();
            cbxEquipoVoz.SelectedIndexChanged += (s, e) =>
            {
                if (!cargandoEquipos) SeleccionarEquipo(cbxEquipoVoz.SelectedItem as Equipo);
            };
            btnPTTVoz.MouseDown += btnPTTVoz_MouseDown;
            btnPTTVoz.MouseUp += btnPTTVoz_MouseUp;
            btnConfirmarComando.Click += (s, e) => Responder(clsOperacionVozNegocio.PalabraConfirmar);
            btnCancelarComando.Click += (s, e) => Responder(clsOperacionVozNegocio.PalabraCancelar);
            btnDetenerEquipo.Click += btnDetenerEquipo_Click;
            temporizador.Tick += temporizador_Tick;
        }

        // ======================= INICIO / CIERRE =======================

        private void FrmOperacionVoz_Load(object sender, EventArgs e)
        {
            try
            {
                pttObligatorio = voz.mtdPttObligatorio();
                umbralGlobal = voz.mtdUmbralGlobal();
            }
            catch (Exception) { /* sin BD se usan los valores por defecto */ }

            IniciarReconocedor();
            IniciarSintetizador();
            CargarEquipos();
            MostrarSinPendiente();
        }

        // Detiene el temporizador, cancela lo pendiente y suelta el micrófono y la voz.
        private void Liberar()
        {
            temporizador.Stop();
            temporizador.Dispose();

            if (pendiente != null)
            {
                voz.mtdCancelar(pendiente, "Se cerró la pantalla de control por voz.");
                pendiente = null;
            }

            if (reconocedor != null)
            {
                reconocedor.Dispose();
                reconocedor = null;
            }

            if (sintetizador != null)
            {
                try { sintetizador.SpeakAsyncCancelAll(); } catch (Exception) { }
                sintetizador.Dispose();
                sintetizador = null;
            }
        }

        // Busca el reconocimiento de voz de Windows en español (es-MX, es-ES...).
        private void IniciarReconocedor()
        {
            string idioma = clsReconocedorVoz.BuscarIdiomaEspanol();

            if (idioma == null)
            {
                lblModoOperacion.Text = "Sin voz en español";
                MostrarUltimaAccion("Windows no tiene reconocimiento de voz en español. Instala Español (México) con \"Reconocimiento de voz\".", ColorError);
                return;
            }

            reconocedor = new clsReconocedorVoz();
            reconocedor.FraseReconocida += (texto, confianza) => EnPantalla(() => FraseEscuchada(texto, confianza));
            reconocedor.EscuchaTerminada += mensaje => EnPantalla(() =>
            {
                MarcarEscuchando(false);
                if (mensaje != null) MostrarUltimaAccion(mensaje, ColorAviso);
            });

            lblModoOperacion.Text = "Micrófono · " + idioma;
        }

        private void IniciarSintetizador()
        {
            try
            {
                if (!voz.mtdFeedbackVoz()) return;

                sintetizador = new SpeechSynthesizer();
                sintetizador.SetOutputToDefaultAudioDevice();

                InstalledVoice espanol = sintetizador.GetInstalledVoices()
                    .FirstOrDefault(v => v.Enabled && v.VoiceInfo.Culture.TwoLetterISOLanguageName == "es");
                if (espanol != null) sintetizador.SelectVoice(espanol.VoiceInfo.Name);
            }
            catch (Exception)
            {
                if (sintetizador != null) sintetizador.Dispose();
                sintetizador = null;
            }
        }

        // ======================= 1) EQUIPO =======================

        private void CargarEquipos()
        {
            List<Equipo> equipos = new List<Equipo>();
            cargandoEquipos = true;
            try
            {
                equipos = voz.mtdListarEquipos();
                cbxEquipoVoz.DataSource = equipos;
                cbxEquipoVoz.DisplayMember = "Etiqueta";
                cbxEquipoVoz.SelectedIndex = equipos.Count == 1 ? 0 : -1;   // si solo hay uno, se elige solo
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los equipos.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cargandoEquipos = false;
            }

            // Una sola vez, ya con el combo listo.
            SeleccionarEquipo(cbxEquipoVoz.SelectedItem as Equipo);
            if (equipos.Count == 0) lblConexionEquipoVoz.Text = "No hay equipos activos";
        }

        private void SeleccionarEquipo(Equipo equipo)
        {
            if (pendiente != null)
            {
                voz.mtdCancelar(pendiente, "Se cambió de equipo antes de confirmar.");
                pendiente = null;
                MostrarSinPendiente();
            }

            DetenerEscucha();
            equipoActual = equipo;
            comandos = new List<Comando>();

            if (equipo == null)
            {
                lblEstadoEquipoVoz.Text = "Sin seleccionar";
                lblConexionEquipoVoz.Text = "Selecciona un equipo";
                lblEquipoConfirmacion.Text = "Equipo: —";
                MostrarEjemplos();
                return;
            }

            try
            {
                comandos = voz.mtdListarComandos(equipo.IdEquipo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los comandos del equipo.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            lblEstadoEquipoVoz.Text = equipo.EstadoTexto;
            lblConexionEquipoVoz.Text = comandos.Count + " comando(s) · " + clsMonitorArduino.TextoEstado.TrimStart('●', ' ');
            lblEquipoConfirmacion.Text = "Equipo: " + equipo.Etiqueta;

            CargarGramatica();
            MostrarEjemplos();
        }

        // Gramática cerrada: solo las frases de este equipo + CONFIRMAR y CANCELAR.
        private async void CargarGramatica()
        {
            if (reconocedor == null) return;

            int miVersion = ++versionGramatica;
            gramaticaLista = false;
            btnPTTVoz.Enabled = false;

            List<string> frases = comandos.Select(c => c.Frase.ToLowerInvariant())
                .Concat(new[] { "confirmar", "cancelar" })
                .ToList();

            string error = await reconocedor.PrepararAsync(frases);

            // Si mientras tanto se eligió otro equipo, este resultado ya no sirve.
            if (IsDisposed || reconocedor == null || miVersion != versionGramatica) return;
            btnPTTVoz.Enabled = true;
            gramaticaLista = error == null;

            if (error != null)
            {
                lblModoOperacion.Text = "Voz no disponible";
                MostrarUltimaAccion(error, ColorError);
            }
            else
            {
                lblModoOperacion.Text = "Micrófono · " + reconocedor.Idioma;
            }
        }

        private void MostrarEjemplos()
        {
            Label[] etiquetas = { lblEjemploComandoVoz1, lblEjemploComandoVoz };

            for (int i = 0; i < etiquetas.Length; i++)
            {
                etiquetas[i].Text = i < comandos.Count
                    ? "“" + FormatoOracion(comandos[i].Frase) + "”"
                    : "";
            }

            lblTituloEjemplosVoz.Text = comandos.Count == 0 ? "Sin comandos para este equipo" : "Di, por ejemplo:";
        }

        // "ESTADO DEL EQUIPO" → "Estado del equipo"
        private static string FormatoOracion(string frase)
        {
            if (string.IsNullOrEmpty(frase)) return frase;
            string minusculas = frase.ToLowerInvariant();
            return char.ToUpperInvariant(minusculas[0]) + minusculas.Substring(1);
        }

        private void lnkComandosDisponibles_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (FrmComandosDisponibles ventana = new FrmComandosDisponibles(equipoActual))
                ventana.ShowDialog(this);
        }

        // ======================= 2) HABLAR (PTT) =======================

        private async void btnPTTVoz_MouseDown(object sender, MouseEventArgs e)
        {
            if (equipoActual == null)
            {
                MostrarUltimaAccion("Primero elige un equipo.", ColorAviso);
                return;
            }

            if (e.Button == MouseButtons.Left && reconocedor == null)
            {
                MessageBox.Show(
                    "El control por voz necesita el reconocimiento de voz de Windows en español.\n\n" +
                    "1. Configuración → Hora e idioma → Idioma y región.\n" +
                    "2. Agrega \"Español (México)\" o \"Español (España)\".\n" +
                    "3. En sus opciones, instala \"Reconocimiento de voz básico\".\n" +
                    "4. Comprueba que el micrófono esté conectado y vuelve a abrir esta pantalla.",
                    "Reconocimiento de voz no disponible", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Clic derecho: escribir la frase (solo para pruebas).
            if (e.Button == MouseButtons.Right)
            {
                string texto = PedirTexto();
                if (!string.IsNullOrWhiteSpace(texto)) ProcesarFrase(texto, 100m, true);
                return;
            }

            if (e.Button != MouseButtons.Left) return;

            // Con PTT obligatorio se escucha solo mientras se mantiene pulsado; si no, cada clic enciende/apaga.
            if (!pttObligatorio && escuchando)
            {
                DetenerEscucha();
                return;
            }

            if (!gramaticaLista)
            {
                MostrarUltimaAccion("El reconocimiento de voz todavía se está preparando o no está disponible.", ColorAviso);
                return;
            }

            MarcarEscuchando(true);
            string error = await reconocedor.IniciarAsync();
            if (error != null && !IsDisposed)
            {
                MarcarEscuchando(false);
                MostrarUltimaAccion(error, ColorError);
            }
        }

        private void btnPTTVoz_MouseUp(object sender, MouseEventArgs e)
        {
            if (pttObligatorio) DetenerEscucha();
        }

        private async void DetenerEscucha()
        {
            if (reconocedor == null || !escuchando) return;
            MarcarEscuchando(false);
            await reconocedor.DetenerAsync();
        }

        private void MarcarEscuchando(bool activo)
        {
            escuchando = activo;
            btnPTTVoz.BackColor = activo ? Color.FromArgb(220, 80, 90) : colorPtt;
            btnPTTVoz.Text = activo ? "Escuchando…" : textoPtt;
        }

        // Lo que entendió el reconocedor (ya en el hilo de la pantalla).
        private void FraseEscuchada(string texto, decimal confianza)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                MostrarUltimaAccion("No te entendí (" + confianza.ToString("0") + " %). Mantén pulsado y repite el comando.", ColorAviso);
                return;
            }

            ProcesarFrase(texto, confianza, false);
        }

        // Los eventos del reconocedor llegan en otro hilo: se pasan al hilo de la pantalla.
        private void EnPantalla(Action accion)
        {
            if (IsDisposed || !IsHandleCreated) return;
            BeginInvoke(accion);
        }

        // ======================= 3) INTERPRETAR Y CONFIRMAR =======================

        private void ProcesarFrase(string texto, decimal confianza, bool escrito)
        {
            if (equipoActual == null) return;

            string frase = clsComandoNegocio.mtdNormalizarFrase(texto);
            bool esConfirmar = frase == clsOperacionVozNegocio.PalabraConfirmar;
            bool esCancelar = frase == clsOperacionVozNegocio.PalabraCancelar;

            // Hay un comando esperando: solo valen CONFIRMAR o CANCELAR.
            if (pendiente != null)
            {
                if (!esConfirmar && !esCancelar)
                {
                    MostrarUltimaAccion("Hay un comando esperando confirmación. Di CONFIRMAR o CANCELAR.", ColorAviso);
                    return;
                }

                if (!escrito && esConfirmar && confianza < umbralGlobal)
                {
                    MostrarUltimaAccion("No se entendió bien la confirmación (" + confianza.ToString("0") + " %). Repítela.", ColorAviso);
                    return;
                }

                Responder(frase);
                return;
            }

            if (esConfirmar || esCancelar)
            {
                MostrarUltimaAccion("No hay ningún comando pendiente de confirmación.", ColorAviso);
                return;
            }

            InterpretacionVoz r = voz.mtdInterpretar(comandos, texto, confianza);

            if (!r.Aceptado)
            {
                voz.mtdRegistrarRechazo(r, equipoActual);
                lblComandoReconocido.Text = "“" + texto + "”";
                lblEstadoComandoVoz.Text = "Rechazado · " + r.Confianza.ToString("0") + " %";
                lblEstadoComandoVoz.ForeColor = ColorError;
                MostrarUltimaAccion(r.Motivo, ColorError);
                Hablar(r.Comando == null ? "Comando no reconocido." : "No te entendí bien. Repite el comando.");
                return;
            }

            pendiente = voz.mtdIniciar(r, equipoActual);

            if (pendiente.RequiereConfirmacion)
                MostrarPaso();
            else
                Ejecutar();   // nivel sin confirmación (por ejemplo, consultar estado)
        }

        private void Responder(string respuesta)
        {
            if (pendiente == null) return;

            switch (voz.mtdResponder(pendiente, respuesta))
            {
                case EstadoConfirmacion.EsperandoSiguientePaso:
                    MostrarPaso();
                    break;

                case EstadoConfirmacion.ListaParaEjecutar:
                    Ejecutar();
                    break;

                default:
                    Cancelar("Comando cancelado por el operador.");
                    break;
            }
        }

        private void temporizador_Tick(object sender, EventArgs e)
        {
            if (pendiente == null)
            {
                temporizador.Stop();
                return;
            }

            if (pendiente.Vencida)
            {
                voz.mtdVencer(pendiente);
                Cancelar("Se agotó el tiempo de confirmación (" + pendiente.TimeoutSegundos + " s).");
                return;
            }

            ActualizarCuentaRegresiva();
        }

        private void Ejecutar()
        {
            OperacionPendiente p = pendiente;
            pendiente = null;

            Resultado resultado = voz.mtdEjecutar(p);
            MostrarSinPendiente();
            lblComandoReconocido.Text = "“" + p.Comando.Frase + "”";
            lblEstadoComandoVoz.Text = resultado.Exito ? "Ejecutado" : "No ejecutado";
            lblEstadoComandoVoz.ForeColor = resultado.Exito ? ColorOk : ColorError;
            MostrarUltimaAccion(resultado.Mensaje, resultado.Exito ? ColorOk : ColorError);
            Hablar(resultado.Exito ? "Comando enviado." : "No se pudo ejecutar el comando.");
        }

        private void Cancelar(string motivo)
        {
            OperacionPendiente p = pendiente;
            pendiente = null;

            voz.mtdCancelar(p, motivo);
            MostrarSinPendiente();
            lblComandoReconocido.Text = "“" + p.Comando.Frase + "”";
            lblEstadoComandoVoz.Text = "Cancelado";
            lblEstadoComandoVoz.ForeColor = ColorAviso;
            MostrarUltimaAccion(motivo, ColorAviso);
            Hablar("Comando cancelado.");
        }

        // ======================= PARADA =======================

        // Acción segura: no pide confirmación y cancela lo que esté pendiente.
        private void btnDetenerEquipo_Click(object sender, EventArgs e)
        {
            if (equipoActual == null)
            {
                MostrarUltimaAccion("Elige un equipo para detenerlo.", ColorAviso);
                return;
            }

            DetenerEscucha();
            string resultado = voz.mtdRegistrarParada(equipoActual, pendiente);   // dice si el Arduino la confirmó
            pendiente = null;
            MostrarSinPendiente();
            MostrarUltimaAccion("Parada · " + resultado, ColorError);
            Hablar("Equipo detenido.");
        }

        // ======================= PANTALLA =======================

        private void MostrarSinPendiente()
        {
            temporizador.Stop();
            lblComandoReconocido.Text = "Esperando comando…";
            lblEstadoComandoVoz.Text = "Sin acción pendiente";
            lblEstadoComandoVoz.ForeColor = Color.Silver;
            btnConfirmarComando.Text = "Confirmar y enviar";
            btnConfirmarComando.Enabled = false;
            btnCancelarComando.Enabled = false;
        }

        private void MostrarPaso()
        {
            Comando c = pendiente.Comando;

            lblComandoReconocido.Text = "“" + c.Frase + "”\n" + c.Nombre + " · " + c.CriticidadTexto +
                                        "\nConfianza: " + pendiente.Confianza.ToString("0.#") + " %";
            lblEstadoComandoVoz.ForeColor = FrmCriticidad.ColorNivel(c.IdNivelCriticidad);
            btnConfirmarComando.Text = pendiente.TotalPasos > 1
                ? "Confirmar (" + pendiente.PasoActual + " de " + pendiente.TotalPasos + ")"
                : "Confirmar y enviar";
            btnConfirmarComando.Enabled = true;
            btnCancelarComando.Enabled = true;

            ActualizarCuentaRegresiva();
            temporizador.Start();

            Hablar(pendiente.PasoActual == 1
                ? "Comando " + c.Nombre + ". Diga confirmar o cancelar."
                : "Segunda confirmación. Diga confirmar.");
        }

        private void ActualizarCuentaRegresiva()
        {
            lblEstadoComandoVoz.Text = "Confirmación " + pendiente.PasoActual + " de " + pendiente.TotalPasos +
                                       " · quedan " + pendiente.SegundosRestantes + " s";
        }

        private void MostrarUltimaAccion(string texto, Color color)
        {
            lblUltimaAccionVoz.Text = DateTime.Now.ToString("HH:mm:ss") + "  " + texto;
            lblUltimaAccionVoz.ForeColor = color;
        }

        private void Hablar(string texto)
        {
            if (sintetizador == null) return;
            try
            {
                sintetizador.SpeakAsyncCancelAll();
                sintetizador.SpeakAsync(texto);
            }
            catch (Exception) { /* la voz es opcional */ }
        }

        // Ventana pequeña para escribir la frase (modo texto o clic derecho en el botón).
        private string PedirTexto()
        {
            using (Form ventana = new Form())
            using (Label etiqueta = new Label())
            using (TextBox caja = new TextBox())
            using (Button aceptar = new Button())
            using (Button cancelar = new Button())
            {
                ventana.Text = "Escribir comando";
                ventana.FormBorderStyle = FormBorderStyle.FixedDialog;
                ventana.StartPosition = FormStartPosition.CenterParent;
                ventana.MaximizeBox = false;
                ventana.MinimizeBox = false;
                ventana.ShowInTaskbar = false;
                ventana.ClientSize = new Size(380, 120);
                ventana.BackColor = Color.FromArgb(22, 29, 37);

                etiqueta.Text = "Escribe la frase tal como la dirías (o CONFIRMAR / CANCELAR):";
                etiqueta.ForeColor = Color.Silver;
                etiqueta.Location = new Point(12, 12);
                etiqueta.Size = new Size(356, 20);

                caja.Location = new Point(12, 40);
                caja.Width = 356;

                aceptar.Text = "Enviar";
                aceptar.DialogResult = DialogResult.OK;
                aceptar.Location = new Point(212, 78);
                aceptar.Size = new Size(75, 28);
                aceptar.BackColor = Color.FromArgb(45, 43, 105);
                aceptar.ForeColor = Color.White;
                aceptar.FlatStyle = FlatStyle.Flat;

                cancelar.Text = "Cancelar";
                cancelar.DialogResult = DialogResult.Cancel;
                cancelar.Location = new Point(293, 78);
                cancelar.Size = new Size(75, 28);
                cancelar.BackColor = Color.FromArgb(32, 41, 56);
                cancelar.ForeColor = Color.White;
                cancelar.FlatStyle = FlatStyle.Flat;

                ventana.Controls.AddRange(new Control[] { etiqueta, caja, aceptar, cancelar });
                ventana.AcceptButton = aceptar;
                ventana.CancelButton = cancelar;

                return ventana.ShowDialog(this) == DialogResult.OK ? caja.Text : null;
            }
        }
    }
}
