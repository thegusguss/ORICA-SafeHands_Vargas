using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using ORICA.Datos;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    public class clsUsuarioNegocio
    {
        // Ajustar a las longitudes reales de la tabla dbo.Usuario
        private const int MaxCodigo = 15;
        private const int MaxNombres = 80;
        private const int MaxApellidos = 100;
        private const int MaxDocumento = 20;
        private const int MaxTelefono = 20;
        private const int MaxCorreo = 120;

        private static readonly Regex RegexCorreo =
            new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        private readonly clsUsuarioDatos _datos = new clsUsuarioDatos();
        private readonly clsRolNegocio _rolNegocio = new clsRolNegocio();

        // ================= CONSULTAS =================

        public List<Usuario> mtdListarUsuarios()
        {
            return mtdLeerUsuarios(_datos.mtdListarUsuarios());
        }

        public List<Usuario> mtdBuscarUsuarios(string texto)
        {
            texto = (texto ?? string.Empty).Trim();
            return texto.Length == 0 ? mtdListarUsuarios() : mtdLeerUsuarios(_datos.mtdBuscarUsuarios(texto));
        }

        public Usuario mtdObtenerPorId(int idUsuario)
        {
            if (idUsuario <= 0) return null;
            DataTable dt = _datos.mtdObtenerPorId(idUsuario);
            return dt.Rows.Count == 0 ? null : mtdLeerUsuario(dt.Rows[0]);
        }

        // ================= ESCRITURA =================

        public Resultado mtdRegistrar(Usuario u)
        {
            try
            {
                if (u == null) return Resultado.Error("No se recibieron datos del usuario.");

                mtdNormalizarUsuario(u);
                string error = mtdValidarUsuario(u, null);
                if (error != null) return Resultado.Error(error);

                int id = _datos.mtdRegistrar(u.Codigo, u.Nombres, u.Apellidos, u.Documento,
                                          u.Telefono, u.Correo, u.IdRol, true);

                return Resultado.Ok($"Usuario {u.Codigo} registrado correctamente.", id);
            }
            catch (SqlException ex) { return Resultado.Error(mtdTraducirError(ex)); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        public Resultado mtdModificar(Usuario u)
        {
            try
            {
                if (u == null || u.IdUsuario <= 0)
                    return Resultado.Error("Seleccione un usuario válido para modificar.");

                if (u.RowVersion == null)
                    return Resultado.Error("No se pudo verificar la versión del registro. Actualice la lista e intente nuevamente.");

                mtdNormalizarUsuario(u);
                string error = mtdValidarUsuario(u, u.IdUsuario);
                if (error != null) return Resultado.Error(error);

                int filas = _datos.mtdModificar(u.IdUsuario, u.Codigo, u.Nombres, u.Apellidos,
                                             u.Documento, u.Telefono, u.Correo, u.IdRol, u.RowVersion);

                if (filas == 0)
                    return Resultado.Error("El usuario fue modificado por otra persona o ya no existe. Actualice la lista e intente nuevamente.");

                return Resultado.Ok($"Usuario {u.Codigo} modificado correctamente.", u.IdUsuario);
            }
            catch (SqlException ex) { return Resultado.Error(mtdTraducirError(ex)); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        public Resultado mtdCambiarEstado(int idUsuario, bool activo)
        {
            try
            {
                if (idUsuario <= 0) return Resultado.Error("Seleccione un usuario válido.");

                int filas = _datos.mtdCambiarEstado(idUsuario, activo);
                if (filas == 0) return Resultado.Error("El usuario ya no existe.");

                return Resultado.Ok(activo ? "Usuario activado correctamente." : "Usuario suspendido correctamente.", idUsuario);
            }
            catch (SqlException ex) { return Resultado.Error(mtdTraducirError(ex)); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        // ================= REGLAS =================

        private static void mtdNormalizarUsuario(Usuario u)
        {
            u.Codigo = (u.Codigo ?? string.Empty).Trim().ToUpperInvariant();
            u.Nombres = (u.Nombres ?? string.Empty).Trim();
            u.Apellidos = (u.Apellidos ?? string.Empty).Trim();
            u.Documento = mtdVacioANulo(u.Documento);
            u.Telefono = mtdVacioANulo(u.Telefono);
            u.Correo = mtdVacioANulo(u.Correo);
            if (u.Correo != null) u.Correo = u.Correo.ToLowerInvariant();
        }

        /// <summary>Devuelve null si todo es válido, o el mensaje del primer error.</summary>
        private string mtdValidarUsuario(Usuario u, int? idExcluir)
        {
            // Obligatorios
            if (u.Codigo.Length == 0) return "El código es obligatorio.";
            if (u.Nombres.Length == 0) return "Los nombres son obligatorios.";
            if (u.Apellidos.Length == 0) return "Los apellidos son obligatorios.";

            // Longitudes
            if (u.Codigo.Length > MaxCodigo) return $"El código no puede superar {MaxCodigo} caracteres.";
            if (u.Nombres.Length > MaxNombres) return $"Los nombres no pueden superar {MaxNombres} caracteres.";
            if (u.Apellidos.Length > MaxApellidos) return $"Los apellidos no pueden superar {MaxApellidos} caracteres.";
            if (u.Documento != null && u.Documento.Length > MaxDocumento) return $"El documento no puede superar {MaxDocumento} caracteres.";
            if (u.Telefono != null && u.Telefono.Length > MaxTelefono) return $"El teléfono no puede superar {MaxTelefono} caracteres.";
            if (u.Correo != null && u.Correo.Length > MaxCorreo) return $"El correo no puede superar {MaxCorreo} caracteres.";

            // Formato
            if (u.Correo != null && !RegexCorreo.IsMatch(u.Correo)) return "El correo no tiene un formato válido.";

            // Rol
            if (u.IdRol <= 0) return "Seleccione un rol.";
            if (!_rolNegocio.mtdEsRolValido(u.IdRol)) return "El rol seleccionado no es válido o está inactivo.";

            // Duplicados
            if (_datos.mtdExisteCodigo(u.Codigo, idExcluir))
                return $"El código {u.Codigo} ya se encuentra registrado.";
            if (u.Documento != null && _datos.mtdExisteDocumento(u.Documento, idExcluir))
                return $"El documento {u.Documento} ya se encuentra registrado.";
            if (u.Correo != null && _datos.mtdExisteCorreo(u.Correo, idExcluir))
                return $"El correo {u.Correo} ya se encuentra registrado.";

            return null;
        }

        private static string mtdTraducirError(SqlException ex)
        {
            switch (ex.Number)
            {
                case 2627:
                case 2601:
                    return "Ya existe un usuario con el mismo código, documento o correo.";
                case 547:
                    return "El rol seleccionado no existe en la base de datos.";
                case -2:
                    return "La base de datos tardó demasiado en responder. Intente nuevamente.";
                default:
                    return "Error de base de datos: " + ex.Message;
            }
        }

        private static string mtdVacioANulo(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
        }

        // ================= MAPEO =================

        private static List<Usuario> mtdLeerUsuarios(DataTable dt)
        {
            List<Usuario> lista = new List<Usuario>();
            foreach (DataRow r in dt.Rows) lista.Add(mtdLeerUsuario(r));
            return lista;
        }

        private static Usuario mtdLeerUsuario(DataRow r)
        {
            return new Usuario
            {
                IdUsuario = clsLectura.mtdLeerEntero(r, "IdUsuario"),
                Codigo = clsLectura.mtdLeerTexto(r, "Codigo"),
                Nombres = clsLectura.mtdLeerTexto(r, "Nombres"),
                Apellidos = clsLectura.mtdLeerTexto(r, "Apellidos"),
                Documento = clsLectura.mtdLeerTexto(r, "Documento"),
                Telefono = clsLectura.mtdLeerTexto(r, "Telefono"),
                Correo = clsLectura.mtdLeerTexto(r, "Correo"),
                IdRol = clsLectura.mtdLeerEntero(r, "IdRol"),
                RolNombre = clsLectura.mtdLeerTexto(r, "RolNombre"),
                Estado = clsLectura.mtdLeerBooleano(r, "Estado"),
                FechaRegistro = clsLectura.mtdLeerFecha(r, "FechaRegistro"),
                FechaModificacion = clsLectura.mtdLeerFecha(r, "FechaModificacion"),
                UltimoAcceso = clsLectura.mtdLeerFecha(r, "UltimoAcceso"),
                RowVersion = clsLectura.mtdLeerBytes(r, "RowVersion"),
                Credenciales = clsLectura.mtdLeerTexto(r, "Credenciales")
            };
        }
    }
}
