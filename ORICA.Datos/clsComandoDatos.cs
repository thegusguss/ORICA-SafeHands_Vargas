using System;
using System.Collections.Generic;
using System.Data;

namespace ORICA.Datos
{
    // Acceso a datos de los comandos de voz y su asignación a equipos (ComandoEquipo).
    public class clsComandoDatos
    {
        private const string SelectComandos = @"
SELECT c.IdComando, c.Codigo, c.Nombre, c.Frase, c.IdNivelCriticidad,
       nc.Nombre AS CriticidadNombre, nc.NumeroConfirmaciones, nc.TimeoutSegundos,
       c.IdTipoAccion, ta.Nombre AS TipoAccionNombre, ta.Codigo AS TipoAccionCodigo,
       c.ConfianzaMinima, c.RequierePTT, c.Estado, c.FechaRegistro, c.FechaModificacion, c.RowVersion
FROM dbo.Comando c
INNER JOIN dbo.NivelCriticidad nc ON nc.IdNivelCriticidad = c.IdNivelCriticidad
INNER JOIN dbo.TipoAccion ta      ON ta.IdTipoAccion = c.IdTipoAccion";

        // ---------------- Consultas ----------------

        public DataTable mtdListar()
        {
            return clsBaseDatos.mtdConsultar(SelectComandos + " ORDER BY c.Codigo;");
        }

        /// <summary>Comandos activos asignados (y activos) a un equipo activo.</summary>
        public DataTable mtdListarPorEquipo(int idEquipo)
        {
            string sql = SelectComandos + @"
INNER JOIN dbo.ComandoEquipo ce ON ce.IdComando = c.IdComando
INNER JOIN dbo.Equipo e         ON e.IdEquipo = ce.IdEquipo
WHERE ce.IdEquipo = @IdEquipo
  AND ce.Estado = 1
  AND c.Estado = 1
  AND e.Estado = 1
ORDER BY c.IdNivelCriticidad, c.Frase;";

            return clsBaseDatos.mtdConsultar(sql, clsBaseDatos.mtdParametro("@IdEquipo", idEquipo));
        }

        /// <summary>Ids de los equipos a los que el comando está asignado (activos).</summary>
        public DataTable mtdListarEquiposAsignados(int idComando)
        {
            const string sql = @"
SELECT IdEquipo FROM dbo.ComandoEquipo
WHERE IdComando = @IdComando AND Estado = 1;";

            return clsBaseDatos.mtdConsultar(sql, clsBaseDatos.mtdParametro("@IdComando", idComando));
        }

        public DataTable mtdListarTiposAccion()
        {
            const string sql = @"
SELECT IdTipoAccion, Codigo, Nombre, EsAccionSegura
FROM dbo.TipoAccion
WHERE Estado = 1
ORDER BY IdTipoAccion;";

            return clsBaseDatos.mtdConsultar(sql);
        }

        // ---------------- Escritura ----------------

        // Inserta el comando y sus equipos en una sola transacción. Devuelve el IdComando nuevo.
        public int mtdRegistrar(string codigo, string nombre, string frase, int idNivel, int idTipoAccion,
                                decimal confianza, bool requierePtt, IEnumerable<int> idsEquipos)
        {
            const string sql = @"
SET XACT_ABORT ON;
BEGIN TRANSACTION;

DECLARE @Nuevo TABLE (IdComando INT);

INSERT INTO dbo.Comando (Codigo, Nombre, Frase, IdNivelCriticidad, IdTipoAccion, ConfianzaMinima, RequierePTT)
OUTPUT INSERTED.IdComando INTO @Nuevo
VALUES (@Codigo, @Nombre, @Frase, @IdNivel, @IdTipoAccion, @Confianza, @PTT);

DECLARE @IdComando INT = (SELECT IdComando FROM @Nuevo);

INSERT INTO dbo.ComandoEquipo (IdComando, IdEquipo)
SELECT @IdComando, CAST(value AS INT)
FROM STRING_SPLIT(@Equipos, ',')
WHERE LTRIM(RTRIM(value)) <> '';

COMMIT TRANSACTION;

SELECT @IdComando;";

            object id = clsBaseDatos.mtdConsultarValor(sql,
                clsBaseDatos.mtdParametro("@Codigo", codigo),
                clsBaseDatos.mtdParametro("@Nombre", nombre),
                clsBaseDatos.mtdParametro("@Frase", frase),
                clsBaseDatos.mtdParametro("@IdNivel", idNivel),
                clsBaseDatos.mtdParametro("@IdTipoAccion", idTipoAccion),
                clsBaseDatos.mtdParametro("@Confianza", confianza),
                clsBaseDatos.mtdParametro("@PTT", requierePtt),
                clsBaseDatos.mtdParametro("@Equipos", string.Join(",", idsEquipos) + ","));

            return Convert.ToInt32(id);
        }

        /// <summary>
        /// Actualiza el comando (si RowVersion no cambió) y deja asignados exactamente los equipos indicados.
        /// Devuelve 0 si otro usuario lo modificó antes.
        /// </summary>
        public int mtdModificar(int idComando, string codigo, string nombre, string frase, int idNivel,
                                int idTipoAccion, decimal confianza, bool requierePtt, byte[] rowVersion,
                                IEnumerable<int> idsEquipos)
        {
            const string sql = @"
SET XACT_ABORT ON;
BEGIN TRANSACTION;

UPDATE dbo.Comando
SET Codigo = @Codigo,
    Nombre = @Nombre,
    Frase = @Frase,
    IdNivelCriticidad = @IdNivel,
    IdTipoAccion = @IdTipoAccion,
    ConfianzaMinima = @Confianza,
    RequierePTT = @PTT,
    FechaModificacion = dbo.fnAhora()
WHERE IdComando = @IdComando
  AND RowVersion = @RowVersion;

DECLARE @Filas INT = @@ROWCOUNT;

IF @Filas > 0
BEGIN
    DECLARE @Sel TABLE (IdEquipo INT PRIMARY KEY);
    INSERT INTO @Sel (IdEquipo)
    SELECT DISTINCT CAST(value AS INT) FROM STRING_SPLIT(@Equipos, ',') WHERE LTRIM(RTRIM(value)) <> '';

    -- Los que ya no están marcados se desactivan; los marcados se activan o se insertan.
    UPDATE dbo.ComandoEquipo SET Estado = 0
    WHERE IdComando = @IdComando AND IdEquipo NOT IN (SELECT IdEquipo FROM @Sel);

    UPDATE dbo.ComandoEquipo SET Estado = 1
    WHERE IdComando = @IdComando AND IdEquipo IN (SELECT IdEquipo FROM @Sel);

    INSERT INTO dbo.ComandoEquipo (IdComando, IdEquipo)
    SELECT @IdComando, s.IdEquipo FROM @Sel s
    WHERE NOT EXISTS (SELECT 1 FROM dbo.ComandoEquipo ce WHERE ce.IdComando = @IdComando AND ce.IdEquipo = s.IdEquipo);
END

COMMIT TRANSACTION;

SELECT @Filas;";

            object filas = clsBaseDatos.mtdConsultarValor(sql,
                clsBaseDatos.mtdParametro("@IdComando", idComando),
                clsBaseDatos.mtdParametro("@Codigo", codigo),
                clsBaseDatos.mtdParametro("@Nombre", nombre),
                clsBaseDatos.mtdParametro("@Frase", frase),
                clsBaseDatos.mtdParametro("@IdNivel", idNivel),
                clsBaseDatos.mtdParametro("@IdTipoAccion", idTipoAccion),
                clsBaseDatos.mtdParametro("@Confianza", confianza),
                clsBaseDatos.mtdParametro("@PTT", requierePtt),
                clsBaseDatos.mtdParametro("@RowVersion", rowVersion),
                clsBaseDatos.mtdParametro("@Equipos", string.Join(",", idsEquipos) + ","));

            return Convert.ToInt32(filas);
        }

        public int mtdCambiarEstado(int idComando, bool estado)
        {
            const string sql = @"
UPDATE dbo.Comando
SET Estado = @Estado, FechaModificacion = dbo.fnAhora()
WHERE IdComando = @IdComando;";

            return clsBaseDatos.mtdEjecutarComando(sql,
                clsBaseDatos.mtdParametro("@IdComando", idComando),
                clsBaseDatos.mtdParametro("@Estado", estado));
        }

        // ---------------- Validación de duplicados ----------------

        public bool mtdExisteCodigo(string codigo, int? idExcluir)
        {
            return mtdExiste("Codigo", codigo, idExcluir);
        }

        public bool mtdExisteFrase(string frase, int? idExcluir)
        {
            return mtdExiste("Frase", frase, idExcluir);
        }

        private bool mtdExiste(string columna, string valor, int? idExcluir)
        {
            if (string.IsNullOrWhiteSpace(valor)) return false;

            string sql = "SELECT COUNT(1) FROM dbo.Comando WHERE " + columna + " = @Valor" +
                         " AND (@IdExcluir IS NULL OR IdComando <> @IdExcluir);";

            object total = clsBaseDatos.mtdConsultarValor(sql,
                clsBaseDatos.mtdParametro("@Valor", valor),
                clsBaseDatos.mtdParametro("@IdExcluir", idExcluir));

            return Convert.ToInt32(total) > 0;
        }
    }
}
