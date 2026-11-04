using System;
using System.Data;
using System.Data.SqlClient;

namespace ORICA.Datos
{
    /// <summary>
    /// Helper interno de ADO.NET. Evita repetir el mismo bloque using/open
    /// en cada clase Datos. Solo visible dentro de ORICA.Datos.
    /// </summary>
    internal static class clsBaseDatos
    {
        public static DataTable mtdConsultar(string sql, params SqlParameter[] parametros)
        {
            using (SqlConnection cn = clsConexion.mtdCrearConexion())
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

        public static object mtdConsultarValor(string sql, params SqlParameter[] parametros)
        {
            using (SqlConnection cn = clsConexion.mtdCrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (parametros != null) cmd.Parameters.AddRange(parametros);
                cn.Open();
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>Devuelve el número de filas afectadas.</summary>
        public static int mtdEjecutarComando(string sql, params SqlParameter[] parametros)
        {
            using (SqlConnection cn = clsConexion.mtdCrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (parametros != null) cmd.Parameters.AddRange(parametros);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>Convierte texto vacío o null en DBNull para columnas opcionales.</summary>
        public static object mtdTextoParaSql(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? (object)DBNull.Value : valor;
        }
    }
}
