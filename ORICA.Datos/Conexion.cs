using System.Configuration;
using System.Data.SqlClient;

namespace ORICA.Datos
{
    /// <summary>
    /// Crea conexiones a ORICA_SafeHandsDB.
    /// La cadena se lee del App.config del proyecto ejecutable (Presentación).
    /// Requiere referencia a System.Configuration en ORICA.Datos.
    /// </summary>
    public static class Conexion
    {
        private const string NombreCadena = "ORICA_SafeHandsDB";

        public static SqlConnection Crear()
        {
            ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings[NombreCadena];

            if (cs == null || string.IsNullOrWhiteSpace(cs.ConnectionString))
            {
                throw new ConfigurationErrorsException(
                    $"No se encontró la cadena de conexión {NombreCadena} en el App.config de Presentación.");
            }

            return new SqlConnection(cs.ConnectionString);
        }

        /// <summary>Prueba rápida de conectividad (útil para Diagnóstico).</summary>
        public static bool Probar()
        {
            using (SqlConnection cn = Crear())
            {
                cn.Open();
                return true;
            }
        }
    }
}
