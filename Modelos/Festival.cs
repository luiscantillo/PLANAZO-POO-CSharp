namespace Planazo.Modelos
{
    /// RETO 1: Nuevo tipo de evento.
    /// Un festival dura varios días. La entrada es un abono para todos los días:
    /// precio final = precio base por día * cantidad de días, con 20 % de descuento por comprar el abono.
    /// Se agregó al sistema sin tocar el cálculo de Cine, Concierto ni Taller.
    public class Festival : Evento
    {
        private const decimal DescuentoAbono = 0.20m;

        public int CantidadDias { get; private set; }

        public Festival(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima,
                        int cantidadDias)
            : base(id, nombre, fecha, precioBase, capacidadMaxima)
        {
            if (cantidadDias <= 0)
                throw new ArgumentException("El festival debe durar al menos un día.");

            CantidadDias = cantidadDias;
        }

        public override decimal CalcularPrecioFinal()
        {
            // ejemplo: 90.000 x 3 días = 270.000, menos 20 % = 216.000
            decimal precioTodosLosDias = PrecioBase * CantidadDias;
            return precioTodosLosDias - (precioTodosLosDias * DescuentoAbono);
        }

        public override string ObtenerTipo()
        {
            return "Festival";
        }

        public override string ObtenerDetalle()
        {
            return base.ObtenerDetalle() +
                   $"\nDuración:          {CantidadDias} días" +
                   $"\nRegla de precio:   (Precio base x días) - 20 % por abono";
        }
    }
}
