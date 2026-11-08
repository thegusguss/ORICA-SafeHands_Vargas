using System;

namespace ORICA.Negocio
{
    // Dice qué módulos del menú puede abrir cada rol.
    // ADMIN ve todo; OPE usa su propia ventana (FrmPrincipalOperador), así que aquí no aparece.
    public static class clsPermisoNegocio
    {
        // Nombres de los módulos del menú del administrador.
        public const string Dashboard = "DASHBOARD";
        public const string Diagnostico = "DIAGNOSTICO";
        public const string Alertas = "ALERTAS";
        public const string Criticidad = "CRITICIDAD";
        public const string Eventos = "EVENTOS";
        public const string Operadores = "OPERADORES";
        public const string Credenciales = "CREDENCIALES";
        public const string Equipos = "EQUIPOS";
        public const string Comandos = "COMANDOS";
        public const string Sincronizacion = "SINCRONIZACION";
        public const string Reportes = "REPORTES";
        public const string Configuracion = "CONFIGURACION";

        // Supervisor: supervisión de eventos, alertas y reportes.
        private static readonly string[] ModulosSupervisor = { Dashboard, Alertas, Eventos, Reportes };

        // Mantenimiento: diagnóstico técnico, equipos y comunicaciones.
        private static readonly string[] ModulosMantenimiento = { Dashboard, Diagnostico, Alertas, Equipos, Sincronizacion };

        // True si el rol puede ver el módulo.
        public static bool mtdPuedeVer(string rolCodigo, string modulo)
        {
            switch (rolCodigo)
            {
                case "ADMIN": return true;
                case "SUP": return Array.IndexOf(ModulosSupervisor, modulo) >= 0;
                case "MANT": return Array.IndexOf(ModulosMantenimiento, modulo) >= 0;
                default: return modulo == Dashboard;   // rol desconocido: solo el dashboard
            }
        }

        // Atajo para el usuario que inició sesión.
        public static bool mtdPuedeVer(string modulo)
        {
            return Sesion.Activa && mtdPuedeVer(Sesion.UsuarioActual.RolCodigo, modulo);
        }
    }
}
