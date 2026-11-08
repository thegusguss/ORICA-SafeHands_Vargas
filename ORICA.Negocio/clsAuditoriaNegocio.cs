using System;
using ORICA.Datos;

namespace ORICA.Negocio
{
    /// <summary>
    /// Registra cambios administrativos en dbo.Auditoria.
    /// Nunca lanza excepciones: si la auditoría falla, la operación principal no se cae.
    /// </summary>
    public static class clsAuditoriaNegocio
    {
        // Módulos (texto fijo para poder filtrar después)
        public const string ModuloOperadores = "OPERADORES";
        public const string ModuloCredenciales = "CREDENCIALES";
        public const string ModuloEquipos = "EQUIPOS";
        public const string ModuloComandos = "COMANDOS";
        public const string ModuloCriticidad = "CRITICIDAD";
        public const string ModuloConfiguracion = "CONFIGURACION";

        public static void mtdRegistrar(string modulo, string accion, string entidad, object idRegistro,
                                        string descripcion, string valorAnterior = null, string valorNuevo = null)
        {
            try
            {
                int? idUsuario = Sesion.Activa ? (int?)Sesion.UsuarioActual.IdUsuario : null;

                new clsAuditoriaDatos().mtdRegistrar(idUsuario, modulo, accion, entidad,
                    idRegistro == null ? null : Convert.ToString(idRegistro),
                    mtdRecortar(descripcion, 500), valorAnterior, valorNuevo, mtdRecortar(Environment.MachineName, 100));
            }
            catch (Exception)
            {
                // La auditoría es secundaria: no interrumpe la operación que ya se guardó.
            }
        }

        internal static string mtdRecortar(string texto, int maximo)
        {
            if (texto == null) return null;
            return texto.Length <= maximo ? texto : texto.Substring(0, maximo);
        }
    }
}
