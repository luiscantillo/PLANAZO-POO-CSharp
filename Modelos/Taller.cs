namespace Planazo.Modelos
{
    /// Taller. Tiene un descuento del 10 % sobre el precio base.
    public class Taller : Evento
    {
        private const decimal PorcentajeDescuento = 0.10m;

        public string Instructor { get; private set; }
        public int DuracionHoras { get; private set; }

        public Taller(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima,
                      string instructor, int duracionHoras)
            : base(id, nombre, fecha, precioBase, capacidadMaxima)
        {
            if (duracionHoras <= 0)
                throw new ArgumentException("La duración del taller debe ser mayor que cero.");

            Instructor = instructor;
            DuracionHoras = duracionHoras;
        }

        public override decimal CalcularPrecioFinal()
        {
            // Ej: 80.000 - 10 % = 72.000
            return PrecioBase - (PrecioBase * PorcentajeDescuento);
        }

        public override string ObtenerTipo()
        {
            return "Taller";
        }

        public override string ObtenerDetalle()
        {
            return base.ObtenerDetalle() +
                   $"\nInstructor:        {Instructor}" +
                   $"\nDuración:          {DuracionHoras} horas" +
                   $"\nRegla de precio:   Precio base - 10 %";
        }
    }
}
