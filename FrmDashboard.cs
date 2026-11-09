using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    // Ventana principal del administrador: menú a la izquierda y pantallas a la derecha.
    // Los clics de los botones del menú se conectan en el diseñador (FrmDashboard.Designer.cs).
    // El panel principal (dashboard) muestra datos reales de la BD y se actualiza cada 10 segundos.
    public partial class frmPrincipal : Form
    {
        private static readonly Color ColorOk = Color.MediumAquamarine;
        private static readonly Color ColorAviso = Color.Goldenrod;
        private static readonly Color ColorError = Color.Salmon;
        private static readonly Color ColorApagado = Color.Gray;

        private const int ActividadReciente = 12;   // filas de la tabla "Actividad reciente"

        private Form formularioActivo;  // pantalla que se está mostrando ahora en pnlVista
        private readonly clsDashboardNegocio dashboardNegocio = new clsDashboardNegocio();
        private readonly clsEventoNegocio eventoNegocio = new clsEventoNegocio();
        private readonly Timer actualizacion = new Timer { Interval = 10000 };

        // True cuando se pulsó "Cerrar sesión" (volver al login en vez de salir).
        public bool CerrarSesionSolicitada { get; private set; }

        public frmPrincipal()
        {
            InitializeComponent();

            // Datos del usuario que inició sesión (abajo del menú y en la tarjeta "Sesión actual").
            if (Sesion.Activa)
            {
                lblSesionAdmin.Text = Sesion.UsuarioActual.NombreCompleto;
                lblEstadoSesion.Text = Sesion.UsuarioActual.Codigo + " · " + Sesion.UsuarioActual.RolNombre;
            }

            // Los íconos se dibujan por código porque son imágenes hechas con una fuente.
            ColocarIcono(btnDashboard, "⌂");
            ColocarIcono(btnDiagnostico, "⚒");
            ColocarIcono(btnAlertas, "⚠");
            ColocarIcono(btnCriticidad, "◆");
            ColocarIcono(btnEventos, "☷");
            ColocarIcono(btnOperadores, "♟");
            ColocarIcono(btnCredenciales, "▣");
            ColocarIcono(btnEquipos, "▤");
            ColocarIcono(btnComandos, "▶");
            ColocarIcono(btnSincronizacion, "↻");
            ColocarIcono(btnReportes, "▥");
            ColocarIcono(btnConfiguracion, "⚙");

            // Textos que antes hablaban del ESP32 / enlace simulado.
            v.Text = "Comandos ejecutados con éxito (hoy)";
            pgbSaludEnlace.Minimum = 0;
            pgbSaludEnlace.Maximum = 100;

            // Eventos del panel (se conectan aquí para no tocar el diseñador).
            // Si falla, se avisa: un error dentro de Load puede perderse sin mensaje al depurar en 64 bits.
            Load += (s, e) =>
            {
                try { CargarDashboard(); }
                catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error al cargar el dashboard", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                actualizacion.Start();
            };
            FormClosed += (s, e) => { actualizacion.Stop(); actualizacion.Dispose(); };
            actualizacion.Tick += (s, e) => { if (!pnlVista.Visible) CargarDashboard(); };
            btnParadaSimulada.Click += btnParadaSimulada_Click;

            AplicarPermisos();
        }

        // ---------------- Permisos por rol ----------------

        // Oculta los botones del menú que el rol no puede usar.
        // El menú es un FlowLayoutPanel, así que los botones visibles se acomodan solos.
        private void AplicarPermisos()
        {
            btnDashboard.Visible = clsPermisoNegocio.mtdPuedeVer(clsPermisoNegocio.Dashboard);
            btnDiagnostico.Visible = clsPermisoNegocio.mtdPuedeVer(clsPermisoNegocio.Diagnostico);
            btnAlertas.Visible = clsPermisoNegocio.mtdPuedeVer(clsPermisoNegocio.Alertas);
            btnCriticidad.Visible = clsPermisoNegocio.mtdPuedeVer(clsPermisoNegocio.Criticidad);
            btnEventos.Visible = clsPermisoNegocio.mtdPuedeVer(clsPermisoNegocio.Eventos);
            btnOperadores.Visible = clsPermisoNegocio.mtdPuedeVer(clsPermisoNegocio.Operadores);
            btnCredenciales.Visible = clsPermisoNegocio.mtdPuedeVer(clsPermisoNegocio.Credenciales);
            btnEquipos.Visible = clsPermisoNegocio.mtdPuedeVer(clsPermisoNegocio.Equipos);
            btnComandos.Visible = clsPermisoNegocio.mtdPuedeVer(clsPermisoNegocio.Comandos);
            btnSincronizacion.Visible = clsPermisoNegocio.mtdPuedeVer(clsPermisoNegocio.Sincronizacion);
            btnReportes.Visible = clsPermisoNegocio.mtdPuedeVer(clsPermisoNegocio.Reportes);
            btnConfiguracion.Visible = clsPermisoNegocio.mtdPuedeVer(clsPermisoNegocio.Configuracion);
        }

        // ---------------- Botones del menú ----------------

        private void btnDashboard_Click(object sender, EventArgs e) { MostrarDashboard(); }
        private void btnDiagnostico_Click(object sender, EventArgs e) { AbrirFormulario(new FrmDiagnostico()); }
        private void btnAlertas_Click(object sender, EventArgs e) { AbrirFormulario(new FrmAlertas()); }
        private void btnCriticidad_Click(object sender, EventArgs e) { AbrirFormulario(new FrmCriticidad()); }
        private void btnEventos_Click(object sender, EventArgs e) { AbrirFormulario(new FrmEventos()); }
        private void btnOperadores_Click(object sender, EventArgs e) { AbrirFormulario(new FrmOperadores()); }
        private void btnCredenciales_Click(object sender, EventArgs e) { AbrirFormulario(new Credenciales()); }
        private void btnEquipos_Click(object sender, EventArgs e) { AbrirFormulario(new FrmEquipos()); }
        private void btnComandos_Click(object sender, EventArgs e) { AbrirFormulario(new FrmComandos()); }
        private void btnSincronizacion_Click(object sender, EventArgs e) { AbrirFormulario(new FrmSincronizacion()); }
        private void btnReportes_Click(object sender, EventArgs e) { AbrirFormulario(new FrmReportes()); }
        private void btnConfiguracion_Click(object sender, EventArgs e) { AbrirFormulario(new FrmConfiguracion()); }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            CerrarSesionSolicitada = true;
            Close();
        }

        // ---------------- Pantallas ----------------

        private void AbrirFormulario(Form formulario)
        {
            clsPantallas.mtdCerrar(formularioActivo);
            formularioActivo = formulario;
            clsPantallas.mtdMostrarEnPanel(pnlVista, formulario, "flpMenu");
        }

        // Vuelve al dashboard: cierra la pantalla abierta, oculta el panel y recarga los datos.
        private void MostrarDashboard()
        {
            clsPantallas.mtdCerrar(formularioActivo);
            formularioActivo = null;
            pnlVista.Visible = false;
            CargarDashboard();
        }

        // ======================= DATOS DEL DASHBOARD =======================

        private void CargarDashboard()
        {
            ResumenDashboard r;
            List<RegistroHistorial> actividad;

            try
            {
                r = dashboardNegocio.mtdResumen();
                actividad = eventoNegocio.mtdListarHistorial(ActividadReciente);
            }
            catch (Exception)
            {
                MostrarSinConexion();
                return;
            }

            MostrarCabecera(true);
            MostrarIndicadores(r);
            MostrarTotales(r);
            MostrarArquitectura(r);
            MostrarExito(r);
            MostrarSimulacion(r);
            MostrarActividad(actividad);
        }

        private void MostrarCabecera(bool conectado)
        {
            lblEstadoDatos.Text = conectado ? "● SQL Server conectado" : "● Sin conexión a la BD";
            lblEstadoDatos.ForeColor = conectado ? ColorOk : ColorError;

            lblConexionESP32.Text = clsMonitorArduino.TextoEstado;
            lblConexionESP32.ForeColor = clsMonitorArduino.Conectado ? ColorOk : ColorApagado;

            lblEstadoApp.Text = "● App en línea · " + DateTime.Now.ToString("HH:mm:ss");
            lblEstadoApp.ForeColor = ColorOk;
        }

        // Tarjetas de arriba: comandos de hoy, confianza y alertas pendientes.
        private void MostrarIndicadores(ResumenDashboard r)
        {
            lblCantidadComandos.Text = r.EjecutadosHoy + " / " + r.IntentosHoy;
            lblCantidadComandos.ForeColor = r.IntentosHoy == 0 ? Color.White : ColorOk;

            decimal? confianza = r.ConfianzaHoy ?? r.Confianza7Dias;
            lblPorcentajeConfianza.Text = confianza.HasValue ? confianza.Value.ToString("0.#") + " %" : "—";
            lblPorcentajeConfianza.ForeColor = !confianza.HasValue ? Color.White
                                             : confianza.Value >= 85m ? ColorOk
                                             : confianza.Value >= 70m ? ColorAviso : ColorError;

            lblCantidadAlertas.Text = r.AlertasPendientes.ToString();
            lblCantidadAlertas.ForeColor = r.AlertasActivas > 0 ? ColorError
                                         : r.AlertasPendientes > 0 ? ColorAviso : ColorOk;
        }

        private void MostrarTotales(ResumenDashboard r)
        {
            lblTotalOperadores.Text = r.Operadores.ToString();
            lblTotalCredenciales.Text = r.Credenciales.ToString();
            lblTotalComandos.Text = r.Comandos.ToString();
            lblTotalEquipos.Text = r.Equipos.ToString();
        }

        // "Estado de la arquitectura": sesión, PTT, reconocimiento de voz y enlace con el Arduino.
        private void MostrarArquitectura(ResumenDashboard r)
        {
            // Sesión
            bool hayAdmin = Sesion.Activa;
            lblDetalleSesion.Text = hayAdmin
                ? Sesion.UsuarioActual.Codigo + " · " + r.SesionesAbiertas + " sesión(es) abiertas hoy"
                : "Sin autenticar";
            lblIndicadorSesion.ForeColor = hayAdmin ? ColorOk : ColorError;

            // PTT (según Configuración)
            bool pttObligatorio = true;
            try { pttObligatorio = new clsConfiguracionNegocio().mtdObtenerBooleano(clsConfiguracionNegocio.PttObligatorio); }
            catch (Exception) { }
            lblEstadoPTT.Text = pttObligatorio ? "Obligatorio" : "Opcional";
            lblIndicadorPTT.ForeColor = pttObligatorio ? ColorOk : ColorAviso;

            // Reconocimiento de voz offline (Windows en español)
            string idioma = clsReconocedorVoz.BuscarIdiomaEspanol();
            lblEstadoSTT.Text = idioma == null ? "Sin voz en español" : "Disponible · " + idioma;
            lblIndicadorSTT.ForeColor = idioma == null ? ColorError : ColorOk;

            // Enlace con el Arduino (por ahora: presencia del puerto COM)
            lblEstadoHeartbeat.Text = clsMonitorArduino.Conectado
                ? "Arduino en " + clsMonitorArduino.Puerto
                : string.IsNullOrEmpty(clsMonitorArduino.Puerto) ? "Puerto sin configurar" : "Sin Arduino (" + clsMonitorArduino.Puerto + ")";
            lblIndicadorHeartbeat.ForeColor = clsMonitorArduino.Conectado ? ColorOk : ColorApagado;
        }

        // Barra: porcentaje de comandos de hoy que se ejecutaron.
        private void MostrarExito(ResumenDashboard r)
        {
            int? porcentaje = r.PorcentajeExito;

            pgbSaludEnlace.Value = Math.Max(0, Math.Min(100, porcentaje ?? 0));
            lblPorcentajeEnlace.Text = porcentaje.HasValue
                ? porcentaje + " % · " + r.EjecutadosHoy + " de " + r.IntentosHoy + " · " + r.RechazadosHoy + " rechazados"
                : "Sin comandos hoy";
            lblPorcentajeEnlace.ForeColor = !porcentaje.HasValue ? Color.Silver
                                          : porcentaje >= 80 ? ColorOk
                                          : porcentaje >= 50 ? ColorAviso : ColorError;
        }

        // "Estado de simulación": se deduce de los últimos comandos ejecutados (hasta conectar el Arduino).
        private void MostrarSimulacion(ResumenDashboard r)
        {
            Estado(lblEstadoMotor, lblIndicadorMotor, r.MotorEnMarcha,
                   r.MotorEnMarcha ? "En marcha" : "Detenido", r.MotorDesde, ColorOk);

            Estado(lblEstadoLED, lblIndicadorLED, r.LedEncendido,
                   r.LedEncendido ? "Encendido" : "Apagado", r.LedDesde, ColorOk);

            Estado(lblEstadoBuzzer, lblIndicadorBuzzer, r.BuzzerActivo,
                   r.BuzzerActivo ? "Sonando" : "Apagado", r.BuzzerActivo ? r.BuzzerDesde : null, ColorAviso);

            Estado(lblEstadoParada, lblIndicadorParada, r.ParadaActiva,
                   r.ParadaActiva ? "Activada" : "Normal", r.ParadaActiva ? r.UltimaParada : null, ColorError);
        }

        private static void Estado(Label texto, Label indicador, bool activo, string estado, DateTime? desde, Color colorActivo)
        {
            texto.Text = estado + (desde.HasValue ? " · " + desde.Value.ToString("HH:mm") : "");
            indicador.ForeColor = activo ? colorActivo : ColorApagado;
        }

        private void MostrarActividad(List<RegistroHistorial> actividad)
        {
            dgvActividadReciente.Rows.Clear();

            foreach (RegistroHistorial a in actividad)
            {
                string quien = a.Usuario == "Sistema" ? "" : a.Usuario.Split(' ')[0] + " · ";
                int fila = dgvActividadReciente.Rows.Add(
                    a.FechaHora.ToString(a.FechaHora.Date == DateTime.Today ? "HH:mm:ss" : "dd/MM HH:mm"),
                    quien + a.Descripcion,
                    a.Fuente == "AUDITORIA" ? "Auditoría" : a.ResultadoTexto);

                dgvActividadReciente.Rows[fila].Cells[colEstado.Index].Style.ForeColor = ColorResultado(a.Resultado);
            }

            dgvActividadReciente.ClearSelection();
        }

        private static Color ColorResultado(string resultado)
        {
            switch (resultado)
            {
                case clsEventoNegocio.ResultadoEjecutado: return ColorOk;
                case clsEventoNegocio.ResultadoRechazado:
                case clsEventoNegocio.ResultadoError: return ColorError;
                case clsEventoNegocio.ResultadoCancelado:
                case clsEventoNegocio.ResultadoIgnorado: return ColorAviso;
                case "AUDITORIA": return Color.MediumPurple;
                default: return Color.LightSkyBlue;
            }
        }

        private void MostrarSinConexion()
        {
            MostrarCabecera(false);
            lblEstadoApp.Text = "● Reintentando…";
            lblEstadoApp.ForeColor = ColorAviso;
        }

        // ---------------- Parada general ----------------

        // Envía una parada a todos los equipos activos. Es una acción segura, pero se confirma para evitar clics por error.
        private void btnParadaSimulada_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Enviar PARADA a todos los equipos activos?\nSe registrará el evento y una alerta por cada equipo.",
                    "Parada simulada", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            Resultado resultado = dashboardNegocio.mtdParadaGeneral();
            MessageBox.Show(resultado.Mensaje, "Parada simulada", MessageBoxButtons.OK,
                resultado.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (!pnlVista.Visible) CargarDashboard();
        }

        // ---------------- Íconos ----------------

        // Dibuja el símbolo como imagen a la izquierda del texto del botón.
        // Ojo: usa su propio tamaño (28x35) y fuente; no es igual al ícono del menú del operador.
        private void ColocarIcono(Button boton, string simbolo)
        {
            Bitmap imagen = new Bitmap(28, 35);

            using (Graphics dibujo = Graphics.FromImage(imagen))
            using (Font fuente = new Font("Segoe UI Symbol", 16))
            {
                dibujo.Clear(Color.Transparent);

                TextRenderer.DrawText(dibujo, simbolo, fuente, new Rectangle(0, 0, 28, 28), Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }

            boton.Image = imagen;
            boton.ImageAlign = ContentAlignment.MiddleLeft;
            boton.TextAlign = ContentAlignment.MiddleLeft;
            boton.TextImageRelation = TextImageRelation.ImageBeforeText;
            boton.Padding = new Padding(10, 0, 0, 0);
        }
    }
}
