using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using ORICA.Datos;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    public class clsCredencialNegocio
    {
        private readonly clsCredencialDatos _datos = new clsCredencialDatos();


        public List<Credencial> mtdListar()
        {
            List<Credencial> lista = new List<Credencial>();

            foreach (DataRow r in _datos.mtdListar().Rows)
            {
                lista.Add(new Credencial
                {
                    IdCredencial = clsLectura.mtdLeerEntero(r, "IdCredencial"),
                    Codigo = clsLectura.mtdLeerTexto(r, "Codigo"),
                    IdUsuario = clsLectura.mtdLeerEntero(r, "IdUsuario"),
                    IdTipoCredencial = clsLectura.mtdLeerEntero(r, "IdTipoCredencial"),
                    UsuarioCodigo = clsLectura.mtdLeerTexto(r, "UsuarioCodigo"),
                    Operador = clsLectura.mtdLeerTexto(r, "Operador"),
                    Tipo = clsLectura.mtdLeerTexto(r, "Tipo"),
                    Referencia = clsLectura.mtdLeerTexto(r, "ReferenciaVisible"),
                    FechaRegistro = clsLectura.mtdLeerFecha(r, "FechaRegistro"),
                    Estado = clsLectura.mtdLeerBooleano(r, "Estado"),
                    IntentosFallidos = clsLectura.mtdLeerEntero(r, "IntentosFallidos")
                });
            }

            return lista;
        }

        public List<TipoCredencial> mtdListarTipos()
        {
            List<TipoCredencial> lista = new List<TipoCredencial>();

            foreach (DataRow r in _datos.mtdListarTipos().Rows)
            {
                lista.Add(new TipoCredencial
                {
                    IdTipoCredencial = clsLectura.mtdLeerEntero(r, "IdTipoCredencial"),
                    Codigo = clsLectura.mtdLeerTexto(r, "Codigo"),
                    Nombre = clsLectura.mtdLeerTexto(r, "Nombre"),
                    RequiereSalt = clsLectura.mtdLeerBooleano(r, "RequiereSalt")
                });
            }

            return lista;
        }


        // valor: el PIN o el UID de la tarjeta. confirmacion: solo se revisa si es PIN.
        // Si el usuario ya tenía una credencial activa de ese tipo, la anterior se desactiva.
        public Resultado mtdRegistrar(int idUsuario, TipoCredencial tipo, string valor, string confirmacion)
        {
            try
            {
                if (idUsuario <= 0) return Resultado.Error("Seleccione un operador.");
                if (tipo == null) return Resultado.Error("Seleccione el tipo de credencial.");

                valor = (valor ?? string.Empty).Trim().ToUpperInvariant();

                string error = tipo.EsPin
                    ? mtdValidarNuevoPin(valor, confirmacion)
                    : mtdValidarUid(valor);
                if (error != null) return Resultado.Error(error);

                // El valor nunca se guarda: solo su hash. El salt (si el tipo lo pide) hace
                // que dos PIN iguales tengan hash distinto.
                byte[] salt = tipo.RequiereSalt ? mtdGenerarSalt() : null;
                byte[] hash = clsAutenticacionNegocio.mtdCalcularHash(valor, salt);

                // Una misma tarjeta no puede estar asignada a dos usuarios.
                if (!tipo.EsPin && _datos.mtdExisteHash(tipo.IdTipoCredencial, hash))
                    return Resultado.Error("Esa tarjeta ya está asignada a otro usuario.");

                bool reemplaza = _datos.mtdTieneActiva(idUsuario, tipo.IdTipoCredencial);

                // Código único, por ejemplo CRD-261105153012345 (fecha y hora con milisegundos).
                string codigo = "CRD-" + DateTime.Now.ToString("yyMMddHHmmssfff");
                string referencia = tipo.EsPin ? "PIN ****" : "UID ****" + valor.Substring(valor.Length - 4);

                int id = _datos.mtdRegistrar(codigo, idUsuario, tipo.IdTipoCredencial, hash, salt, referencia);

                clsAuditoriaNegocio.mtdRegistrar(clsAuditoriaNegocio.ModuloCredenciales, "REGISTRAR", "Credencial", id,
                    "Credencial " + tipo.Nombre + " " + codigo + " registrada para el usuario " + idUsuario +
                    (reemplaza ? " (reemplaza a la anterior)." : "."));

                string mensaje = "Credencial " + tipo.Nombre + " registrada correctamente.";
                if (reemplaza) mensaje += "\nLa credencial " + tipo.Nombre + " anterior quedó desactivada.";

                return Resultado.Ok(mensaje, id);
            }
            catch (SqlException ex) { return Resultado.Error("Error de base de datos: " + ex.Message); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        // Un solo botón por fila: si está bloqueada la desbloquea; si no, la activa o desactiva.
        public Resultado mtdCambiarEstado(Credencial c)
        {
            try
            {
                if (c == null) return Resultado.Error("Seleccione una credencial.");

                if (c.Bloqueada)
                {
                    _datos.mtdDesbloquear(c.IdCredencial);
                    clsAuditoriaNegocio.mtdRegistrar(clsAuditoriaNegocio.ModuloCredenciales, "DESBLOQUEAR", "Credencial",
                        c.IdCredencial, "Credencial " + c.Tipo + " de " + c.Operador + " desbloqueada.");
                    return Resultado.Ok("Credencial desbloqueada. El operador ya puede volver a ingresar.");
                }

                if (c.Estado)
                {
                    _datos.mtdCambiarEstado(c.IdCredencial, false);
                    clsAuditoriaNegocio.mtdRegistrar(clsAuditoriaNegocio.ModuloCredenciales, "DESACTIVAR", "Credencial",
                        c.IdCredencial, "Credencial " + c.Tipo + " de " + c.Operador + " desactivada.");
                    return Resultado.Ok("Credencial desactivada.");
                }

                if (_datos.mtdTieneActiva(c.IdUsuario, c.IdTipoCredencial))
                    return Resultado.Error("El operador ya tiene otra credencial " + c.Tipo +
                                           " activa. Desactívela primero.");

                _datos.mtdCambiarEstado(c.IdCredencial, true);
                clsAuditoriaNegocio.mtdRegistrar(clsAuditoriaNegocio.ModuloCredenciales, "ACTIVAR", "Credencial",
                    c.IdCredencial, "Credencial " + c.Tipo + " de " + c.Operador + " activada.");
                return Resultado.Ok("Credencial activada.");
            }
            catch (SqlException ex) { return Resultado.Error("Error de base de datos: " + ex.Message); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        // ================= REGLAS =================

        private static string mtdValidarNuevoPin(string pin, string confirmacion)
        {
            string error = clsAutenticacionNegocio.mtdValidarPin(pin);  // la misma regla del login
            if (error != null) return error;

            if (pin != (confirmacion ?? string.Empty).Trim())
                return "Los PIN no coinciden.";

            return null;
        }

        private static string mtdValidarUid(string uid)
        {
            if (uid.Length == 0) return "Ingrese el UID de la tarjeta.";
            if (uid.Length < 4 || uid.Length > 30) return "El UID debe tener entre 4 y 30 caracteres.";
            if (!uid.All(char.IsLetterOrDigit)) return "El UID solo puede tener letras y números.";
            return null;
        }

        // 16 bytes aleatorios seguros (RNGCryptoServiceProvider es el generador criptográfico de .NET).
        private static byte[] mtdGenerarSalt()
        {
            byte[] salt = new byte[16];
            using (RNGCryptoServiceProvider generador = new RNGCryptoServiceProvider())
            {
                generador.GetBytes(salt);
            }
            return salt;
        }
    }
}
