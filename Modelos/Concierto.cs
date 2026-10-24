namespace Planazo.Modelos
{
    /// Concierto. Su precio final es el precio base + 15 %.
    public class Concierto : Evento
    {
        private const decimal PorcentajeRecargo = 0.15m;

        public string Artista { get; private set; }

        public Concierto(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima,
                         string artista)
            : base(id, nombre, fecha, precioBase, capacidadMaxima)
        {
            Artista = artista;
        }

        public override decimal CalcularPrecioFinal()
        {
            // ejemplo: 50.000 + 15 % = 57.500
            return PrecioBase + (PrecioBase * PorcentajeRecargo);
        }

        public override string ObtenerTipo()
        {
            return "Concierto";
        }

        public override string ObtenerDetalle()
        {
            return base.ObtenerDetalle() +
                   $"\nArtista:           {Artista}" +
                   $"\nRegla de precio:   Precio base + 15 %";
        }
    }
}
