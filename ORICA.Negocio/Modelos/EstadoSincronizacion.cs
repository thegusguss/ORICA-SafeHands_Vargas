using System;

namespace ORICA.Negocio.Modelos
{
    /// <summary>Cómo está la conexión con Azure y la copia local (lo muestra la pantalla Sincronización).</summary>
    public class EstadoSincronizacion
    {
        public bool TieneAzure { get; set; }        // hay cadena de Azure en conexion.config
        public bool EnLinea { get; set; }           // se está trabajando con Azure
        public bool Simulado { get; set; }          // desconexión simulada para pruebas
        public int Pendientes { get; set; }         // registros locales que faltan subir
        public DateTime? FechaCopia { get; set; }   // última copia desde Azure (null = nunca)
        public DateTime? UltimaSubida { get; set; } // última vez que se subieron pendientes
        public string UltimoError { get; set; }
        public bool Ocupado { get; set; }           // hay una sincronización en curso (no se leyeron los datos)
    }

    /// <summary>Una fila del historial de dbo.Sincronizacion.</summary>
    public class RegistroSincronizacion
    {
        public long Id { get; set; }
        public string Tabla { get; set; }
        public string Detalle { get; set; }
        public int Intentos { get; set; }
        public string Estado { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
