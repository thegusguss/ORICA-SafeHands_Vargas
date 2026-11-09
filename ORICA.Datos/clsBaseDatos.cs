using System;
using System.Data;
using System.Data.SqlClient;

namespace ORICA.Datos
{
    /// <summary>
    /// Métodos comunes para ejecutar SQL. Las clases Datos solo escriben
    /// la consulta y sus parámetros; aquí se abre y cierra la conexión.
    /// clsConexion decide si la conexión va a Azure o a la copia local (sin internet).
    /// El "lock" evita que una consulta se cruce con la sincronización.
    /// </summary>
    internal static class clsBaseDatos
    {
        /// <summary>Ejecuta un SELECT y devuelve las filas en un DataTable.</summary>
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

        /// <summary>Ejecuta la consulta y devuelve solo el primer valor (un Id, un COUNT...).</summary>
        public static object mtdConsultarValor(string sql, params SqlParameter[] parametros)
        {
            lock (clsConexion.Candado)
            using (SqlConnection conexion = clsConexion.mtdAbrirConexion(sql))
            using (SqlCommand comando = mtdCrearComando(conexion, sql, parametros))
            {
                return comando.ExecuteScalar();
            }
        }

        /// <summary>Ejecuta un INSERT/UPDATE/DELETE y devuelve las filas afectadas.</summary>
        public static int mtdEjecutarComando(string sql, params SqlParameter[] parametros)
        {
            lock (clsConexion.Candado)
            using (SqlConnection conexion = clsConexion.mtdAbrirConexion(sql))
            using (SqlCommand comando = mtdCrearComando(conexion, sql, parametros))
            {
                return comando.ExecuteNonQuery();
            }
        }

        /// Crea un parámetro SQL. Si el valor es null o un texto vacío,
        /// se envía NULL a la base de datos.
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
