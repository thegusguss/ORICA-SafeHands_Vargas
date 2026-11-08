using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ORICA.Datos;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    // Reglas de los niveles de criticidad. Solo se editan confirmaciones, timeout y descripción.
    public class clsCriticidadNegocio
    {
        public const int MaxConfirmaciones = 2;   // CHECK de la BD: 0 a 2
        public const int MinTimeout = 3;          // con menos de 3 s nadie alcanza a responder
        public const int MaxTimeout = 60;
        private const int MaxDescripcion = 250;

        private readonly clsCriticidadDatos _datos = new clsCriticidadDatos();

        public List<NivelCriticidad> mtdListar()
        {
            List<NivelCriticidad> lista = new List<NivelCriticidad>();

            foreach (DataRow r in _datos.mtdListar().Rows)
            {
                lista.Add(new NivelCriticidad
                {
                    IdNivelCriticidad = clsLectura.mtdLeerEntero(r, "IdNivelCriticidad"),
                    Codigo = clsLectura.mtdLeerTexto(r, "Codigo"),
                    Nombre = clsLectura.mtdLeerTexto(r, "Nombre"),
                    NumeroConfirmaciones = clsLectura.mtdLeerEntero(r, "NumeroConfirmaciones"),
                    TimeoutSegundos = clsLectura.mtdLeerEntero(r, "TimeoutSegundos"),
                    Descripcion = clsLectura.mtdLeerTexto(r, "Descripcion"),
                    Estado = clsLectura.mtdLeerBooleano(r, "Estado")
                });
            }

            return lista;
        }

        public List<NivelCriticidad> mtdListarActivos()
        {
            return mtdListar().FindAll(n => n.Estado);
        }

        public Resultado mtdModificar(NivelCriticidad n)
        {
            try
            {
                if (n == null || n.IdNivelCriticidad <= 0)
                    return Resultado.Error("Seleccione un nivel válido.");

                string descripcion = string.IsNullOrWhiteSpace(n.Descripcion) ? null : n.Descripcion.Trim();

                if (n.NumeroConfirmaciones < 0 || n.NumeroConfirmaciones > MaxConfirmaciones)
                    return Resultado.Error("Las confirmaciones deben estar entre 0 y " + MaxConfirmaciones + ".");

                // Sin confirmaciones no hay nada que esperar: el timeout queda en 0.
                int timeout = n.NumeroConfirmaciones == 0 ? 0 : n.TimeoutSegundos;
                if (n.NumeroConfirmaciones > 0 && (timeout < MinTimeout || timeout > MaxTimeout))
                    return Resultado.Error("El timeout debe estar entre " + MinTimeout + " y " + MaxTimeout + " segundos.");

                // El nivel Crítico (N4) nunca puede quedar con menos de 2 confirmaciones.
                if (n.IdNivelCriticidad == 4 && n.NumeroConfirmaciones < 2)
                    return Resultado.Error("El nivel Crítico debe mantener doble confirmación.");

                if (descripcion != null && descripcion.Length > MaxDescripcion)
                    return Resultado.Error("La descripción no puede superar " + MaxDescripcion + " caracteres.");

                NivelCriticidad anterior = mtdListar().Find(x => x.IdNivelCriticidad == n.IdNivelCriticidad);

                int filas = _datos.mtdModificar(n.IdNivelCriticidad, n.NumeroConfirmaciones, timeout, descripcion);
                if (filas == 0) return Resultado.Error("El nivel ya no existe.");

                clsAuditoriaNegocio.mtdRegistrar(clsAuditoriaNegocio.ModuloCriticidad, "MODIFICAR", "NivelCriticidad",
                    n.IdNivelCriticidad, "Nivel " + n.Nombre + " modificado.",
                    anterior == null ? null : "Confirmaciones=" + anterior.NumeroConfirmaciones + "; Timeout=" + anterior.TimeoutSegundos + " s",
                    "Confirmaciones=" + n.NumeroConfirmaciones + "; Timeout=" + timeout + " s");

                return Resultado.Ok("Nivel " + n.Nombre + " actualizado correctamente.", n.IdNivelCriticidad);
            }
            catch (SqlException ex) { return Resultado.Error("Error de base de datos: " + ex.Message); }
            catch (Exception ex) { return Resultado.Error("Error inesperado: " + ex.Message); }
        }
    }
}
