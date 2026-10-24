using Planazo.Utilidades;

namespace Planazo.Modelos
{
    /// Clase abstracta base para todos los eventos de PLANAZO.
    /// No se puede instanciar directamente, solo a través de sus hijas
    /// (Cine, Concierto, Taller, Festival).
    public abstract class Evento
    {
        // Campos privados: solo se modifican a través de las propiedades o métodos
        private string nombre = string.Empty;
        private decimal precioBase;
        private int capacidadMaxima;
        private int cuposDisponibles;

        public int Id { get; }

        public string Nombre
        {
            get { return nombre; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre del evento no puede estar vacío.");
                nombre = value.Trim();
            }
        }

        public DateTime Fecha { get; private set; }

        public decimal PrecioBase
        {
            get { return precioBase; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("El precio base debe ser mayor que cero.");
                precioBase = value;
            }
        }

        public int CapacidadMaxima
        {
            get { return capacidadMaxima; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("La capacidad máxima debe ser mayor que cero.");
                capacidadMaxima = value;
            }
        }

        // Los cupos solo se leen desde afuera, para cambiarlos hay que usar
        // DescontarCupos() o DevolverCupos(), que validan los valores.
        public int CuposDisponibles
        {
            get { return cuposDisponibles; }
        }

        protected Evento(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima)
        {
            if (id <= 0)
                throw new ArgumentException("El identificador del evento debe ser positivo.");

            Id = id;
            Nombre = nombre;
            Fecha = fecha;
            PrecioBase = precioBase;
            CapacidadMaxima = capacidadMaxima;
            cuposDisponibles = capacidadMaxima; // al crearse, todos los cupos están libres
        }

        // Método abstracto: cada tipo de evento decide cómo calcula su precio (polimorfismo)
        public abstract decimal CalcularPrecioFinal();

        // Nombre del tipo para mostrar en pantalla
        public abstract string ObtenerTipo();

        public void DescontarCupos(int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad de entradas debe ser mayor que cero.");
            if (cantidad > cuposDisponibles)
                throw new InvalidOperationException(
                    $"No hay cupos suficientes. Cupos disponibles: {cuposDisponibles}, solicitados: {cantidad}.");

            cuposDisponibles -= cantidad;
        }

        public void DevolverCupos(int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad a devolver debe ser mayor que cero.");
            if (cuposDisponibles + cantidad > capacidadMaxima)
                throw new InvalidOperationException("No se pueden devolver más cupos que la capacidad máxima del evento.");

            cuposDisponibles += cantidad;
        }

        public bool TieneCupos()
        {
            return cuposDisponibles > 0;
        }

        // Es virtual para que las clases hijas agreguen sus datos propios
        public virtual string ObtenerDetalle()
        {
            return $"ID:                {Id}\n" +
                   $"Tipo:              {ObtenerTipo()}\n" +
                   $"Nombre:            {Nombre}\n" +
                   $"Fecha:             {Fecha:dd/MM/yyyy HH:mm}\n" +
                   $"Precio base:       {Formato.Pesos(PrecioBase)}\n" +
                   $"Precio final:      {Formato.Pesos(CalcularPrecioFinal())}\n" +
                   $"Capacidad máxima:  {CapacidadMaxima}\n" +
                   $"Cupos disponibles: {CuposDisponibles}";
        }

        public override string ToString()
        {
            return $"[{Id}] {Nombre} ({ObtenerTipo()})";
        }
    }
}
