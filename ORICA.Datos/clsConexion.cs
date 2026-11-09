using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ORICA.Datos
{
    /// <summary>
    /// Decide a qué base se conecta la aplicación:
    /// - Azure SQL (principal, compartida): cadena "CadenaAzure" del archivo conexion.config (no se sube a git).
    /// - LocalDB (copia local): cadena "ORICA_SafeHandsDB" del App.config. Se usa cuando no hay internet.
    /// Si Azure no responde al abrir, se pasa solo a la base local y la sincronización vuelve después.
    /// </summary>
    public static class clsConexion
    {
        // true = se trabaja con Azure; false = con la copia local.
        private static volatile bool _enLinea;
        // Para pruebas: obliga a trabajar sin conexión aunque haya internet (botón en Sincronización).
        private static volatile bool _simularSinConexion;

        /// <summary>Lo usan clsBaseDatos y la sincronización para no pisarse (una sola cosa a la vez).</summary>
        public static readonly object Candado = new object();

        /// <summary>Sin conexión, los roles que no son operador solo pueden consultar (sus cambios se perderían).</summary>
        public static bool SoloLecturaSinConexion { get; set; }

        public static string UltimoError { get; private set; }

        public static bool EnLinea { get { return _enLinea; } }

        /// <summary>true si hay una cadena de Azure configurada en conexion.config.</summary>
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

        /// <summary>
        /// Abre la conexión que toca (Azure o local). Si Azure falla al abrir, se pasa a modo local.
        /// sql: la consulta que se va a ejecutar; sirve para bloquear cambios en modo solo lectura.
        /// </summary>
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

        // ¿La consulta inserta, actualiza o borra algo?
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

            // La base gratuita de Azure se pausa sola si nadie la usa y tarda en "despertar":
            // para sincronizar se espera hasta 60 s y se intenta 2 veces.
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

        /// <summary>Abre y cierra una conexión (Azure o local). Si falla, lanza la excepción.</summary>
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
