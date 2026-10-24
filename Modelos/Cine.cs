namespace Planazo.Modelos
{
    /// Función de cine. Mantiene el precio base sin cambios.
    public class Cine : Evento
    {
        public string Sala { get; private set; }
        public string Clasificacion { get; private set; }

        public Cine(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima,
                    string sala, string clasificacion)
            : base(id, nombre, fecha, precioBase, capacidadMaxima)
        {
            Sala = sala;
            Clasificacion = clasificacion;
        }

        public override decimal CalcularPrecioFinal()
        {
            // El cine no tiene recargo ni descuento
            return PrecioBase;
        }

        public override string ObtenerTipo()
        {
            return "Cine";
        }

        public override string ObtenerDetalle()
        {
            return base.ObtenerDetalle() +
                   $"\nSala:              {Sala}" +
                   $"\nClasificación:     {Clasificacion}" +
                   $"\nRegla de precio:   Precio base (sin cambios)";
        }
    }
}
