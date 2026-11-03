namespace ORICA.Negocio
{
    /// <summary>
    /// Respuesta estándar de las operaciones de negocio.
    /// El formulario solo revisa Exito y muestra Mensaje.
    /// </summary>
    public class Resultado
    {
        public bool Exito { get; private set; }
        public string Mensaje { get; private set; }
        public int Id { get; private set; }

        public static Resultado Ok(string mensaje, int id = 0)
        {
            return new Resultado { Exito = true, Mensaje = mensaje, Id = id };
        }

        public static Resultado Error(string mensaje)
        {
            return new Resultado { Exito = false, Mensaje = mensaje };
        }
    }
}
