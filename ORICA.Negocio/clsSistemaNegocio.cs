using System;
using ORICA.Datos;

namespace ORICA.Negocio
{
    /// <summary>
    /// Operaciones generales del sistema que la Presentación necesita
    /// sin referenciar ORICA.Datos directamente.
    /// </summary>
    public static class clsSistemaNegocio
    {
        public static Resultado mtdProbarConexion()
        {
            try
            {
                clsConexion.mtdProbarConexion();
                return Resultado.Ok("Conexión correcta con ORICA_SafeHandsDB");
            }
            catch (Exception ex)
            {
                return Resultado.Error("No se pudo conectar con ORICA_SafeHandsDB: " + ex.Message);
            }
        }
    }
}
