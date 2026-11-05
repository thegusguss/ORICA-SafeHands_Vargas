using System.Configuration;
using System.Data.SqlClient;

namespace ORICA.Datos
{
    /// <summary>
    /// Crea la conexión a ORICA_SafeHandsDB usando la cadena "ORICA_SafeHandsDB"
    /// del App.config del proyecto de Presentación.
    /// </summary>
    public static class clsConexion
    {
        public static SqlConnection mtdCrearConexion()
        {
            ConnectionStringSettings cadena = ConfigurationManager.ConnectionStrings["ORICA_SafeHandsDB"];

            if (cadena == null || string.IsNullOrWhiteSpace(cadena.ConnectionString))
                throw new ConfigurationErrorsException(
                    "No se encontró la cadena de conexión ORICA_SafeHandsDB en el App.config de Presentación.");

            return new SqlConnection(cadena.ConnectionString);
        }

        /// <summary>Abre y cierra una conexión. Si falla, lanza la excepción.</summary>
        public static bool mtdProbarConexion()
        {
            using (SqlConnection conexion = mtdCrearConexion())
            {
                conexion.Open();
                return true;
            }
        }
    }
}
