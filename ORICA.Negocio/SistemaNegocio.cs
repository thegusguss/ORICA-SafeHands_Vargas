using System;
using ORICA.Datos;

namespace ORICA.Negocio
{
    /// <summary>
    /// Operaciones generales del sistema que la Presentación necesita
    /// sin referenciar ORICA.Datos directamente.
    /// </summary>
    public static class SistemaNegocio
    {
        public static Resultado ProbarConexion()
        {
            try
            {
                Conexion.Probar();
                return Resultado.Ok("Conexión correcta con ORICA_SafeHandsDB");
            }
            catch (Exception ex)
            {
                return Resultado.Error("No se pudo conectar con ORICA_SafeHandsDB: " + ex.Message);
            }
        }
    }
}
