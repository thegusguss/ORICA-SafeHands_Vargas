using System.Configuration;
using System.Data.SqlClient;

namespace ORICA.Datos
{
    /// <summary>
    /// Crea conexiones a ORICA_SafeHandsDB.
    /// La cadena se lee del App.config del proyecto ejecutable (Presentación).
    /// Requiere referencia a System.Configuration en ORICA.Datos.
    /// </summary>
    public static class clsConexion
    {
        private const string NombreCadena = "ORICA_SafeHandsDB";

        public static SqlConnection mtdCrearConexion()
        {
            ConnectionStringSettings configuracion = ConfigurationManager.ConnectionStrings[NombreCadena];

            if (configuracion == null || string.IsNullOrWhiteSpace(configuracion.ConnectionString))
            {
                throw new ConfigurationErrorsException(
                    $"No se encontró la cadena de conexión {NombreCadena} en el App.config de Presentación.");
            }

            return new SqlConnection(configuracion.ConnectionString);
        }

        /// <summary>Prueba rápida de conectividad (útil para Diagnóstico).</summary>
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
