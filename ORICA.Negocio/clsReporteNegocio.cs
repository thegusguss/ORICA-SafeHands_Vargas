using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ORICA.Datos;
using ORICA.Negocio.Modelos;

namespace ORICA.Negocio
{
    // Arma el reporte de actividad: lee los eventos filtrados y calcula los totales y gráficos.
    public class clsReporteNegocio
    {
        public const int MaximoFilas = 5000;      // tope para no traer años de datos de golpe
        public const int MaximoDias = 366;        // rango máximo permitido

        private readonly clsReporteDatos _datos = new clsReporteDatos();

        // Revisa las fechas. Devuelve el mensaje de error o null si están bien.
        public string mtdValidar(FiltroReporte f)
        {
            if (f.Desde.Date > f.Hasta.Date) return "La fecha inicial no puede ser mayor que la final.";
            if (f.Hasta.Date > DateTime.Today) return "La fecha final no puede ser futura.";
            if ((f.Hasta.Date - f.Desde.Date).TotalDays >= MaximoDias) return "El rango máximo es de un año.";
            return null;
        }

        public ResultadoReporte mtdGenerar(FiltroReporte f)
        {
            ResultadoReporte r = new ResultadoReporte { Filtro = f };

            // Se pide una fila más que el máximo solo para saber si hubo recorte.
            DataTable dt = _datos.mtdListarEventos(f.Desde.Date, f.Hasta.Date.AddDays(1),
                                                   f.IdUsuario, f.Resultado, f.IdNivel, f.IdEquipo, MaximoFilas + 1);

            foreach (DataRow fila in dt.Rows)
                r.Filas.Add(LeerFila(fila));

            if (r.Filas.Count > MaximoFilas)
            {
                r.Filas.RemoveAt(r.Filas.Count - 1);
                r.Recortado = true;
            }

            // Totales
            r.Total = r.Filas.Count;
            r.Ejecutados = r.Filas.Count(x => x.Resultado == clsEventoNegocio.ResultadoEjecutado);
            r.Rechazados = r.Filas.Count(x => x.Resultado == clsEventoNegocio.ResultadoRechazado);
            r.Cancelados = r.Filas.Count(x => x.Resultado == clsEventoNegocio.ResultadoCancelado);

            // Gráfico 1: cantidad por resultado (de mayor a menor)
            r.PorResultado = r.Filas
                .GroupBy(x => x.Resultado)
                .OrderByDescending(g => g.Count())
                .Select(g => new KeyValuePair<string, int>(mtdTextoResultado(g.Key), g.Count()))
                .ToList();

            // Gráfico 2: confianza promedio por comando (solo comandos reconocidos con confianza)
            r.ConfianzaPorComando = r.Filas
                .Where(x => x.Tipo == clsEventoNegocio.TipoComandoVoz && x.Confianza.HasValue && x.Comando != null)
                .GroupBy(x => x.Comando)
                .Select(g => new ConfianzaComando
                {
                    Comando = g.Key,
                    Promedio = Math.Round(g.Average(x => x.Confianza.Value), 1),
                    Cantidad = g.Count()
                })
                .OrderByDescending(c => c.Cantidad)
                .ToList();

            return r;
        }

        // Resultado en palabras (EJECUTADO -> Ejecutado).
        public static string mtdTextoResultado(string resultado)
        {
            switch (resultado)
            {
                case clsEventoNegocio.ResultadoEjecutado: return "Ejecutado";
                case clsEventoNegocio.ResultadoRechazado: return "Rechazado";
                case clsEventoNegocio.ResultadoCancelado: return "Cancelado";
                case clsEventoNegocio.ResultadoError: return "Error";
                case clsEventoNegocio.ResultadoIgnorado: return "Ignorado";
                case clsEventoNegocio.ResultadoInformativo: return "Informativo";
                default: return resultado ?? "—";
            }
        }

        private static FilaReporte LeerFila(DataRow f)
        {
            string tipo = clsLectura.mtdLeerTexto(f, "TipoEvento");
            string codigo = clsLectura.mtdLeerTexto(f, "UsuarioCodigo");
            string frase = clsLectura.mtdLeerTexto(f, "Frase");
            string texto = clsLectura.mtdLeerTexto(f, "TextoDetectado");

            string comando;
            if (tipo == clsEventoNegocio.TipoParada) comando = "Parada de equipo";
            else if (frase != null) comando = frase;
            else if (texto != null) comando = "\"" + texto + "\" (no reconocido)";
            else comando = "No reconocido";

            return new FilaReporte
            {
                FechaHora = clsLectura.mtdLeerFecha(f, "FechaHora") ?? DateTime.MinValue,
                Tipo = tipo,
                Operador = codigo == null ? "Sistema" : codigo + " - " + clsLectura.mtdLeerTexto(f, "UsuarioNombre"),
                Comando = comando,
                Equipo = clsLectura.mtdLeerTexto(f, "EquipoCodigo") ?? "—",
                Nivel = clsLectura.mtdLeerTexto(f, "NivelCodigo") ?? "—",
                Confianza = f["Confianza"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(f["Confianza"]),
                Resultado = clsLectura.mtdLeerTexto(f, "Resultado")
            };
        }
    }
}
