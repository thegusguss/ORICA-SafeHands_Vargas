using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    /// <summary>
    /// Estado de Azure SQL (base principal) y de la copia local (LocalDB).
    /// Muestra cuántos registros hechos sin internet faltan subir y el historial de subidas (dbo.Sincronizacion).
    /// La sincronización automática corre sola (clsSincronizacionNegocio); aquí se puede forzar o simular un corte.
    /// </summary>
    public partial class FrmSincronizacion : Form
    {
        private static readonly Color ColorOk = Color.MediumAquamarine;
        private static readonly Color ColorAviso = Color.Goldenrod;
        private static readonly Color ColorError = Color.Salmon;

        // Botones configurados en tiempo de ejecución para la primera vez en una PC.
        private readonly Button btnDescargarAzure = new Button();
        private readonly Button btnSubirAzure = new Button();

        public FrmSincronizacion()
        {
            InitializeComponent();

            lblSubtituloSincronizacion.Text = "Azure SQL y copia local";
            lblTituloSyncSQLServer.Text = "AZURE SQL";
            lblTituloSyncSQLite.Text = "COPIA LOCAL";
            lblDetalleSyncSQLite.Text = "Última copia desde Azure";
            lblDetalleSyncPendientes.Text = "Hechos sin conexión, por subir";
            lblTituloColaSincronizacion.Text = "Historial de subidas a Azure";
            colIdLocalSync.HeaderText = "DETALLE";
            colEstadoSync.HeaderText = "ESTADO / FECHA";

            ConfigurarBotonesAzure();
        }

        private void FrmSincronizacion_Load(object sender, EventArgs e)
        {
            MostrarEstado();
            CargarHistorial();
            actualizacion.Start();
        }

        private void FrmSincronizacion_Disposed(object sender, EventArgs e)
        {
            actualizacion.Stop();
            actualizacion.Dispose();
        }

        private void actualizacion_Tick(object sender, EventArgs e)
        {
            if (!IsDisposed) MostrarEstado();
        }

        private async void btnSincronizarAhora_Click(object sender, EventArgs e)
        {
            await Ejecutar(clsSincronizacionNegocio.mtdSincronizar, null);
        }

        private async void btnDescargarAzure_Click(object sender, EventArgs e)
        {
            await Ejecutar(clsSincronizacionNegocio.mtdDescargarDeAzure,
                "Se borrarán los datos de esta PC y se reemplazarán por los de Azure. ¿Continuar?");
        }

        private async void btnSubirAzure_Click(object sender, EventArgs e)
        {
            await Ejecutar(clsSincronizacionNegocio.mtdSubirTodoAAzure,
                "Se borrarán TODOS los datos de Azure y se reemplazarán por los de esta PC.\n" +
                "Úselo solo la primera vez. ¿Continuar?");
        }

        private void btnSimularRedSQL_Click(object sender, EventArgs e)
        {
            SimularCorte();
        }

        private void ConfigurarBotonesAzure()
        {
            CrearBoton(btnDescargarAzure, "Descargar de Azure", 446);
            CrearBoton(btnSubirAzure, "Subir esta PC a Azure", 270);
            btnSubirAzure.Visible = Sesion.Activa && Sesion.UsuarioActual.RolCodigo == "ADMIN";
            btnDescargarAzure.Click += new System.EventHandler(btnDescargarAzure_Click);
            btnSubirAzure.Click += new System.EventHandler(btnSubirAzure_Click);
        }

        private void CrearBoton(Button boton, string texto, int x)
        {
            boton.Text = texto;
            boton.Location = new Point(x, btnSincronizarAhora.Top);
            boton.Size = btnSincronizarAhora.Size;
            boton.Anchor = btnSincronizarAhora.Anchor;
            boton.BackColor = btnSincronizarAhora.BackColor;
            boton.ForeColor = Color.White;
            boton.FlatStyle = FlatStyle.Flat;
            boton.Font = btnSincronizarAhora.Font;
            boton.Cursor = Cursors.Hand;
            pnlColaSincronizacion.Controls.Add(boton);
        }

        // ---------------- Mostrar ----------------

        private void MostrarEstado()
        {
            EstadoSincronizacion estado;
            try
            {
                estado = clsSincronizacionNegocio.mtdEstado();
            }
            catch (Exception ex)
            {
                lblEstadoSyncSQLServer.Text = "Error";
                lblDetalleSyncSQLServer.Text = "No se pudo leer la base local: " + ex.Message;
                return;
            }

            // Cabecera
            lblEstadoDatos.Text = clsSincronizacionNegocio.TextoEstado;
            lblEstadoDatos.ForeColor = !estado.TieneAzure ? Color.Silver : estado.EnLinea ? ColorOk : ColorAviso;
            lblConexionESP32.Text = clsMonitorArduino.TextoEstado;
            lblConexionESP32.ForeColor = clsMonitorArduino.Conectado ? ColorOk : Color.Silver;
            lblDiagEstadoApp.Text = "● App en línea";
            lblDiagEstadoApp.ForeColor = ColorOk;

            // Azure
            if (!estado.TieneAzure)
            {
                lblEstadoSyncSQLServer.Text = "No configurado";
                lblEstadoSyncSQLServer.ForeColor = Color.Silver;
                lblDetalleSyncSQLServer.Text = "Falta conexion.config";
            }
            else if (estado.EnLinea)
            {
                lblEstadoSyncSQLServer.Text = "Conectado";
                lblEstadoSyncSQLServer.ForeColor = ColorOk;
                lblDetalleSyncSQLServer.Text = "Base principal compartida";
            }
            else
            {
                lblEstadoSyncSQLServer.Text = estado.Simulado ? "Corte simulado" : "Sin conexión";
                lblEstadoSyncSQLServer.ForeColor = ColorAviso;
                lblDetalleSyncSQLServer.Text = Recortar(estado.UltimoError ?? "Reintenta cada 30 s", 38);
            }
            MostrarAyuda(lblDetalleSyncSQLServer, estado.UltimoError);   // el error completo al pasar el mouse

            if (estado.Ocupado)
            {
                lblCantidadSyncPendientes.Text = "…";
                lblDetalleSyncPendientes.Text = "Sincronizando...";
                return;
            }
            lblDetalleSyncPendientes.Text = "Hechos sin conexión, por subir";

            // Copia local
            lblCantidadRegistrosLocales.Text = estado.FechaCopia.HasValue
                ? estado.FechaCopia.Value.ToString("dd/MM HH:mm") : "Nunca";
            lblCantidadRegistrosLocales.ForeColor = estado.FechaCopia.HasValue ? Color.White : ColorAviso;

            // Pendientes
            lblCantidadSyncPendientes.Text = estado.Pendientes.ToString();
            lblCantidadSyncPendientes.ForeColor = estado.Pendientes > 0 ? ColorAviso : Color.White;

            // Última subida (de esta sesión de la app)
            lblFechaUltimaSync.Text = estado.UltimaSubida.HasValue
                ? estado.UltimaSubida.Value.ToString("dd/MM HH:mm") : "Sin subidas";
            lblDetalleUltimaSync.Text = estado.UltimaSubida.HasValue ? "Pendientes enviados a Azure" : "Desde que se abrió la app";

            // Botones
            btnSimularRedSQL.Text = estado.Simulado ? "Volver a conectar" : "Simular sin conexión";
            btnSimularRedSQL.Enabled = estado.TieneAzure;
            btnSincronizarAhora.Enabled = estado.TieneAzure;
            btnDescargarAzure.Enabled = estado.TieneAzure;
            btnSubirAzure.Enabled = estado.TieneAzure;
        }

        private void CargarHistorial()
        {
            dgvColaSincronizacion.Rows.Clear();
            try
            {
                foreach (RegistroSincronizacion r in clsSincronizacionNegocio.mtdListarHistorial())
                {
                    string estado = r.Estado + (r.Fecha.HasValue ? " · " + r.Fecha.Value.ToString("dd/MM/yy HH:mm") : "");
                    dgvColaSincronizacion.Rows.Add(r.Id, r.Tabla, r.Detalle, r.Intentos, estado);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el historial.\n" + ex.Message, "Sincronización",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ---------------- Acciones ----------------

        // Ejecuta una acción en segundo plano (puede tardar si Azure demora en responder).
        private async Task Ejecutar(Func<Resultado> accion, string confirmacion)
        {
            if (confirmacion != null &&
                MessageBox.Show(confirmacion, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            pnlColaSincronizacion.Enabled = false;
            Cursor = Cursors.WaitCursor;

            Resultado resultado = await Task.Run(accion);

            if (IsDisposed) return;
            Cursor = Cursors.Default;
            pnlColaSincronizacion.Enabled = true;

            MostrarEstado();
            CargarHistorial();
            MessageBox.Show(resultado.Mensaje, "Sincronización", MessageBoxButtons.OK,
                resultado.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        // Para probar sin desconectar internet: la app trabaja con la copia local hasta "Volver a conectar".
        private async void SimularCorte()
        {
            bool activar = !clsSincronizacionNegocio.mtdEstado().Simulado;
            clsSincronizacionNegocio.mtdSimularSinConexion(activar);

            if (activar)
            {
                MostrarEstado();
                MessageBox.Show("Ahora la aplicación trabaja como si no hubiera internet.\n" +
                    "Lo que haga un operador se guardará en esta PC y se subirá al volver a conectar.",
                    "Corte simulado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                await Ejecutar(clsSincronizacionNegocio.mtdSincronizar, null);   // vuelve y sube lo pendiente
            }
        }

        // ---------------- Ayudas ----------------

        private static string Recortar(string texto, int maximo)
        {
            return texto.Length <= maximo ? texto : texto.Substring(0, maximo - 3) + "...";
        }

        private readonly ToolTip ayuda = new ToolTip();

        private void MostrarAyuda(Control control, string texto)
        {
            ayuda.SetToolTip(control, texto ?? "");
        }
    }
}
