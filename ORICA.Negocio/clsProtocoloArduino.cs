using System;
using System.Linq;

namespace ORICA.Negocio
{
    /// <summary>
    /// Arma y lee las tramas que se intercambian con el Arduino (ver Arduino/ORICA_SafeHands.ino).
    /// Formato:  $&lt;secuencia&gt;,&lt;tipo&gt;[,&lt;dato&gt;...]*&lt;CRC&gt;
    /// CRC = XOR de todos los caracteres entre '$' y '*', en 2 dígitos hexadecimales.
    /// </summary>
    public static class clsProtocoloArduino
    {
        /// <summary>Calcula el CRC (XOR de 8 bits) del texto que va entre '$' y '*'.</summary>
        public static string mtdCrc(string cuerpo)
        {
            byte x = 0;
            foreach (char c in cuerpo) x ^= (byte)c;
            return x.ToString("X2");
        }

        /// <summary>Ejemplo: mtdArmar(5, "OPERAR", "SIM-001") → "$5,OPERAR,SIM-001*65".</summary>
        public static string mtdArmar(long secuencia, string tipo, params string[] datos)
        {
            string cuerpo = string.Join(",", new[] { secuencia.ToString(), tipo }.Concat(datos));
            return "$" + cuerpo + "*" + mtdCrc(cuerpo);
        }

        /// <summary>Lee una línea recibida. Devuelve null si no tiene el formato de una trama.</summary>
        public static TramaArduino mtdLeer(string linea)
        {
            linea = (linea ?? "").Trim();
            int asterisco = linea.LastIndexOf('*');
            if (!linea.StartsWith("$") || asterisco < 0) return null;

            string cuerpo = linea.Substring(1, asterisco - 1);
            string crc = linea.Substring(asterisco + 1);
            string[] partes = cuerpo.Split(',');

            long secuencia;
            if (partes.Length < 2 || !long.TryParse(partes[0], out secuencia)) return null;

            return new TramaArduino
            {
                Secuencia = secuencia,
                Tipo = partes[1],
                Datos = partes.Skip(2).ToArray(),
                Crc = crc,
                CrcValido = string.Equals(crc, mtdCrc(cuerpo), StringComparison.OrdinalIgnoreCase)
            };
        }
    }

    /// <summary>Una trama ya separada en partes.</summary>
    public class TramaArduino
    {
        public long Secuencia { get; set; }
        public string Tipo { get; set; }        // PONG, ACK, NAK, EVT
        public string[] Datos { get; set; }
        public string Crc { get; set; }
        public bool CrcValido { get; set; }

        /// <summary>Dato en la posición indicada, o "" si no viene.</summary>
        public string Dato(int i)
        {
            return Datos != null && i < Datos.Length ? Datos[i] : "";
        }
    }
}
