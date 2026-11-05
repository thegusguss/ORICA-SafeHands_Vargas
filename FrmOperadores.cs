using ORICA.Negocio;
using ORICA.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    /// <summary>Lista de operadores: buscar, crear, editar y suspender/activar.</summary>
    public partial class FrmOperadores : Form
    {
        private readonly clsUsuarioNegocio usuarioNegocio = new clsUsuarioNegocio();

        public FrmOperadores()
        {
            InitializeComponent();

            colEditarOperador.Text = "Editar";  // texto fijo del botón de esa columna

            Load += (s, e) => CargarOperadores();
            txtBuscarOperador.TextChanged += (s, e) => CargarOperadores();
            btnNuevoOperador.Click += (s, e) => AbrirEditor(null);
            dgvOperadores.CellContentClick += dgvOperadores_CellContentClick;
        }

        // Llena la tabla. Si hay texto en el buscador, solo muestra los que coinciden.
        private void CargarOperadores()
        {
            List<Usuario> usuarios;
            try
            {
                usuarios = usuarioNegocio.mtdBuscarUsuarios(txtBuscarOperador.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar la lista de operadores.\n" + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgvOperadores.Rows.Clear();

            foreach (Usuario u in usuarios)
            {
                int fila = dgvOperadores.Rows.Add(
                    u.Codigo,
                    u.NombreCompleto,
                    u.RolNombre,
                    u.Credenciales ?? "Sin credencial",
                    u.EstadoTexto,
                    null,                                  // botón "Editar" (texto fijo)
                    u.Estado ? "Suspender" : "Activar");   // botón de estado

                // Se guarda el usuario completo en la fila para usarlo al hacer clic.
                dgvOperadores.Rows[fila].Tag = u;
            }

            dgvOperadores.ClearSelection();
        }

        // Clic en un botón de la tabla: "Editar" o "Suspender/Activar".
        private void dgvOperadores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;  // clic en el encabezado

            Usuario usuario = (Usuario)dgvOperadores.Rows[e.RowIndex].Tag;

            if (e.ColumnIndex == colEditarOperador.Index)
                AbrirEditor(usuario);
            else if (e.ColumnIndex == colCambiarEstadoOperador.Index)
                CambiarEstado(usuario);
        }

        // usuario = null → crear uno nuevo; si no, editar el que llega.
        private void AbrirEditor(Usuario usuario)
        {
            using (FrmOperadorEditar editor = new FrmOperadorEditar(usuario))
            {
                if (editor.ShowDialog(this) == DialogResult.OK)
                    CargarOperadores();
            }
        }

        private void CambiarEstado(Usuario usuario)
        {
            bool activar = !usuario.Estado;

            // Evita que el administrador se suspenda a sí mismo y quede sin acceso.
            if (!activar && Sesion.Activa && Sesion.UsuarioActual.IdUsuario == usuario.IdUsuario)
            {
                MessageBox.Show("No puede suspender su propia cuenta.", "Operadores",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string accion = activar ? "activar" : "suspender";

            DialogResult respuesta = MessageBox.Show(
                "¿Desea " + accion + " a " + usuario.NombreCompleto + "?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes) return;

            Resultado resultado = usuarioNegocio.mtdCambiarEstado(usuario.IdUsuario, activar);
            MostrarResultado(resultado);

            if (resultado.Exito)
                CargarOperadores();
        }

        private static void MostrarResultado(Resultado resultado)
        {
            MessageBox.Show(resultado.Mensaje, "Operadores", MessageBoxButtons.OK,
                resultado.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }
    }
}
