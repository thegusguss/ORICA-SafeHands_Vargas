using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ORICA.Datos
{
    

    public static class clsConexion
    {
        private static volatile bool _enLinea;
        private static volatile bool _simularSinConexion;

        public static readonly object Candado = new object();

        public static bool SoloLecturaSinConexion { get; set; }

        public static string UltimoError { get; private set; }

        public static bool EnLinea { get { return _enLinea; } }

        public static bool TieneAzure
        {
            get { return !string.IsNullOrWhiteSpace(CadenaAzure); }
        }

        public static bool SimularSinConexion
        {
            get { return _simularSinConexion; }
            set { _simularSinConexion = value; if (value) mtdPasarAModoLocal("Desconexión simulada."); }
        }

        internal static string CadenaAzure
        {
            get { return ConfigurationManager.AppSettings["CadenaAzure"]; }
        }

        internal static string CadenaLocal
        {
            get
            {
                ConnectionStringSettings cadena = ConfigurationManager.ConnectionStrings["ORICA_SafeHandsDB"];
                if (cadena == null || string.IsNullOrWhiteSpace(cadena.ConnectionString))
                    throw new ConfigurationErrorsException(
                        "No se encontró la cadena de conexión ORICA_SafeHandsDB en el App.config de Presentación.");
                return cadena.ConnectionString;
            }
        }

        internal static SqlConnection mtdAbrirConexion(string sql)
        {
            if (_enLinea)
            {
                SqlConnection azure = new SqlConnection(CadenaAzure);
                try
                {
                    azure.Open();
                    return azure;
                }
                catch (Exception ex)
                {
                    azure.Dispose();
                    mtdPasarAModoLocal(ex.Message);   // se sigue trabajando con la copia local
                }
            }

            if (SoloLecturaSinConexion && ModificaDatos(sql))
                throw new InvalidOperationException(
                    "Sin conexión con Azure. Los cambios de administración solo se pueden hacer con conexión.");

            SqlConnection local = new SqlConnection(CadenaLocal);
            local.Open();
            return local;
        }

        private static bool ModificaDatos(string sql)
        {
            return Regex.IsMatch(sql, @"\b(INSERT|UPDATE|DELETE|MERGE)\b", RegexOptions.IgnoreCase);
        }

        public static void mtdPasarAModoLocal(string motivo)
        {
            _enLinea = false;
            UltimoError = motivo;
        }

        public static void mtdPasarAModoAzure()
        {
            _enLinea = true;
            UltimoError = null;
        }

        /// <summary>Abre una conexión directa a Azure (sin pasar a local). La usa la sincronización.</summary>
        public static SqlConnection mtdAbrirAzure()
        {
            if (!TieneAzure) throw new InvalidOperationException("No hay cadena de Azure en conexion.config.");
            if (_simularSinConexion) throw new InvalidOperationException("Desconexión simulada activa.");

            SqlConnectionStringBuilder cadena = new SqlConnectionStringBuilder(CadenaAzure) { ConnectTimeout = 60 };

            for (int intento = 1; ; intento++)
            {
                SqlConnection azure = new SqlConnection(cadena.ConnectionString);
                try
                {
                    azure.Open();
                    return azure;
                }
                catch (Exception ex)
                {
                    azure.Dispose();
                    if (intento == 2) throw new Exception("Azure no respondió: " + ex.Message, ex);
                }
            }
        }

        internal static SqlConnection mtdAbrirLocal()
        {
            SqlConnection local = new SqlConnection(CadenaLocal);
            try
            {
                local.Open();
                return local;
            }
            catch (Exception ex)
            {
                local.Dispose();
                throw new Exception("La base local (LocalDB) no respondió: " + ex.Message, ex);
            }
        }

        public static bool mtdProbarConexion()
        {
            lock (Candado)
            {
                using (SqlConnection conexion = mtdAbrirConexion("SELECT 1"))
                    return true;
            }
        }
    }
}
