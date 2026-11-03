using System;
using System.Data;
using System.Data.SqlClient;

namespace ORICA.Datos
{
    /// <summary>
    /// Helper interno de ADO.NET. Evita repetir el mismo bloque using/open
    /// en cada clase Datos. Solo visible dentro de ORICA.Datos.
    /// </summary>
    internal static class BaseDatos
    {
        public static DataTable Consultar(string sql, params SqlParameter[] parametros)
        {
            using (SqlConnection cn = Conexion.Crear())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (parametros != null) cmd.Parameters.AddRange(parametros);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public static object Escalar(string sql, params SqlParameter[] parametros)
        {
            using (SqlConnection cn = Conexion.Crear())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (parametros != null) cmd.Parameters.AddRange(parametros);
                cn.Open();
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>Devuelve el número de filas afectadas.</summary>
        public static int Ejecutar(string sql, params SqlParameter[] parametros)
        {
            using (SqlConnection cn = Conexion.Crear())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (parametros != null) cmd.Parameters.AddRange(parametros);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>Convierte texto vacío o null en DBNull para columnas opcionales.</summary>
        public static object TextoONulo(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? (object)DBNull.Value : valor;
        }
    }
}
