using System;

namespace ORICA.Negocio.Modelos
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Codigo { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Documento { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public int IdRol { get; set; }
        public string RolNombre { get; set; }
        public string RolCodigo { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public byte[] RowVersion { get; set; }
        public string Credenciales { get; set; }   // ej. "PIN, RFID"; null si no tiene ninguna

        // Propiedades auxiliares para la interfaz
        public string NombreCompleto
        {
            get { return $"{Nombres} {Apellidos}".Trim(); }
        }

        public string EstadoTexto
        {
            get { return Estado ? "Activo" : "Suspendido"; }
        }

        /// <summary>Texto para listas de selección, p. ej. "AD-001 - Marco Salas (Administrador)".</summary>
        public string Etiqueta
        {
            get { return $"{Codigo} - {NombreCompleto} ({RolNombre})"; }
        }
    }
}
