using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    // Lista de credenciales: registrar nuevas y activar / desactivar / desbloquear.
    public partial class Credenciales : Form
    {
        private readonly clsCredencialNegocio credencialNegocio = new clsCredencialNegocio();

        public Credenciales()
        {
            InitializeComponent();

            Load += (s, e) => CargarCredenciales();
            btnRegistrarCredencial.Click += btnRegistrarCredencial_Click;
            dgvCredenciales.CellContentClick += dgvCredenciales_CellContentClick;
        }

        private void CargarCredenciales()
        {
            List<Credencial> credenciales;
            try
            {
                credenciales = credencialNegocio.mtdListar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar la lista de credenciales.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgvCredenciales.Rows.Clear();

            foreach (Credencial c in credenciales)
            {
                int fila = dgvCredenciales.Rows.Add(
                    c.Codigo,
                    c.UsuarioCodigo + " - " + c.Operador,
                    c.Tipo,
                    c.Referencia,
                    c.FechaRegistro.HasValue ? c.FechaRegistro.Value.ToString("dd/MM/yyyy HH:mm") : "",
                    c.EstadoTexto,
                    TextoAccion(c));

                // Se guarda la credencial en la fila para usarla al hacer clic.
                dgvCredenciales.Rows[fila].Tag = c;
            }

            dgvCredenciales.ClearSelection();
        }

        // Texto del botón según el estado de la credencial.
        private static string TextoAccion(Credencial c)
        {
            if (c.Bloqueada) return "Desbloquear";
            return c.Estado ? "Desactivar" : "Activar";
        }

        private void btnRegistrarCredencial_Click(object sender, EventArgs e)
        {
            using (FrmCredencialRegistrar registro = new FrmCredencialRegistrar())
            {
                if (registro.ShowDialog(this) == DialogResult.OK)
                    CargarCredenciales();
            }
        }

        // Clic en el botón de la columna ACCIÓN.
        private void dgvCredenciales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colCambiarEstadoCredencial.Index) return;

            Credencial credencial = (Credencial)dgvCredenciales.Rows[e.RowIndex].Tag;
            string accion = TextoAccion(credencial).ToLower();

            DialogResult respuesta = MessageBox.Show(
                "¿Desea " + accion + " la credencial " + credencial.Tipo + " de " + credencial.Operador + "?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes) return;

            Resultado resultado = credencialNegocio.mtdCambiarEstado(credencial);
            MessageBox.Show(resultado.Mensaje, "Credenciales", MessageBoxButtons.OK,
                resultado.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (resultado.Exito)
                CargarCredenciales();
        }
    }
}
