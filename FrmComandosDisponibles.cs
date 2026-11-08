using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    /// <summary>Muestra los comandos de voz de un equipo como tarjetas, con un buscador.</summary>
    public partial class FrmComandosDisponibles : Form
    {
        private readonly List<ComandoDisponible> comandos = new List<ComandoDisponible>();

        // Sin equipo: muestra el primer equipo activo que tenga comandos asignados.
        public FrmComandosDisponibles() : this(null)
        {
        }

        // Con equipo: muestra solo los comandos que el operador puede usar en ese equipo.
        public FrmComandosDisponibles(Equipo equipo)
        {
            InitializeComponent();
            CargarDesdeBaseDatos(equipo);
        }

        private void CargarDesdeBaseDatos(Equipo equipo)
        {
            try
            {
                clsComandoNegocio comandoNegocio = new clsComandoNegocio();
                List<Comando> lista = new List<Comando>();

                if (equipo != null)
                {
                    lista = comandoNegocio.mtdListarPorEquipo(equipo.IdEquipo);
                }
                else
                {
                    foreach (Equipo eq in new clsEquipoNegocio().mtdListarActivos())
                    {
                        lista = comandoNegocio.mtdListarPorEquipo(eq.IdEquipo);
                        if (lista.Count > 0) { equipo = eq; break; }
                    }
                }

                List<ComandoDisponible> disponibles = new List<ComandoDisponible>();
                foreach (Comando c in lista)
                {
                    disponibles.Add(new ComandoDisponible
                    {
                        Frase = c.Frase,
                        Descripcion = c.Nombre + " · " + c.CriticidadTexto + " · Confianza mínima " +
                                      c.ConfianzaMinima.ToString("0.##") + " %" + (c.RequierePTT ? " · Mantener PTT" : ""),
                        Confirmacion = c.ConfirmacionTexto,
                        RequiereConfirmacion = c.NumeroConfirmaciones > 0
                    });
                }

                CargarComandos(equipo == null ? "Sin equipos con comandos asignados" : equipo.Etiqueta, disponibles);
            }
            catch (Exception ex)
            {
                CargarComandos("Sin conexión", null);
                MessageBox.Show("No se pudieron cargar los comandos.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CargarComandos(string nombreEquipo, IEnumerable<ComandoDisponible> comandosPermitidos)
        {
            lblEquipoComandos.Text = "Equipo: " + nombreEquipo;

            comandos.Clear();
            if (comandosPermitidos != null)
                comandos.AddRange(comandosPermitidos);

            txtBuscarComando.Clear();
            MostrarComandos();
        }

        private void txtBuscarComando_TextChanged(object sender, EventArgs e)
        {
            MostrarComandos();
        }

        private void btnEntendidoComandos_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Vuelve a dibujar las tarjetas, solo las que coinciden con el texto buscado.
        private void MostrarComandos()
        {
            flpComandosDisponibles.SuspendLayout();

            // Borra las tarjetas anteriores (Dispose también las quita del panel).
            while (flpComandosDisponibles.Controls.Count > 0)
                flpComandosDisponibles.Controls[0].Dispose();

            string busqueda = txtBuscarComando.Text.Trim();
            int encontrados = 0;

            foreach (ComandoDisponible comando in comandos)
            {
                string texto = comando.Frase + " " + comando.Descripcion;
                if (texto.IndexOf(busqueda, StringComparison.CurrentCultureIgnoreCase) < 0)
                    continue;

                flpComandosDisponibles.Controls.Add(CrearTarjetaComando(comando));
                encontrados++;
            }

            if (encontrados == 0)
            {
                flpComandosDisponibles.Controls.Add(new Label
                {
                    AutoSize = false,
                    Size = new Size(535, 65),
                    ForeColor = Color.Silver,
                    Text = comandos.Count == 0
                        ? "No hay comandos disponibles para este equipo y sesión."
                        : "No se encontraron comandos con esa búsqueda."
                });
            }

            flpComandosDisponibles.ResumeLayout();
        }

        // Una tarjeta = frase en negrita, descripción, etiqueta de confirmación y una línea abajo.
        private Panel CrearTarjetaComando(ComandoDisponible comando)
        {
            Panel tarjeta = new Panel
            {
                Size = new Size(535, 115),
                BackColor = Color.FromArgb(23, 30, 41),
                Margin = new Padding(0, 0, 0, 10)
            };

            tarjeta.Controls.Add(new Label
            {
                AutoSize = false,
                Location = new Point(0, 5),
                Size = new Size(530, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                Text = "“" + comando.Frase + "”"
            });

            tarjeta.Controls.Add(new Label
            {
                AutoSize = false,
                Location = new Point(0, 35),
                Size = new Size(530, 35),
                ForeColor = Color.FromArgb(167, 179, 198),
                Text = comando.Descripcion
            });

            // Amarillo si el comando pide confirmación; gris si no.
            tarjeta.Controls.Add(new Label
            {
                AutoSize = false,
                Location = new Point(0, 76),
                Size = new Size(250, 27),
                Font = new Font("Segoe UI", 9),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                Text = comando.Confirmacion,
                BackColor = comando.RequiereConfirmacion ? Color.FromArgb(48, 41, 30) : Color.FromArgb(32, 41, 56),
                ForeColor = comando.RequiereConfirmacion ? Color.FromArgb(255, 209, 128) : Color.FromArgb(198, 207, 221)
            });

            tarjeta.Controls.Add(new Panel
            {
                Location = new Point(0, 114),
                Size = new Size(535, 1),
                BackColor = Color.FromArgb(44, 54, 71)
            });

            return tarjeta;
        }
    }
}
