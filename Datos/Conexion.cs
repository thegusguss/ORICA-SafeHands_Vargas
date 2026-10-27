using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PROYECTO_ORICA_SAFEHANDS_VARGAS.Datos
{
    public static class Conexion
    {
        private static string ObtenerCadenaConexion()
        {
            ConnectionStringSettings configuracion =
                ConfigurationManager.ConnectionStrings["ORICA_SafeHandsDB"];

            if (configuracion == null)
            {
                throw new Exception(
                    "No se encontró la cadena de conexión ORICA_SafeHandsDB en App.config."
                );
            }

            return configuracion.ConnectionString;
        }

        public static SqlConnection CrearConexion()
        {
            return new SqlConnection(ObtenerCadenaConexion());
        }

        public static bool ProbarConexion()
        {
            using (SqlConnection conexion = CrearConexion())
            {
                conexion.Open();

                return conexion.State == ConnectionState.Open;
            }
        }
    }
}