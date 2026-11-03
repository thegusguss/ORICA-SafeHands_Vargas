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
    public class AutenticacionNegocio
    {
        private const int LongitudPin = 4;
        private const int MaxIntentosFallidos = 5;
        private const string MetodoPin = "PIN";

        private readonly AutenticacionDatos _datos = new AutenticacionDatos();

        // ================= CONSULTAS =================

        /// <summary>Operadores que pueden iniciar sesión con PIN.</summary>
        public List<Usuario> ListarOperadores()
        {
            List<Usuario> lista = new List<Usuario>();

            foreach (DataRow r in _datos.ListarUsuariosConPin().Rows)
            {
                lista.Add(new Usuario
                {
                    IdUsuario = Lectura.Entero(r, "IdUsuario"),
                    Codigo = Lectura.Texto(r, "Codigo"),
                    Nombres = Lectura.Texto(r, "Nombres"),
                    Apellidos = Lectura.Texto(r, "Apellidos"),
                    IdRol = Lectura.Entero(r, "IdRol"),
                    RolCodigo = Lectura.Texto(r, "RolCodigo"),
                    RolNombre = Lectura.Texto(r, "RolNombre"),
                    Estado = Lectura.Booleano(r, "Estado")
                });
            }

            return lista;
        }

        // ================= INICIO / CIERRE DE SESIÓN =================

        public Resultado IngresarConPin(Usuario usuario, string pin)
        {
            try
            {
                if (usuario == null || usuario.IdUsuario <= 0)
                    return Resultado.Error("Seleccione un operador.");

                pin = (pin ?? string.Empty).Trim();
                if (pin.Length == 0) return Resultado.Error("Ingrese su PIN.");
                if (pin.Length != LongitudPin || !pin.All(char.IsDigit))
                    return Resultado.Error($"El PIN debe tener {LongitudPin} números.");

                DataTable dt = _datos.ObtenerCredencialPin(usuario.IdUsuario);
                if (dt.Rows.Count == 0)
                    return Resultado.Error("El operador no tiene una credencial PIN activa.");

                DataRow c = dt.Rows[0];
                int idCredencial = Lectura.Entero(c, "IdCredencial");

                if (Lectura.Entero(c, "IntentosFallidos") >= MaxIntentosFallidos)
                    return Resultado.Error("La credencial está bloqueada por intentos fallidos. Contacte al administrador.");

                byte[] hashGuardado = Lectura.Bytes(c, "HashCredencial");
                byte[] salt = Lectura.Bytes(c, "Salt");

                if (!PinCoincide(pin, salt, hashGuardado))
                {
                    int intentos = _datos.RegistrarIntentoFallido(idCredencial);
                    int restantes = MaxIntentosFallidos - intentos;

                    return restantes > 0
                        ? Resultado.Error($"El PIN es incorrecto. Intentos restantes: {restantes}.")
                        : Resultado.Error("El PIN es incorrecto. La credencial quedó bloqueada.");
                }

                Guid idSesion = _datos.RegistrarAccesoExitoso(idCredencial, usuario.IdUsuario,
                                                              MetodoPin, Environment.MachineName);
                Sesion.Iniciar(usuario, idSesion);

                return Resultado.Ok($"Bienvenido, {usuario.NombreCompleto}.", usuario.IdUsuario);
            }
            catch (SqlException ex) { return Resultado.Error("Error de base de datos: " + ex.Message); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        public void CerrarSesion()
        {
            try
            {
                if (Sesion.IdSesion != Guid.Empty) _datos.CerrarSesion(Sesion.IdSesion);
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

        /// <summary>
        /// Mismo cálculo que HASHBYTES('SHA2_256', ...) en SQL Server sobre un VARCHAR.
        /// Si la credencial tiene Salt, se antepone al PIN antes de calcular el hash.
        /// </summary>
        private static bool PinCoincide(string pin, byte[] salt, byte[] hashGuardado)
        {
            if (hashGuardado == null) return false;

            byte[] pinBytes = Encoding.ASCII.GetBytes(pin);
            byte[] datos = salt == null ? pinBytes : salt.Concat(pinBytes).ToArray();

            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(datos).SequenceEqual(hashGuardado);
            }
        }
    }
}
