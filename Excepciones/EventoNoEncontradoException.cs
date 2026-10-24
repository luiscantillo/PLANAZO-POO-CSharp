namespace Planazo.Excepciones
{
    /// Excepción propia que se lanza cuando se busca un evento con un ID que no existe.
    public class EventoNoEncontradoException : Exception
    {
        public int IdBuscado { get; }

        public EventoNoEncontradoException(int idBuscado)
            : base($"No existe ningún evento con el ID {idBuscado}.")
        {
            IdBuscado = idBuscado;
        }
    }
}
