using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using ORICA.Datos;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    public class UsuarioNegocio
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

        private readonly UsuarioDatos _datos = new UsuarioDatos();
        private readonly RolNegocio _rolNegocio = new RolNegocio();

        // ================= CONSULTAS =================

        public List<Usuario> Listar()
        {
            return MapearLista(_datos.Listar());
        }

        public List<Usuario> Buscar(string texto)
        {
            texto = (texto ?? string.Empty).Trim();
            return texto.Length == 0 ? Listar() : MapearLista(_datos.Buscar(texto));
        }

        public Usuario ObtenerPorId(int idUsuario)
        {
            if (idUsuario <= 0) return null;
            DataTable dt = _datos.ObtenerPorId(idUsuario);
            return dt.Rows.Count == 0 ? null : Mapear(dt.Rows[0]);
        }

        // ================= ESCRITURA =================

        public Resultado Registrar(Usuario u)
        {
            try
            {
                if (u == null) return Resultado.Error("No se recibieron datos del usuario.");

                Normalizar(u);
                string error = Validar(u, null);
                if (error != null) return Resultado.Error(error);

                int id = _datos.Registrar(u.Codigo, u.Nombres, u.Apellidos, u.Documento,
                                          u.Telefono, u.Correo, u.IdRol, true);

                return Resultado.Ok($"Usuario {u.Codigo} registrado correctamente.", id);
            }
            catch (SqlException ex) { return Resultado.Error(TraducirError(ex)); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        public Resultado Modificar(Usuario u)
        {
            try
            {
                if (u == null || u.IdUsuario <= 0)
                    return Resultado.Error("Seleccione un usuario válido para modificar.");

                if (u.RowVersion == null)
                    return Resultado.Error("No se pudo verificar la versión del registro. Actualice la lista e intente nuevamente.");

                Normalizar(u);
                string error = Validar(u, u.IdUsuario);
                if (error != null) return Resultado.Error(error);

                int filas = _datos.Modificar(u.IdUsuario, u.Codigo, u.Nombres, u.Apellidos,
                                             u.Documento, u.Telefono, u.Correo, u.IdRol, u.RowVersion);

                if (filas == 0)
                    return Resultado.Error("El usuario fue modificado por otra persona o ya no existe. Actualice la lista e intente nuevamente.");

                return Resultado.Ok($"Usuario {u.Codigo} modificado correctamente.", u.IdUsuario);
            }
            catch (SqlException ex) { return Resultado.Error(TraducirError(ex)); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        public Resultado CambiarEstado(int idUsuario, bool activo)
        {
            try
            {
                if (idUsuario <= 0) return Resultado.Error("Seleccione un usuario válido.");

                int filas = _datos.CambiarEstado(idUsuario, activo);
                if (filas == 0) return Resultado.Error("El usuario ya no existe.");

                return Resultado.Ok(activo ? "Usuario activado correctamente." : "Usuario suspendido correctamente.", idUsuario);
            }
            catch (SqlException ex) { return Resultado.Error(TraducirError(ex)); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        // ================= REGLAS =================

        private static void Normalizar(Usuario u)
        {
            u.Codigo = (u.Codigo ?? string.Empty).Trim().ToUpperInvariant();
            u.Nombres = (u.Nombres ?? string.Empty).Trim();
            u.Apellidos = (u.Apellidos ?? string.Empty).Trim();
            u.Documento = VacioANulo(u.Documento);
            u.Telefono = VacioANulo(u.Telefono);
            u.Correo = VacioANulo(u.Correo);
            if (u.Correo != null) u.Correo = u.Correo.ToLowerInvariant();
        }

        /// <summary>Devuelve null si todo es válido, o el mensaje del primer error.</summary>
        private string Validar(Usuario u, int? idExcluir)
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
            if (!_rolNegocio.EsRolValido(u.IdRol)) return "El rol seleccionado no es válido o está inactivo.";

            // Duplicados
            if (_datos.ExisteCodigo(u.Codigo, idExcluir))
                return $"El código {u.Codigo} ya se encuentra registrado.";
            if (u.Documento != null && _datos.ExisteDocumento(u.Documento, idExcluir))
                return $"El documento {u.Documento} ya se encuentra registrado.";
            if (u.Correo != null && _datos.ExisteCorreo(u.Correo, idExcluir))
                return $"El correo {u.Correo} ya se encuentra registrado.";

            return null;
        }

        private static string TraducirError(SqlException ex)
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

        private static string VacioANulo(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
        }

        // ================= MAPEO =================

        private static List<Usuario> MapearLista(DataTable dt)
        {
            List<Usuario> lista = new List<Usuario>();
            foreach (DataRow r in dt.Rows) lista.Add(Mapear(r));
            return lista;
        }

        private static Usuario Mapear(DataRow r)
        {
            return new Usuario
            {
                IdUsuario = Lectura.Entero(r, "IdUsuario"),
                Codigo = Lectura.Texto(r, "Codigo"),
                Nombres = Lectura.Texto(r, "Nombres"),
                Apellidos = Lectura.Texto(r, "Apellidos"),
                Documento = Lectura.Texto(r, "Documento"),
                Telefono = Lectura.Texto(r, "Telefono"),
                Correo = Lectura.Texto(r, "Correo"),
                IdRol = Lectura.Entero(r, "IdRol"),
                RolNombre = Lectura.Texto(r, "RolNombre"),
                Estado = Lectura.Booleano(r, "Estado"),
                FechaRegistro = Lectura.Fecha(r, "FechaRegistro"),
                FechaModificacion = Lectura.Fecha(r, "FechaModificacion"),
                UltimoAcceso = Lectura.Fecha(r, "UltimoAcceso"),
                RowVersion = Lectura.Bytes(r, "RowVersion")
            };
        }
    }
}
