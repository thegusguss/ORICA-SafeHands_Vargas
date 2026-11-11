using System.Data;

namespace ORICA.Datos
{
    public class clsCriticidadDatos
    {
        public DataTable mtdListar()
        {
            const string sql = @"
                                SELECT IdNivelCriticidad, Codigo, Nombre, NumeroConfirmaciones, TimeoutSegundos, Descripcion, Estado
                                FROM dbo.NivelCriticidad
                                ORDER BY IdNivelCriticidad;";

            return clsBaseDatos.mtdConsultar(sql);
        }

        public int mtdModificar(int idNivel, int confirmaciones, int timeoutSegundos, string descripcion)
        {
            const string sql = @"
                                UPDATE dbo.NivelCriticidad
                                SET NumeroConfirmaciones = @Confirmaciones,
                                    TimeoutSegundos = @Timeout,
                                    Descripcion = @Descripcion
                                WHERE IdNivelCriticidad = @IdNivel;";

            return clsBaseDatos.mtdEjecutarComando(sql,
                clsBaseDatos.mtdParametro("@IdNivel", idNivel),
                clsBaseDatos.mtdParametro("@Confirmaciones", confirmaciones),
                clsBaseDatos.mtdParametro("@Timeout", timeoutSegundos),
                clsBaseDatos.mtdParametro("@Descripcion", descripcion));
        }
    }
}

