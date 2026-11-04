using System;
using System.Data;

namespace ORICA.Negocio
{
    /// <summary>Conversión segura de columnas de DataRow, manejando NULL.</summary>
    internal static class clsLectura
    {
        public static string mtdLeerTexto(DataRow r, string col)
        {
            return r[col] == DBNull.Value ? null : Convert.ToString(r[col]);
        }

        public static int mtdLeerEntero(DataRow r, string col)
        {
            return r[col] == DBNull.Value ? 0 : Convert.ToInt32(r[col]);
        }

        public static bool mtdLeerBooleano(DataRow r, string col)
        {
            return r[col] != DBNull.Value && Convert.ToBoolean(r[col]);
        }

        public static DateTime? mtdLeerFecha(DataRow r, string col)
        {
            return r[col] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r[col]);
        }

        public static byte[] mtdLeerBytes(DataRow r, string col)
        {
            return r[col] == DBNull.Value ? null : (byte[])r[col];
        }
    }
}
