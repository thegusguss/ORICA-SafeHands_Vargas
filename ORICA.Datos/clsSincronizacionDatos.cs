using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ORICA.Datos
{
    /// <summary>
    /// Mueve datos entre la copia local (LocalDB) y Azure SQL.
    /// - Pendientes: lo que se registró en local sin internet (sesiones, eventos, confirmaciones, alertas).
    ///   Se reconocen porque son más nuevos que la última copia (dbo.CopiaLocal, tabla solo local).
    /// - Copia completa: borra las tablas del destino y las llena con las del origen (SqlBulkCopy).
    /// Estos métodos abren sus propias conexiones; quien los llama debe tener clsConexion.Candado.
    /// </summary>
    public class clsSincronizacionDatos
    {
        // Todas las tablas, en un orden en que cada una va después de las que referencia (llaves foráneas).
        private static readonly string[] Tablas =
        {
            "Rol", "Usuario", "TipoAccion", "NivelCriticidad", "Comando", "TipoEquipo", "Equipo",
            "SesionOperador", "Evento", "Alerta", "Auditoria", "ComandoEquipo", "Configuracion",
            "Confirmacion", "TipoCredencial", "Credencial", "Dispositivo", "EstadoDispositivo",
            "Sincronizacion", "VersionBaseDatos"
        };

        // Lo que se puede crear sin internet. Llaves: para saber si ya existe en Azure.
        private class TablaPendiente
        {
            public string Nombre, Filtro, Identidad;
            public string[] Llaves;
        }

        private static readonly TablaPendiente[] Pendientes =
        {
            new TablaPendiente { Nombre = "SesionOperador", Llaves = new[] { "IdSesion" },
                                 Filtro = "FechaInicio >= @Desde OR FechaFin >= @Desde" },
            new TablaPendiente { Nombre = "Evento", Llaves = new[] { "IdEvento" },
                                 Filtro = "FechaHora >= @Desde" },
            new TablaPendiente { Nombre = "Confirmacion", Llaves = new[] { "IdEvento", "NumeroPaso" },
                                 Identidad = "IdConfirmacion", Filtro = "FechaHora >= @Desde" },
            new TablaPendiente { Nombre = "Alerta", Llaves = new[] { "IdAlerta" },
                                 Filtro = "FechaHora >= @Desde OR FechaReconocida >= @Desde OR FechaCierre >= @Desde" }
        };

        // Margen por si los relojes de la PC y de Azure no están iguales.
        private static readonly TimeSpan Margen = TimeSpan.FromMinutes(2);

        // ---------------- Preparar la copia local ----------------

        /// <summary>
        /// Deja la LocalDB lista para trabajar como copia (se puede ejecutar muchas veces):
        /// crea dbo.fnAhora y dbo.CopiaLocal, marca los eventos locales como SIN_CONEXION
        /// y apaga las llaves foráneas (la integridad la revisa Azure al subir).
        /// </summary>
        public void mtdPrepararLocal()
        {
            const string sql = @"
IF OBJECT_ID('dbo.fnAhora') IS NULL
    EXEC('CREATE FUNCTION dbo.fnAhora() RETURNS DATETIME2(0) AS
          BEGIN RETURN CONVERT(DATETIME2(0), SYSDATETIMEOFFSET() AT TIME ZONE ''SA Pacific Standard Time''); END');

IF OBJECT_ID('dbo.CopiaLocal') IS NULL
    CREATE TABLE dbo.CopiaLocal (FechaCopia DATETIME2(0) NOT NULL);

IF OBJECT_ID('DF_Evento_Origen') IS NOT NULL
   AND (SELECT definition FROM sys.default_constraints WHERE name = 'DF_Evento_Origen') <> '(''SIN_CONEXION'')'
BEGIN
    ALTER TABLE dbo.Evento DROP CONSTRAINT DF_Evento_Origen;
    ALTER TABLE dbo.Evento ADD CONSTRAINT DF_Evento_Origen DEFAULT ('SIN_CONEXION') FOR Origen;
END

DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql += N'ALTER TABLE ' + QUOTENAME(SCHEMA_NAME(t.schema_id)) + N'.' + QUOTENAME(t.name)
             + N' NOCHECK CONSTRAINT ' + QUOTENAME(fk.name) + N';'
FROM sys.foreign_keys fk JOIN sys.tables t ON t.object_id = fk.parent_object_id
WHERE fk.is_disabled = 0;
EXEC (@sql);";

            using (SqlConnection local = clsConexion.mtdAbrirLocal())
            using (SqlCommand comando = new SqlCommand(sql, local))
                comando.ExecuteNonQuery();
        }

        /// <summary>Fecha de la última copia desde Azure (null = esta PC nunca se sincronizó).</summary>
        public DateTime? mtdFechaCopia()
        {
            using (SqlConnection local = clsConexion.mtdAbrirLocal())
            using (SqlCommand comando = new SqlCommand("SELECT MAX(FechaCopia) FROM dbo.CopiaLocal;", local))
            {
                object valor = comando.ExecuteScalar();
                return valor == null || valor == DBNull.Value ? (DateTime?)null : (DateTime)valor;
            }
        }

        /// <summary>Cuántos registros locales faltan subir a Azure.</summary>
        public int mtdContarPendientes()
        {
            DateTime? copia = mtdFechaCopia();
            if (copia == null) return 0;

            int total = 0;
            using (SqlConnection local = clsConexion.mtdAbrirLocal())
            {
                foreach (TablaPendiente t in Pendientes)
                {
                    string sql = "SELECT COUNT(*) FROM dbo." + t.Nombre + " WHERE " + t.Filtro + ";";
                    using (SqlCommand comando = new SqlCommand(sql, local))
                    {
                        comando.Parameters.AddWithValue("@Desde", copia.Value - Margen);
                        total += (int)comando.ExecuteScalar();
                    }
                }
            }
            return total;
        }

        // ---------------- Subir pendientes ----------------

        /// <summary>
        /// Sube a Azure lo registrado sin conexión, todo en una transacción (o se sube todo o nada).
        /// Si un registro ya existe en Azure se actualiza; si no, se inserta. Devuelve cuántos subió.
        /// </summary>
        public int mtdSubirPendientes(SqlConnection azure)
        {
            DateTime? copia = mtdFechaCopia();
            if (copia == null) return 0;

            // 1) Leer de la base local lo pendiente de cada tabla.
            List<KeyValuePair<TablaPendiente, DataTable>> datos = new List<KeyValuePair<TablaPendiente, DataTable>>();
            using (SqlConnection local = clsConexion.mtdAbrirLocal())
            {
                foreach (TablaPendiente t in Pendientes)
                {
                    string sql = "SELECT * FROM dbo." + t.Nombre + " WHERE " + t.Filtro + ";";
                    using (SqlCommand comando = new SqlCommand(sql, local))
                    using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                    {
                        comando.Parameters.AddWithValue("@Desde", copia.Value - Margen);
                        DataTable tabla = new DataTable();
                        adaptador.Fill(tabla);
                        datos.Add(new KeyValuePair<TablaPendiente, DataTable>(t, tabla));
                    }
                }
            }

            int total = datos.Sum(d => d.Value.Rows.Count);
            if (total == 0) return 0;

            // 2) Escribirlo en Azure dentro de una transacción y dejar constancia en dbo.Sincronizacion.
            Guid lote = Guid.NewGuid();
            using (SqlTransaction tx = azure.BeginTransaction())
            {
                foreach (KeyValuePair<TablaPendiente, DataTable> d in datos)
                {
                    foreach (DataRow fila in d.Value.Rows)
                        GuardarFila(azure, tx, d.Key, fila);

                    if (d.Value.Rows.Count > 0)
                        RegistrarLote(azure, tx, lote, d.Key.Nombre, d.Value.Rows.Count);
                }
                tx.Commit();
            }
            return total;
        }

        // Si la fila ya existe en Azure (por sus llaves) la actualiza; si no, la inserta.
        private static void GuardarFila(SqlConnection azure, SqlTransaction tx, TablaPendiente t, DataRow fila)
        {
            List<string> columnas = fila.Table.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName).Where(c => c != t.Identidad).ToList();
            List<string> cambiables = columnas.Where(c => !t.Llaves.Contains(c)).ToList();

            string donde = string.Join(" AND ", t.Llaves.Select(c => "[" + c + "] = @" + c));
            string sql =
                "IF EXISTS (SELECT 1 FROM dbo." + t.Nombre + " WHERE " + donde + ") " +
                "UPDATE dbo." + t.Nombre + " SET " + string.Join(", ", cambiables.Select(c => "[" + c + "] = @" + c)) +
                " WHERE " + donde + " " +
                "ELSE INSERT INTO dbo." + t.Nombre + " (" + string.Join(", ", columnas.Select(c => "[" + c + "]")) + ")" +
                " VALUES (" + string.Join(", ", columnas.Select(c => "@" + c)) + ");";

            using (SqlCommand comando = new SqlCommand(sql, azure, tx))
            {
                foreach (string c in columnas)
                    comando.Parameters.AddWithValue("@" + c, fila[c]);
                comando.ExecuteNonQuery();
            }
        }

        private static void RegistrarLote(SqlConnection azure, SqlTransaction tx, Guid lote, string tabla, int cantidad)
        {
            const string sql = @"
INSERT INTO dbo.Sincronizacion (IdLote, TipoEntidad, IdRegistroOrigen, Accion, EstadoSincronizacion,
                                Intentos, FechaCreacion, FechaUltimoIntento, FechaCompletado)
VALUES (@Lote, @Tabla, @Origen, 'INSERT', 'COMPLETADO', 1, dbo.fnAhora(), dbo.fnAhora(), dbo.fnAhora());";

            using (SqlCommand comando = new SqlCommand(sql, azure, tx))
            {
                comando.Parameters.AddWithValue("@Lote", lote);
                comando.Parameters.AddWithValue("@Tabla", tabla);
                comando.Parameters.AddWithValue("@Origen", Environment.MachineName + ": " + cantidad + " registros");
                comando.ExecuteNonQuery();
            }
        }

        // ---------------- Copia completa ----------------

        /// <summary>Reemplaza la copia local con todo lo que hay en Azure y guarda la fecha de copia.</summary>
        public void mtdDescargarDeAzure(SqlConnection azure)
        {
            DateTime inicio = DateTime.Now;
            using (SqlConnection local = clsConexion.mtdAbrirLocal())
            {
                Copiar(azure, local);
                GuardarFechaCopia(local, inicio);
            }
        }

        /// <summary>Reemplaza TODO Azure con la base de esta PC (solo para la primera carga).</summary>
        public void mtdSubirTodoAAzure(SqlConnection azure)
        {
            using (SqlConnection local = clsConexion.mtdAbrirLocal())
            {
                Copiar(local, azure);
                GuardarFechaCopia(local, DateTime.Now);
            }
        }

        // Borra las tablas del destino (de la última a la primera) y las llena con las del origen.
        // Todo en una transacción: si algo falla, el destino queda como estaba.
        private static void Copiar(SqlConnection origen, SqlConnection destino)
        {
            using (SqlTransaction tx = destino.BeginTransaction())
            {
                foreach (string tabla in Tablas.Reverse())
                    using (SqlCommand borrar = new SqlCommand("DELETE FROM dbo." + tabla + ";", destino, tx))
                        borrar.ExecuteNonQuery();

                foreach (string tabla in Tablas)
                {
                    using (SqlCommand leer = new SqlCommand("SELECT * FROM dbo." + tabla + ";", origen))
                    using (SqlDataReader lector = leer.ExecuteReader())
                    using (SqlBulkCopy copia = new SqlBulkCopy(destino,
                               SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.KeepNulls, tx))
                    {
                        copia.DestinationTableName = "dbo." + tabla;
                        copia.BulkCopyTimeout = 300;
                        for (int i = 0; i < lector.FieldCount; i++)
                            copia.ColumnMappings.Add(lector.GetName(i), lector.GetName(i));   // por nombre, no por posición
                        copia.WriteToServer(lector);
                    }
                }
                tx.Commit();
            }
        }

        private static void GuardarFechaCopia(SqlConnection local, DateTime fecha)
        {
            const string sql = "DELETE FROM dbo.CopiaLocal; INSERT INTO dbo.CopiaLocal (FechaCopia) VALUES (@Fecha);";
            using (SqlCommand comando = new SqlCommand(sql, local))
            {
                comando.Parameters.AddWithValue("@Fecha", fecha);
                comando.ExecuteNonQuery();
            }
        }

        // ---------------- Historial ----------------

        /// <summary>Últimas subidas registradas en dbo.Sincronizacion (de Azure o de la copia local).</summary>
        public DataTable mtdListarHistorial(int maximo)
        {
            const string sql = @"
SELECT TOP (@Maximo) IdSincronizacion, TipoEntidad, IdRegistroOrigen, Intentos, EstadoSincronizacion,
       FechaCompletado, MensajeError
FROM dbo.Sincronizacion
ORDER BY IdSincronizacion DESC;";

            return clsBaseDatos.mtdConsultar(sql, clsBaseDatos.mtdParametro("@Maximo", maximo));
        }
    }
}
