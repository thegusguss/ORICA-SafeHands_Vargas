using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using ORICA.Datos;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    // Reglas de los comandos de voz: gramática cerrada, criticidad, confianza y equipos asignados.
    public class clsComandoNegocio
    {
        private const int MaxCodigo = 20;
        private const int MaxNombre = 100;
        private const int MaxFrase = 180;
        private const int MinPalabras = 2;            // frases de una palabra se confunden con facilidad
        public const decimal MinConfianza = 50m;
        public const decimal MaxConfianza = 100m;

        private readonly clsComandoDatos _datos = new clsComandoDatos();

        // ================= CONSULTAS =================

        public List<Comando> mtdListar()
        {
            return mtdLeerComandos(_datos.mtdListar());
        }

        /// <summary>Comandos que el operador puede usar en ese equipo (comando, asignación y equipo activos).</summary>
        public List<Comando> mtdListarPorEquipo(int idEquipo)
        {
            if (idEquipo <= 0) return new List<Comando>();
            return mtdLeerComandos(_datos.mtdListarPorEquipo(idEquipo));
        }

        public List<int> mtdListarEquiposAsignados(int idComando)
        {
            List<int> ids = new List<int>();
            foreach (DataRow r in _datos.mtdListarEquiposAsignados(idComando).Rows)
                ids.Add(clsLectura.mtdLeerEntero(r, "IdEquipo"));
            return ids;
        }

        public List<TipoAccion> mtdListarTiposAccion()
        {
            List<TipoAccion> lista = new List<TipoAccion>();

            foreach (DataRow r in _datos.mtdListarTiposAccion().Rows)
            {
                lista.Add(new TipoAccion
                {
                    IdTipoAccion = clsLectura.mtdLeerEntero(r, "IdTipoAccion"),
                    Codigo = clsLectura.mtdLeerTexto(r, "Codigo"),
                    Nombre = clsLectura.mtdLeerTexto(r, "Nombre"),
                    EsAccionSegura = clsLectura.mtdLeerBooleano(r, "EsAccionSegura")
                });
            }

            return lista;
        }

        // ================= ESCRITURA =================

        public Resultado mtdRegistrar(Comando c)
        {
            try
            {
                if (c == null) return Resultado.Error("No se recibieron datos del comando.");

                mtdNormalizar(c);
                string error = mtdValidar(c, null);
                if (error != null) return Resultado.Error(error);

                int id = _datos.mtdRegistrar(c.Codigo, c.Nombre, c.Frase, c.IdNivelCriticidad, c.IdTipoAccion,
                                             c.ConfianzaMinima, c.RequierePTT, c.IdsEquipos);

                clsAuditoriaNegocio.mtdRegistrar(clsAuditoriaNegocio.ModuloComandos, "REGISTRAR", "Comando", id,
                    "Comando " + c.Codigo + " \"" + c.Frase + "\" registrado (N" + c.IdNivelCriticidad + ", " +
                    c.IdsEquipos.Count + " equipo(s)).");

                return Resultado.Ok("Comando " + c.Codigo + " registrado correctamente.", id);
            }
            catch (SqlException ex) { return Resultado.Error(mtdTraducirError(ex)); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        public Resultado mtdModificar(Comando c)
        {
            try
            {
                if (c == null || c.IdComando <= 0)
                    return Resultado.Error("Seleccione un comando válido para modificar.");

                if (c.RowVersion == null)
                    return Resultado.Error("No se pudo verificar la versión del registro. Actualice la lista e intente nuevamente.");

                mtdNormalizar(c);
                string error = mtdValidar(c, c.IdComando);
                if (error != null) return Resultado.Error(error);

                int filas = _datos.mtdModificar(c.IdComando, c.Codigo, c.Nombre, c.Frase, c.IdNivelCriticidad,
                                                c.IdTipoAccion, c.ConfianzaMinima, c.RequierePTT, c.RowVersion,
                                                c.IdsEquipos);

                if (filas == 0)
                    return Resultado.Error("El comando fue modificado por otra persona o ya no existe. Actualice la lista e intente nuevamente.");

                clsAuditoriaNegocio.mtdRegistrar(clsAuditoriaNegocio.ModuloComandos, "MODIFICAR", "Comando", c.IdComando,
                    "Comando " + c.Codigo + " \"" + c.Frase + "\" modificado (N" + c.IdNivelCriticidad + ", " +
                    c.IdsEquipos.Count + " equipo(s)).");

                return Resultado.Ok("Comando " + c.Codigo + " modificado correctamente.", c.IdComando);
            }
            catch (SqlException ex) { return Resultado.Error(mtdTraducirError(ex)); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        public Resultado mtdCambiarEstado(int idComando, bool activo)
        {
            try
            {
                if (idComando <= 0) return Resultado.Error("Seleccione un comando válido.");

                int filas = _datos.mtdCambiarEstado(idComando, activo);
                if (filas == 0) return Resultado.Error("El comando ya no existe.");

                clsAuditoriaNegocio.mtdRegistrar(clsAuditoriaNegocio.ModuloComandos, activo ? "ACTIVAR" : "DESACTIVAR",
                    "Comando", idComando, activo ? "Comando activado." : "Comando desactivado.");

                return Resultado.Ok(activo ? "Comando activado correctamente." : "Comando desactivado correctamente.", idComando);
            }
            catch (SqlException ex) { return Resultado.Error(mtdTraducirError(ex)); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        // ================= REGLAS =================

        /// <summary>
        /// Deja una frase lista para comparar: MAYÚSCULAS, sin tildes, sin signos y con un solo espacio.
        /// La usa también el reconocimiento de voz para comparar lo que se escuchó.
        /// </summary>
        public static string mtdNormalizarFrase(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return string.Empty;

            string sinTildes = new string(texto.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                .ToArray()).Normalize(NormalizationForm.FormC);

            StringBuilder sb = new StringBuilder();
            foreach (char ch in sinTildes.ToUpperInvariant())
                sb.Append(char.IsLetterOrDigit(ch) ? ch : ' ');

            return string.Join(" ", sb.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }

        private static void mtdNormalizar(Comando c)
        {
            c.Codigo = (c.Codigo ?? string.Empty).Trim().ToUpperInvariant();
            c.Nombre = (c.Nombre ?? string.Empty).Trim();
            c.Frase = mtdNormalizarFrase(c.Frase);
            c.IdsEquipos = (c.IdsEquipos ?? new List<int>()).Where(id => id > 0).Distinct().ToList();
        }

        private string mtdValidar(Comando c, int? idExcluir)
        {
            if (c.Codigo.Length == 0) return "El código es obligatorio.";
            if (c.Nombre.Length == 0) return "El nombre es obligatorio.";
            if (c.Frase.Length == 0) return "La frase autorizada es obligatoria.";

            if (c.Codigo.Length > MaxCodigo) return "El código no puede superar " + MaxCodigo + " caracteres.";
            if (c.Nombre.Length > MaxNombre) return "El nombre no puede superar " + MaxNombre + " caracteres.";
            if (c.Frase.Length > MaxFrase) return "La frase no puede superar " + MaxFrase + " caracteres.";

            if (c.Frase.Split(' ').Length < MinPalabras)
                return "La frase debe tener al menos " + MinPalabras + " palabras para evitar activaciones accidentales.";

            // Las palabras de confirmación no pueden ser comandos (se confundirían en el paso de confirmación).
            if (c.Frase == "CONFIRMAR" || c.Frase == "CANCELAR")
                return "CONFIRMAR y CANCELAR están reservadas para las confirmaciones.";

            if (c.IdNivelCriticidad <= 0) return "Seleccione el nivel de criticidad.";
            if (c.IdTipoAccion <= 0) return "Seleccione el tipo de acción.";

            if (c.ConfianzaMinima < MinConfianza || c.ConfianzaMinima > MaxConfianza)
                return "La confianza mínima debe estar entre " + MinConfianza + " y " + MaxConfianza + ".";

            // Los niveles Alto y Crítico exigen una confianza mínima más estricta.
            if (c.IdNivelCriticidad >= 3 && c.ConfianzaMinima < 90m)
                return "Los comandos de nivel Alto o Crítico deben pedir al menos 90% de confianza.";

            if (c.IdsEquipos.Count == 0)
                return "Asigne el comando al menos a un equipo.";

            if (_datos.mtdExisteCodigo(c.Codigo, idExcluir))
                return "El código " + c.Codigo + " ya se encuentra registrado.";
            if (_datos.mtdExisteFrase(c.Frase, idExcluir))
                return "La frase \"" + c.Frase + "\" ya pertenece a otro comando.";

            return null;
        }

        private static string mtdTraducirError(SqlException ex)
        {
            switch (ex.Number)
            {
                case 2627:
                case 2601:
                    return "Ya existe un comando con el mismo código o la misma frase.";
                case 547:
                    return "Alguno de los datos seleccionados (criticidad, tipo de acción o equipo) no es válido.";
                case -2:
                    return "La base de datos tardó demasiado en responder. Intente nuevamente.";
                default:
                    return "Error de base de datos: " + ex.Message;
            }
        }

        // ================= MAPEO =================

        private static List<Comando> mtdLeerComandos(DataTable dt)
        {
            List<Comando> lista = new List<Comando>();

            foreach (DataRow r in dt.Rows)
            {
                lista.Add(new Comando
                {
                    IdComando = clsLectura.mtdLeerEntero(r, "IdComando"),
                    Codigo = clsLectura.mtdLeerTexto(r, "Codigo"),
                    Nombre = clsLectura.mtdLeerTexto(r, "Nombre"),
                    Frase = clsLectura.mtdLeerTexto(r, "Frase"),
                    IdNivelCriticidad = clsLectura.mtdLeerEntero(r, "IdNivelCriticidad"),
                    CriticidadNombre = clsLectura.mtdLeerTexto(r, "CriticidadNombre"),
                    NumeroConfirmaciones = clsLectura.mtdLeerEntero(r, "NumeroConfirmaciones"),
                    TimeoutSegundos = clsLectura.mtdLeerEntero(r, "TimeoutSegundos"),
                    IdTipoAccion = clsLectura.mtdLeerEntero(r, "IdTipoAccion"),
                    TipoAccionNombre = clsLectura.mtdLeerTexto(r, "TipoAccionNombre"),
                    ConfianzaMinima = r["ConfianzaMinima"] == DBNull.Value ? 0m : Convert.ToDecimal(r["ConfianzaMinima"]),
                    RequierePTT = clsLectura.mtdLeerBooleano(r, "RequierePTT"),
                    Estado = clsLectura.mtdLeerBooleano(r, "Estado"),
                    FechaRegistro = clsLectura.mtdLeerFecha(r, "FechaRegistro"),
                    FechaModificacion = clsLectura.mtdLeerFecha(r, "FechaModificacion"),
                    RowVersion = clsLectura.mtdLeerBytes(r, "RowVersion")
                });
            }

            return lista;
        }
    }
}
