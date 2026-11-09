using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ORICA.Datos
{
    // Acceso a datos de dbo.Configuracion (parámetros clave/valor del sistema).
    public class clsConfiguracionDatos
    {
        public DataTable mtdListar()
        {
            const string sql = @"
SELECT IdConfiguracion, Categoria, Clave, Valor, TipoDato, Descripcion, EsEditable, Estado,
       FechaModificacion, IdUsuarioModifica
FROM dbo.Configuracion
WHERE Estado = 1
ORDER BY Categoria, Clave;";

            return clsBaseDatos.mtdConsultar(sql);
        }

        /// <summary>
        /// Guarda varios parámetros en una sola transacción: actualiza los que existen y crea los que faltan.
        /// Cada elemento: categoria, clave, valor, tipoDato, descripcion.
        /// </summary>
        public void mtdGuardar(IList<string[]> parametros, int? idUsuario)
        {
            if (parametros == null || parametros.Count == 0) return;

            StringBuilder sql = new StringBuilder("SET XACT_ABORT ON;\nBEGIN TRANSACTION;\n");
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(clsBaseDatos.mtdParametro("@IdUsuario", idUsuario));

            for (int i = 0; i < parametros.Count; i++)
            {
                string[] p = parametros[i];
                sql.AppendFormat(@"
IF EXISTS (SELECT 1 FROM dbo.Configuracion WHERE Clave = @Clave{0})
    UPDATE dbo.Configuracion
    SET Valor = @Valor{0}, FechaModificacion = dbo.fnAhora(), IdUsuarioModifica = @IdUsuario
    WHERE Clave = @Clave{0} AND EsEditable = 1 AND Valor <> @Valor{0};
ELSE
    INSERT INTO dbo.Configuracion (Categoria, Clave, Valor, TipoDato, Descripcion, IdUsuarioModifica)
    VALUES (@Categoria{0}, @Clave{0}, @Valor{0}, @TipoDato{0}, @Descripcion{0}, @IdUsuario);
", i);

                lista.Add(clsBaseDatos.mtdParametro("@Categoria" + i, p[0]));
                lista.Add(clsBaseDatos.mtdParametro("@Clave" + i, p[1]));
                // El valor nunca se manda como NULL (la columna es NOT NULL).
                lista.Add(new SqlParameter("@Valor" + i, SqlDbType.VarChar, 300) { Value = p[2] ?? string.Empty });
                lista.Add(clsBaseDatos.mtdParametro("@TipoDato" + i, p[3]));
                lista.Add(clsBaseDatos.mtdParametro("@Descripcion" + i, p[4]));
            }

            sql.Append("\nCOMMIT TRANSACTION;");
            clsBaseDatos.mtdEjecutarComando(sql.ToString(), lista.ToArray());
        }
    }
}
