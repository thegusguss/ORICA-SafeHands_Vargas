using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ORICA.Datos;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    public class clsEquipoNegocio
    {
        private const int MaxCodigo = 20;
        private const int MaxNombre = 100;
        private const int MaxDescripcion = 250;
        private const int MaxHardware = 80;
        private const int MaxUbicacion = 120;

        private readonly clsEquipoDatos _datos = new clsEquipoDatos();


        public List<Equipo> mtdListar()
        {
            return mtdLeerEquipos(_datos.mtdListar());
        }

        public List<Equipo> mtdListarActivos()
        {
            return mtdListar().Where(e => e.Estado).ToList();
        }

        public List<Equipo> mtdBuscar(string texto)
        {
            texto = (texto ?? string.Empty).Trim();
            return texto.Length == 0 ? mtdListar() : mtdLeerEquipos(_datos.mtdBuscar(texto));
        }

        public List<TipoEquipo> mtdListarTipos()
        {
            List<TipoEquipo> lista = new List<TipoEquipo>();

            foreach (DataRow r in _datos.mtdListarTipos().Rows)
            {
                lista.Add(new TipoEquipo
                {
                    IdTipoEquipo = clsLectura.mtdLeerEntero(r, "IdTipoEquipo"),
                    Codigo = clsLectura.mtdLeerTexto(r, "Codigo"),
                    Nombre = clsLectura.mtdLeerTexto(r, "Nombre"),
                    Descripcion = clsLectura.mtdLeerTexto(r, "Descripcion")
                });
            }

            return lista;
        }

        // ================= ESCRITURA =================

        public Resultado mtdRegistrar(Equipo e)
        {
            try
            {
                if (e == null) return Resultado.Error("No se recibieron datos del equipo.");

                mtdNormalizar(e);
                string error = mtdValidar(e, null);
                if (error != null) return Resultado.Error(error);

                int id = _datos.mtdRegistrar(e.Codigo, e.Nombre, e.IdTipoEquipo, e.Descripcion,
                                             e.IdentificadorHardware, e.UbicacionReferencia);

                clsAuditoriaNegocio.mtdRegistrar(clsAuditoriaNegocio.ModuloEquipos, "REGISTRAR", "Equipo", id,
                    "Equipo " + e.Codigo + " - " + e.Nombre + " registrado.");

                return Resultado.Ok("Equipo " + e.Codigo + " registrado correctamente.", id);
            }
            catch (SqlException ex) { return Resultado.Error(mtdTraducirError(ex)); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        public Resultado mtdModificar(Equipo e)
        {
            try
            {
                if (e == null || e.IdEquipo <= 0)
                    return Resultado.Error("Seleccione un equipo válido para modificar.");

                if (e.RowVersion == null)
                    return Resultado.Error("No se pudo verificar la versión del registro. Actualice la lista e intente nuevamente.");

                mtdNormalizar(e);
                string error = mtdValidar(e, e.IdEquipo);
                if (error != null) return Resultado.Error(error);

                int filas = _datos.mtdModificar(e.IdEquipo, e.Codigo, e.Nombre, e.IdTipoEquipo, e.Descripcion,
                                                e.IdentificadorHardware, e.UbicacionReferencia, e.RowVersion);

                if (filas == 0)
                    return Resultado.Error("El equipo fue modificado por otra persona o ya no existe. Actualice la lista e intente nuevamente.");

                clsAuditoriaNegocio.mtdRegistrar(clsAuditoriaNegocio.ModuloEquipos, "MODIFICAR", "Equipo", e.IdEquipo,
                    "Equipo " + e.Codigo + " - " + e.Nombre + " modificado.");

                return Resultado.Ok("Equipo " + e.Codigo + " modificado correctamente.", e.IdEquipo);
            }
            catch (SqlException ex) { return Resultado.Error(mtdTraducirError(ex)); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        public Resultado mtdCambiarEstado(int idEquipo, bool activo)
        {
            try
            {
                if (idEquipo <= 0) return Resultado.Error("Seleccione un equipo válido.");

                int filas = _datos.mtdCambiarEstado(idEquipo, activo);
                if (filas == 0) return Resultado.Error("El equipo ya no existe.");

                clsAuditoriaNegocio.mtdRegistrar(clsAuditoriaNegocio.ModuloEquipos, activo ? "ACTIVAR" : "DESACTIVAR",
                    "Equipo", idEquipo, activo ? "Equipo activado." : "Equipo desactivado.");

                return Resultado.Ok(activo ? "Equipo activado correctamente." : "Equipo desactivado correctamente.", idEquipo);
            }
            catch (SqlException ex) { return Resultado.Error(mtdTraducirError(ex)); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }

        // ================= REGLAS =================

        private static void mtdNormalizar(Equipo e)
        {
            e.Codigo = (e.Codigo ?? string.Empty).Trim().ToUpperInvariant();
            e.Nombre = (e.Nombre ?? string.Empty).Trim();
            e.Descripcion = mtdVacioANulo(e.Descripcion);
            e.IdentificadorHardware = mtdVacioANulo(e.IdentificadorHardware);
            if (e.IdentificadorHardware != null) e.IdentificadorHardware = e.IdentificadorHardware.ToUpperInvariant();
            e.UbicacionReferencia = mtdVacioANulo(e.UbicacionReferencia);
        }

        /// <summary>Devuelve null si todo es válido, o el mensaje del primer error.</summary>
        private string mtdValidar(Equipo e, int? idExcluir)
        {
            if (e.Codigo.Length == 0) return "El código es obligatorio.";
            if (e.Nombre.Length == 0) return "El nombre es obligatorio.";
            if (e.IdTipoEquipo <= 0) return "Seleccione el tipo de equipo.";

            if (e.Codigo.Length > MaxCodigo) return "El código no puede superar " + MaxCodigo + " caracteres.";
            if (e.Nombre.Length > MaxNombre) return "El nombre no puede superar " + MaxNombre + " caracteres.";
            if (e.Descripcion != null && e.Descripcion.Length > MaxDescripcion) return "La descripción no puede superar " + MaxDescripcion + " caracteres.";
            if (e.IdentificadorHardware != null && e.IdentificadorHardware.Length > MaxHardware) return "El identificador de hardware no puede superar " + MaxHardware + " caracteres.";
            if (e.UbicacionReferencia != null && e.UbicacionReferencia.Length > MaxUbicacion) return "La ubicación no puede superar " + MaxUbicacion + " caracteres.";

            if (_datos.mtdExisteCodigo(e.Codigo, idExcluir))
                return "El código " + e.Codigo + " ya se encuentra registrado.";

            // Dos equipos no pueden apuntar al mismo hardware (por ejemplo, la misma MAC del ESP32).
            if (e.IdentificadorHardware != null && _datos.mtdExisteHardware(e.IdentificadorHardware, idExcluir))
                return "El identificador de hardware " + e.IdentificadorHardware + " ya está asignado a otro equipo.";

            return null;
        }

        private static string mtdTraducirError(SqlException ex)
        {
            switch (ex.Number)
            {
                case 2627:
                case 2601:
                    return "Ya existe un equipo con el mismo código.";
                case 547:
                    return "El tipo de equipo seleccionado no existe en la base de datos.";
                case -2:
                    return "La base de datos tardó demasiado en responder. Intente nuevamente.";
                default:
                    return "Error de base de datos: " + ex.Message;
            }
        }

        private static string mtdVacioANulo(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
        }

        // ================= MAPEO =================

        private static List<Equipo> mtdLeerEquipos(DataTable dt)
        {
            List<Equipo> lista = new List<Equipo>();

            foreach (DataRow r in dt.Rows)
            {
                lista.Add(new Equipo
                {
                    IdEquipo = clsLectura.mtdLeerEntero(r, "IdEquipo"),
                    Codigo = clsLectura.mtdLeerTexto(r, "Codigo"),
                    Nombre = clsLectura.mtdLeerTexto(r, "Nombre"),
                    IdTipoEquipo = clsLectura.mtdLeerEntero(r, "IdTipoEquipo"),
                    TipoNombre = clsLectura.mtdLeerTexto(r, "TipoNombre"),
                    Descripcion = clsLectura.mtdLeerTexto(r, "Descripcion"),
                    IdentificadorHardware = clsLectura.mtdLeerTexto(r, "IdentificadorHardware"),
                    UbicacionReferencia = clsLectura.mtdLeerTexto(r, "UbicacionReferencia"),
                    Estado = clsLectura.mtdLeerBooleano(r, "Estado"),
                    UltimoHeartbeat = clsLectura.mtdLeerFecha(r, "UltimoHeartbeat"),
                    FechaRegistro = clsLectura.mtdLeerFecha(r, "FechaRegistro"),
                    FechaModificacion = clsLectura.mtdLeerFecha(r, "FechaModificacion"),
                    RowVersion = clsLectura.mtdLeerBytes(r, "RowVersion")
                });
            }

            return lista;
        }
    }
}
