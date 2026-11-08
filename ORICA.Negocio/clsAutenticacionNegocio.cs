using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ORICA.Datos;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    public class clsAutenticacionNegocio
    {
        private const int LongitudPin = 4;
        internal const int MaxIntentosFallidos = 5;  // también lo usa clsCredencialNegocio
        private const string MetodoPin = "PIN";

        private readonly clsAutenticacionDatos _datos = new clsAutenticacionDatos();

        // ================= CONSULTAS =================

        //Operadores que pueden iniciar sesión con PIN.
        public List<Usuario> mtdListarOperadores()
        {
            List<Usuario> lista = new List<Usuario>();

            foreach (DataRow r in _datos.mtdListarUsuariosConPin().Rows)
            {
                lista.Add(new Usuario
                {
                    IdUsuario = clsLectura.mtdLeerEntero(r, "IdUsuario"),
                    Codigo = clsLectura.mtdLeerTexto(r, "Codigo"),
                    Nombres = clsLectura.mtdLeerTexto(r, "Nombres"),
                    Apellidos = clsLectura.mtdLeerTexto(r, "Apellidos"),
                    IdRol = clsLectura.mtdLeerEntero(r, "IdRol"),
                    RolCodigo = clsLectura.mtdLeerTexto(r, "RolCodigo"),
                    RolNombre = clsLectura.mtdLeerTexto(r, "RolNombre"),
                    Estado = clsLectura.mtdLeerBooleano(r, "Estado")
                });
            }

            return lista;
        }

        // ================= INICIO / CIERRE DE SESIÓN =================

        public Resultado mtdIniciarSesionConPin(Usuario usuario, string pin)
        {
            try
            {
                if (usuario == null || usuario.IdUsuario <= 0)
                    return Resultado.Error("Seleccione un operador.");

                pin = (pin ?? string.Empty).Trim();
                string errorPin = mtdValidarPin(pin);
                if (errorPin != null)
                    return Resultado.Error(errorPin);

                DataTable credenciales = _datos.mtdObtenerCredencialPin(usuario.IdUsuario);
                if (credenciales.Rows.Count == 0)
                    return Resultado.Error("El operador no tiene una credencial PIN activa.");

                DataRow credencial = credenciales.Rows[0];
                int idCredencial = clsLectura.mtdLeerEntero(credencial, "IdCredencial");
                int intentosFallidos = clsLectura.mtdLeerEntero(credencial, "IntentosFallidos");

                if (intentosFallidos >= MaxIntentosFallidos)
                {
                    new clsEventoNegocio().mtdRegistrar(clsEventoNegocio.TipoAccesoFallido, clsEventoNegocio.ResultadoRechazado,
                        "Intento con credencial PIN bloqueada.", idUsuario: usuario.IdUsuario);
                    return Resultado.Error("La credencial está bloqueada por intentos fallidos. Contacte al administrador.");
                }

                byte[] hashGuardado = clsLectura.mtdLeerBytes(credencial, "HashCredencial");
                byte[] salt = clsLectura.mtdLeerBytes(credencial, "Salt");

                if (!mtdPinCoincide(pin, salt, hashGuardado))
                {
                    int intentos = _datos.mtdRegistrarIntentoFallido(idCredencial);
                    int restantes = MaxIntentosFallidos - intentos;

                    new clsEventoNegocio().mtdRegistrar(clsEventoNegocio.TipoAccesoFallido, clsEventoNegocio.ResultadoRechazado,
                        restantes > 0 ? "PIN incorrecto (" + intentos + " de " + MaxIntentosFallidos + ")." : "PIN incorrecto. Credencial bloqueada.",
                        idUsuario: usuario.IdUsuario);

                    return restantes > 0
                        ? Resultado.Error($"El PIN es incorrecto. Intentos restantes: {restantes}.")
                        : Resultado.Error("El PIN es incorrecto. La credencial quedó bloqueada.");
                }

                Guid idSesion = _datos.mtdRegistrarAccesoExitoso(idCredencial, usuario.IdUsuario,
                                                              MetodoPin, Environment.MachineName);
                Sesion.Iniciar(usuario, idSesion);

                new clsEventoNegocio().mtdRegistrar(clsEventoNegocio.TipoInicioSesion, clsEventoNegocio.ResultadoInformativo,
                    "Ingreso con PIN desde " + Environment.MachineName + ".");

                return Resultado.Ok($"Bienvenido, {usuario.NombreCompleto}.", usuario.IdUsuario);
            }
            catch (SqlException ex) { return Resultado.Error("Error de base de datos: " + ex.Message); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        public void mtdCerrarSesion()
        {
            try
            {
                if (Sesion.Activa)
                    new clsEventoNegocio().mtdRegistrar(clsEventoNegocio.TipoCierreSesion, clsEventoNegocio.ResultadoInformativo,
                        "Sesión cerrada.");

                if (Sesion.IdSesion != Guid.Empty) _datos.mtdCerrarSesion(Sesion.IdSesion);
            }
            catch (Exception)
            {
                // Si la BD no responde al salir, la sesión queda ABIERTA; no se bloquea el cierre.
            }
            finally
            {
                Sesion.Limpiar();
            }
        }

        // ================= REGLAS =================

        // Único lugar donde se valida el formato del PIN (el login ya no lo repite).
        internal static string mtdValidarPin(string pin)
        {
            if (pin.Length == 0)
                return "Ingrese su PIN.";

            if (pin.Length != LongitudPin || !pin.All(char.IsDigit))
                return $"El PIN debe tener {LongitudPin} números.";

            return null;
        }

        /// <summary>
        /// Mismo cálculo que HASHBYTES('SHA2_256', ...) en SQL Server sobre un VARCHAR.
        /// Si la credencial tiene Salt, se antepone al PIN antes de calcular el hash.
        /// </summary>
        private static bool mtdPinCoincide(string pin, byte[] salt, byte[] hashGuardado)
        {
            if (hashGuardado == null) return false;
            return mtdCalcularHash(pin, salt).SequenceEqual(hashGuardado);
        }

        // Hash SHA-256 de (salt + texto). Se usa al iniciar sesión y al registrar credenciales,
        // por eso los dos lados siempre calculan igual.
        internal static byte[] mtdCalcularHash(string texto, byte[] salt)
        {
            byte[] textoBytes = Encoding.ASCII.GetBytes(texto);
            byte[] datos = salt == null ? textoBytes : salt.Concat(textoBytes).ToArray();

            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(datos);
            }
        }
    }
}
