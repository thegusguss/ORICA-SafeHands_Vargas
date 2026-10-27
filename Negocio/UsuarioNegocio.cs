using System;
using System.Collections.Generic;
using System.Net.Mail;
using PROYECTO_ORICA_SAFEHANDS_VARGAS.Datos;
using PROYECTO_ORICA_SAFEHANDS_VARGAS.Entidades;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS.Negocio
{
    public class UsuarioNegocio
    {
        private readonly UsuarioDatos usuarioDatos = new UsuarioDatos();

        public List<Usuario> Listar()
        {
            return usuarioDatos.Listar();
        }

        public List<Usuario> Buscar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return usuarioDatos.Listar();
            }

            return usuarioDatos.Buscar(texto.Trim());
        }

        public Usuario ObtenerPorId(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                throw new Exception("El usuario seleccionado no es válido.");
            }

            Usuario usuario = usuarioDatos.ObtenerPorId(idUsuario);

            if (usuario == null)
            {
                throw new Exception("No se encontró el usuario solicitado.");
            }

            return usuario;
        }

        public int Registrar(Usuario usuario)
        {
            ValidarUsuario(usuario);

            usuario.Codigo = usuario.Codigo.Trim().ToUpper();
            usuario.Nombres = usuario.Nombres.Trim();
            usuario.Apellidos = usuario.Apellidos.Trim();
            usuario.Documento = LimpiarTexto(usuario.Documento);
            usuario.Telefono = LimpiarTexto(usuario.Telefono);
            usuario.Correo = LimpiarTexto(usuario.Correo);

            if (!string.IsNullOrWhiteSpace(usuario.Correo))
            {
                usuario.Correo = usuario.Correo.ToLower();
            }

            usuario.Estado = true;

            return usuarioDatos.Registrar(usuario);
        }

        public bool Modificar(Usuario usuario)
        {
            if (usuario.IdUsuario <= 0)
            {
                throw new Exception("El usuario seleccionado no es válido.");
            }

            ValidarUsuario(usuario);

            usuario.Codigo = usuario.Codigo.Trim().ToUpper();
            usuario.Nombres = usuario.Nombres.Trim();
            usuario.Apellidos = usuario.Apellidos.Trim();
            usuario.Documento = LimpiarTexto(usuario.Documento);
            usuario.Telefono = LimpiarTexto(usuario.Telefono);
            usuario.Correo = LimpiarTexto(usuario.Correo);

            if (!string.IsNullOrWhiteSpace(usuario.Correo))
            {
                usuario.Correo = usuario.Correo.ToLower();
            }

            return usuarioDatos.Modificar(usuario);
        }

        public bool CambiarEstado(int idUsuario, bool estado)
        {
            if (idUsuario <= 0)
            {
                throw new Exception("El usuario seleccionado no es válido.");
            }

            return usuarioDatos.CambiarEstado(idUsuario, estado);
        }

        private void ValidarUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new Exception("Los datos del usuario son obligatorios.");
            }

            if (string.IsNullOrWhiteSpace(usuario.Codigo))
            {
                throw new Exception("El código del usuario es obligatorio.");
            }

            if (usuario.Codigo.Trim().Length > 15)
            {
                throw new Exception("El código no puede superar los 15 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(usuario.Nombres))
            {
                throw new Exception("Los nombres son obligatorios.");
            }

            if (usuario.Nombres.Trim().Length > 80)
            {
                throw new Exception("Los nombres no pueden superar los 80 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(usuario.Apellidos))
            {
                throw new Exception("Los apellidos son obligatorios.");
            }

            if (usuario.Apellidos.Trim().Length > 100)
            {
                throw new Exception("Los apellidos no pueden superar los 100 caracteres.");
            }

            if (usuario.IdRol <= 0)
            {
                throw new Exception("Debe seleccionar un rol.");
            }

            if (!string.IsNullOrWhiteSpace(usuario.Documento) &&
                usuario.Documento.Trim().Length > 20)
            {
                throw new Exception("El documento no puede superar los 20 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(usuario.Telefono) &&
                usuario.Telefono.Trim().Length > 20)
            {
                throw new Exception("El teléfono no puede superar los 20 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(usuario.Correo))
            {
                if (usuario.Correo.Trim().Length > 120)
                {
                    throw new Exception("El correo no puede superar los 120 caracteres.");
                }

                if (!CorreoValido(usuario.Correo))
                {
                    throw new Exception("El formato del correo no es válido.");
                }
            }
        }

        private bool CorreoValido(string correo)
        {
            try
            {
                MailAddress direccion = new MailAddress(correo);
                return direccion.Address == correo.Trim();
            }
            catch
            {
                return false;
            }
        }

        private string LimpiarTexto(string texto)
        {
            return string.IsNullOrWhiteSpace(texto)
                ? null
                : texto.Trim();
        }
    }
}