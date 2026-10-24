namespace Planazo.Modelos
{
    /// RETO 2: Gestión de usuarios.
    /// Identifica a la persona que hace las reservas por su número de documento
    /// y guarda la lista de sus reservas.
    public class Usuario
    {
        private string nombre = string.Empty;
        private readonly List<Reserva> reservas = new List<Reserva>();

        public string Documento { get; }

        public string Nombre
        {
            get { return nombre; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre de la persona no puede estar vacío.");
                nombre = value.Trim();
            }
        }

        // Desde afuera solo se puede leer la lista, no agregar ni borrar
        public IReadOnlyList<Reserva> Reservas
        {
            get { return reservas.AsReadOnly(); }
        }

        public Usuario(string documento, string nombre)
        {
            if (string.IsNullOrWhiteSpace(documento))
                throw new ArgumentException("El documento no puede estar vacío.");
            if (!documento.Trim().All(char.IsDigit))
                throw new ArgumentException("El documento solo puede contener números.");

            Documento = documento.Trim();
            Nombre = nombre;
        }

        public void AgregarReserva(Reserva reserva)
        {
            if (reserva == null)
                throw new ArgumentNullException(nameof(reserva));
            if (reserva.Usuario != this)
                throw new InvalidOperationException("La reserva no pertenece a este usuario.");

            reservas.Add(reserva);
        }

        public int ContarReservasActivas()
        {
            return reservas.Count(r => r.Estado == EstadoReserva.Activa);
        }

        public decimal TotalGastado()
        {
            return reservas.Where(r => r.Estado == EstadoReserva.Activa).Sum(r => r.ValorTotal);
        }

        public override string ToString()
        {
            return $"{Nombre} (Doc. {Documento})";
        }
    }
}
