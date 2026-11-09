using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    // Ventana principal del operador: menú a la izquierda y pantallas a la derecha (pnlPantallasOperador).
    // Los clics de los botones se conectan en el diseñador (FrmPrincipalOperador.Designer.cs).
    public partial class FrmPrincipalOperador : Form
    {
        private Form pantallaActual;  // pantalla que se está mostrando ahora
        private readonly Timer estadoConexion = new Timer { Interval = 3000 };  // revisa si hay conexión con Azure

        // True cuando el operador pulsó "Cerrar sesión" (volver al login en vez de salir).
        public bool CerrarSesionSolicitada { get; private set; }

        public FrmPrincipalOperador()
        {
            InitializeComponent();

            // Datos del operador que inició sesión.
            if (Sesion.Activa)
            {
                lblNombreOperador.Text = Sesion.UsuarioActual.NombreCompleto;
                lblRolOperador.Text = Sesion.UsuarioActual.RolNombre;
            }

            // Colores e íconos del menú (se dibujan por código).
            EstiloMenuOperador.Aplicar(pnlMenuOperador, pnlSesionOperador, btnInicioOperador,
                btnControlVozOperador, btnHistorialOperador, btnCerrarSesionOperador, btnInicioOperador);

            // Debajo de "SafeHands" se avisa si se está trabajando sin conexión (las órdenes se guardan en la PC).
            MostrarConexion();
            estadoConexion.Tick += (s, e) => MostrarConexion();
            estadoConexion.Start();
            Disposed += (s, e) => estadoConexion.Dispose();

            // La pantalla de inicio se llena con los datos reales del operador.
            // Si falla, se avisa: un error dentro de Load puede perderse sin mensaje al depurar en 64 bits.
            Load += (s, e) =>
            {
                try { CargarInicio(); }
                catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error al cargar el inicio", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };
        }

        private void MostrarConexion()
        {
            if (clsSincronizacionNegocio.SinConexion)
            {
                lblSeccionOperador.Text = "● SIN CONEXIÓN · SE GUARDA EN LA PC";
                lblSeccionOperador.ForeColor = Color.Goldenrod;
            }
            else
            {
                lblSeccionOperador.Text = "ESPACIO DEL OPERADOR";
                lblSeccionOperador.ForeColor = Color.Silver;
            }
        }

        // ---------------- Botones del menú ----------------

        private void btnInicioOperador_Click(object sender, EventArgs e)
        {
            clsPantallas.mtdCerrar(pantallaActual);
            pantallaActual = null;
            pnlPantallasOperador.Visible = false;
            MarcarMenu(btnInicioOperador);
            CargarInicio();   // al volver al inicio se ven las acciones recién hechas
        }

        private void btnControlVozOperador_Click(object sender, EventArgs e)
        {
            MostrarPantalla(new FrmOperacionVoz(), btnControlVozOperador);
        }

        private void btnHistorialOperador_Click(object sender, EventArgs e)
        {
            MostrarPantalla(new FrmMiHistorial(), btnHistorialOperador);
        }

        // Botón "Ir a control por voz" del inicio.
        private void btnIrControlVoz_Click(object sender, EventArgs e)
        {
            MostrarPantalla(new FrmOperacionVoz(), btnControlVozOperador);
        }

        private void btnCerrarSesionOperador_Click(object sender, EventArgs e)
        {
            CerrarSesionSolicitada = true;
            Close();
        }

        private void btnVerComandosInicio_Click(object sender, EventArgs e)
        {
            new FrmComandosDisponibles().ShowDialog();
        }

        // ---------------- Inicio con datos reales ----------------

        // Llena el saludo y las tarjetas con la sesión y el historial del operador que entró.
        private void CargarInicio()
        {
            if (!Sesion.Activa) return;

            lblTituloInicio.Text = "Hola, " + Sesion.UsuarioActual.Nombres;
            lblRutaInicio.Text = "ORICA / OPERADOR / " + Sesion.UsuarioActual.Codigo;

            ResumenOperador r;
            try
            {
                r = new clsDashboardNegocio().mtdResumenOperador();
            }
            catch (Exception)
            {
                lblMensaje.Text = "No se pudo conectar con la base de datos para mostrar tu resumen.";
                lblMensaje.ForeColor = Color.Salmon;
                return;
            }

            // Mensaje superior: última acción o primera vez
            lblMensaje.ForeColor = Color.Silver;
            if (r.UltimaAccionFecha.HasValue)
            {
                lblMensaje.Text = "Tu última acción: " + r.UltimaAccionTexto + " · " +
                                  TextoResultado(r.UltimaAccionResultado) + " · " + TextoFecha(r.UltimaAccionFecha.Value);
            }
            else
            {
                lblMensaje.Text = "Aún no has dado comandos. Empieza en Control por voz.";
            }

            if (r.SesionAnterior.HasValue)
                lblMensaje.Text += "   |   Sesión anterior: " + TextoFecha(r.SesionAnterior.Value);

            // Tarjeta: equipos disponibles
            lblCantidadEquiposInicio.Text = r.EquiposDisponibles.ToString();
            lblDetalleEquiposInicio.Text = r.EquiposDisponibles == 0
                ? "Ningún equipo tiene comandos activos"
                : r.CodigosEquipos + " · " + r.ComandosDisponibles + " comando(s)";

            // Tarjeta: acciones en esta sesión
            lblCantidadAccionesInicio.Text = r.AccionesSesion.ToString();
            lblDetalleAccionesInicio.Text = r.AccionesSesion == 0
                ? "Comandos procesados en esta sesión"
                : r.EjecutadasSesion + " ejecutada(s) · " + r.RechazadasSesion + " rechazada(s) · " +
                  r.CanceladasSesion + " cancelada(s)";

            // Tarjeta: acciones confirmadas (ejecutadas después de confirmar)
            lblCantidadConfirmadasInicio.Text = r.ConfirmadasSesion.ToString();
            lblDetalleConfirmadasInicio.Text = "De " + r.EjecutadasSesion + " ejecutada(s) en esta sesión · " +
                                               r.EjecutadasTotal + " en total";

            // Ayuda
            lblDescripcionAyudaInicio.Text = "Tienes " + r.ComandosDisponibles +
                " frase(s) disponibles. Revisa cuáles piden confirmación antes de usarlas.";
        }

        private static string TextoResultado(string resultado)
        {
            switch (resultado)
            {
                case clsEventoNegocio.ResultadoEjecutado: return "ejecutada";
                case clsEventoNegocio.ResultadoRechazado: return "rechazada";
                case clsEventoNegocio.ResultadoCancelado: return "cancelada";
                case clsEventoNegocio.ResultadoError: return "con error";
                default: return (resultado ?? "").ToLower();
            }
        }

        // "hoy 14:32", "ayer 09:10" o "05/11 18:00"
        private static string TextoFecha(DateTime fecha)
        {
            if (fecha.Date == DateTime.Today) return "hoy " + fecha.ToString("HH:mm");
            if (fecha.Date == DateTime.Today.AddDays(-1)) return "ayer " + fecha.ToString("HH:mm");
            return fecha.ToString("dd/MM HH:mm");
        }

        // ---------------- Pantallas ----------------

        private void MostrarPantalla(Form pantalla, Button seleccionado)
        {
            clsPantallas.mtdCerrar(pantallaActual);
            pantallaActual = pantalla;
            clsPantallas.mtdMostrarEnPanel(pnlPantallasOperador, pantalla, "pnlMenuOperador");
            MarcarMenu(seleccionado);
        }

        // Pinta de morado el botón elegido y deja los demás con el fondo normal.
        private void MarcarMenu(Button seleccionado)
        {
            Color fondo = Color.FromArgb(17, 24, 34);
            btnInicioOperador.BackColor = fondo;
            btnControlVozOperador.BackColor = fondo;
            btnHistorialOperador.BackColor = fondo;

            seleccionado.BackColor = Color.FromArgb(50, 43, 96);
        }
    }
}
