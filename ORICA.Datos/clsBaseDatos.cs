using System;
using System.Data;
using System.Data.SqlClient;

namespace ORICA.Datos
{
    internal static class clsBaseDatos
    {
        public static DataTable mtdConsultar(string sql, params SqlParameter[] parametros)
        {
            lock (clsConexion.Candado)
            using (SqlConnection conexion = clsConexion.mtdAbrirConexion(sql))
            using (SqlCommand comando = mtdCrearComando(conexion, sql, parametros))
            using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
            {
                DataTable tabla = new DataTable();
                adaptador.Fill(tabla);
                return tabla;
            }
        }

        public static object mtdConsultarValor(string sql, params SqlParameter[] parametros)
        {
            lock (clsConexion.Candado)
            using (SqlConnection conexion = clsConexion.mtdAbrirConexion(sql))
            using (SqlCommand comando = mtdCrearComando(conexion, sql, parametros))
            {
                return comando.ExecuteScalar();
            }
        }

        public static int mtdEjecutarComando(string sql, params SqlParameter[] parametros)
        {
            lock (clsConexion.Candado)
            using (SqlConnection conexion = clsConexion.mtdAbrirConexion(sql))
            using (SqlCommand comando = mtdCrearComando(conexion, sql, parametros))
            {
                return comando.ExecuteNonQuery();
            }
        }

        public static SqlParameter mtdParametro(string nombre, object valor)
        {
            bool vacio = valor == null || (valor is string texto && string.IsNullOrWhiteSpace(texto));
            return new SqlParameter(nombre, vacio ? DBNull.Value : valor);
        }

        private static SqlCommand mtdCrearComando(SqlConnection conexion, string sql, SqlParameter[] parametros)
        {
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.AddRange(parametros);
            return comando;
        }
    }
}
