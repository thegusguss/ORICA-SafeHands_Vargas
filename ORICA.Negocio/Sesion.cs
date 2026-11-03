using System;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    /// <summary>
    /// Usuario autenticado en la aplicación. Se llena al iniciar sesión
    /// y los formularios lo consultan para saber quién está operando.
    /// </summary>
    public static class Sesion
    {
        public static Usuario UsuarioActual { get; private set; }
        public static Guid IdSesion { get; private set; }

        public static bool Activa
        {
            get { return UsuarioActual != null; }
        }

        internal static void Iniciar(Usuario usuario, Guid idSesion)
        {
            UsuarioActual = usuario;
            IdSesion = idSesion;
        }

        internal static void Limpiar()
        {
            UsuarioActual = null;
            IdSesion = Guid.Empty;
        }
    }
}
