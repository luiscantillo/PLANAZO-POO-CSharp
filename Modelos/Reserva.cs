namespace Planazo.Modelos
{
    /// Representa una reserva de entradas que asocia un usuario con un evento.
    public class Reserva
    {
        public int Id { get; }
        public Usuario Usuario { get; }
        public Evento Evento { get; }
        public int Cantidad { get; }
        public decimal ValorTotal { get; }
        public DateTime FechaReserva { get; }

        // El estado solo cambia con el método Cancelar()
        public EstadoReserva Estado { get; private set; }

        // Nombre de la persona (se toma del usuario asociado)
        public string NombrePersona
        {
            get { return Usuario.Nombre; }
        }

        public Reserva(int id, Usuario usuario, Evento evento, int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad de entradas debe ser mayor que cero.");

            Id = id;
            Usuario = usuario ?? throw new ArgumentNullException(nameof(usuario));
            Evento = evento ?? throw new ArgumentNullException(nameof(evento));
            Cantidad = cantidad;

            // Aquí se usa el polimorfismo: no importa qué tipo de evento sea,
            // cada uno sabe calcular su propio precio final.
            ValorTotal = evento.CalcularPrecioFinal() * cantidad;

            FechaReserva = DateTime.Now;
            Estado = EstadoReserva.Activa;
        }

        public bool EstaActiva()
        {
            return Estado == EstadoReserva.Activa;
        }

        public void Cancelar()
        {
            if (Estado == EstadoReserva.Cancelada)
                throw new InvalidOperationException($"La reserva #{Id} ya fue cancelada anteriormente.");

            // Al cancelar se devuelven los cupos al evento
            Evento.DevolverCupos(Cantidad);
            Estado = EstadoReserva.Cancelada;
        }
    }
}
