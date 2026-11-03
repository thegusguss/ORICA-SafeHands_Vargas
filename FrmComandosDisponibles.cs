using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    public partial class FrmComandosDisponibles : Form
    {
        private readonly List<ComandoDisponible> comandos =
            new List<ComandoDisponible>();

        public FrmComandosDisponibles()
        {
            InitializeComponent();
            txtBuscarComando.TextChanged += BuscarComando_TextChanged;

            btnEntendidoComandos.Click += (s, e) => Close();

            btnEntendidoComandos.DialogResult = DialogResult.OK;
            this.CancelButton = btnEntendidoComandos;

            // Datos temporales para comprobar el diseño.
            CargarComandos(
                "Luz indicadora · SIM-02 (demostración)",
                new List<ComandoDisponible>
                {
            new ComandoDisponible
            {
                Frase = "Consultar estado",
                Descripcion =
                    "Consulta si la luz está encendida o apagada.",
                Confirmacion = "Sin confirmación",
                RequiereConfirmacion = false
            },

            new ComandoDisponible
            {
                Frase = "Encender luz",
                Descripcion =
                    "Solicita el encendido del equipo seleccionado.",
                Confirmacion = "Requiere confirmación",
                RequiereConfirmacion = true
            },

            new ComandoDisponible
            {
                Frase = "Apagar luz",
                Descripcion =
                    "Solicita el apagado del equipo seleccionado.",
                Confirmacion = "Sin confirmación",
                RequiereConfirmacion = false
            }
                });
        }

        public void CargarComandos(
            string nombreEquipo,
            IEnumerable<ComandoDisponible> comandosPermitidos)
        {
            lblEquipoComandos.Text = "Equipo: " + nombreEquipo;

            comandos.Clear();

            if (comandosPermitidos != null)
            {
                comandos.AddRange(comandosPermitidos);
            }

            txtBuscarComando.Clear();
            MostrarComandos();
        }

        private void BuscarComando_TextChanged(object sender, EventArgs e)
        {
            MostrarComandos();
        }

        private void MostrarComandos()
        {
            flpComandosDisponibles.SuspendLayout();

            try
            {
                while (flpComandosDisponibles.Controls.Count > 0)
                {
                    Control control =
                        flpComandosDisponibles.Controls[0];

                    flpComandosDisponibles.Controls.Remove(control);
                    control.Dispose();
                }

                string busqueda = txtBuscarComando.Text.Trim();
                int encontrados = 0;

                foreach (ComandoDisponible comando in comandos)
                {
                    string texto =
                        comando.Frase + " " + comando.Descripcion;

                    bool coincide = texto.IndexOf(
                        busqueda,
                        StringComparison.CurrentCultureIgnoreCase) >= 0;

                    if (!coincide)
                    {
                        continue;
                    }

                    flpComandosDisponibles.Controls.Add(
                        CrearTarjetaComando(comando));

                    encontrados++;
                }

                if (encontrados == 0)
                {
                    Label mensaje = new Label
                    {
                        AutoSize = false,
                        Size = new Size(535, 65),
                        ForeColor = Color.Silver,
                        Text = comandos.Count == 0
                            ? "No hay comandos disponibles para este equipo y sesión."
                            : "No se encontraron comandos con esa búsqueda."
                    };

                    flpComandosDisponibles.Controls.Add(mensaje);
                }
            }
            finally
            {
                flpComandosDisponibles.ResumeLayout();
            }
        }

        private Panel CrearTarjetaComando(ComandoDisponible comando)
        {
            Panel tarjeta = new Panel
            {
                Size = new Size(535, 115),
                BackColor = Color.FromArgb(23, 30, 41),
                Margin = new Padding(0, 0, 0, 10)
            };

            Label titulo = new Label
            {
                AutoSize = false,
                Location = new Point(0, 5),
                Size = new Size(530, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                Text = "“" + comando.Frase + "”"
            };

            Label descripcion = new Label
            {
                AutoSize = false,
                Location = new Point(0, 35),
                Size = new Size(530, 35),
                ForeColor = Color.FromArgb(167, 179, 198),
                Text = comando.Descripcion
            };

            Label confirmacion = new Label
            {
                AutoSize = false,
                Location = new Point(0, 76),
                Size = new Size(250, 27),
                Font = new Font("Segoe UI", 9),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                Text = comando.Confirmacion
            };

            if (comando.RequiereConfirmacion)
            {
                confirmacion.BackColor =
                    Color.FromArgb(48, 41, 30);

                confirmacion.ForeColor =
                    Color.FromArgb(255, 209, 128);
            }
            else
            {
                confirmacion.BackColor =
                    Color.FromArgb(32, 41, 56);

                confirmacion.ForeColor =
                    Color.FromArgb(198, 207, 221);
            }

            Panel separador = new Panel
            {
                Location = new Point(0, 114),
                Size = new Size(535, 1),
                BackColor = Color.FromArgb(44, 54, 71)
            };

            tarjeta.Controls.Add(titulo);
            tarjeta.Controls.Add(descripcion);
            tarjeta.Controls.Add(confirmacion);
            tarjeta.Controls.Add(separador);

            return tarjeta;
        }
    }
}