using System.Data;

namespace ORICA.Datos
{
    // Escritura en dbo.Auditoria: quién cambió qué, cuándo y desde qué PC.
    public class clsAuditoriaDatos
    {
        public int mtdRegistrar(int? idUsuario, string modulo, string accion, string entidad, string idRegistro,
                                string descripcion, string valorAnterior, string valorNuevo, string equipoCliente)
        {
            const string sql = @"
INSERT INTO dbo.Auditoria (IdUsuario, Modulo, Accion, Entidad, IdRegistro, Descripcion,
                           ValorAnterior, ValorNuevo, NombreEquipoCliente)
VALUES (@IdUsuario, @Modulo, @Accion, @Entidad, @IdRegistro, @Descripcion,
        @ValorAnterior, @ValorNuevo, @EquipoCliente);";

            return clsBaseDatos.mtdEjecutarComando(sql,
                clsBaseDatos.mtdParametro("@IdUsuario", idUsuario),
                clsBaseDatos.mtdParametro("@Modulo", modulo),
                clsBaseDatos.mtdParametro("@Accion", accion),
                clsBaseDatos.mtdParametro("@Entidad", entidad),
                clsBaseDatos.mtdParametro("@IdRegistro", idRegistro),
                clsBaseDatos.mtdParametro("@Descripcion", descripcion),
                clsBaseDatos.mtdParametro("@ValorAnterior", valorAnterior),
                clsBaseDatos.mtdParametro("@ValorNuevo", valorNuevo),
                clsBaseDatos.mtdParametro("@EquipoCliente", equipoCliente));
        }
    }
}
