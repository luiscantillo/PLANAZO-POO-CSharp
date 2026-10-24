using Planazo.Excepciones;
using Planazo.Modelos;

namespace Planazo.Servicios
{
    /// Clase que maneja toda la lógica del sistema: guarda los eventos, los usuarios
    /// y las reservas, y aplica las reglas de negocio (validar cupos, cancelar, etc).
    /// Program.cs solo se encarga de pedir datos y mostrar resultados.
    public class GestorReservas
    {
        private readonly List<Evento> eventos = new List<Evento>();
        private readonly List<Reserva> reservas = new List<Reserva>();
        private readonly List<Usuario> usuarios = new List<Usuario>();
        private int siguienteIdReserva = 1;

        public GestorReservas()
        {
            CargarEventosIniciales();
        }

        // ---------------------- EVENTOS ----------------------

        private void CargarEventosIniciales()
        {
            // 2 películas
            eventos.Add(new Cine(1, "El Último Vallenato", new DateTime(2026, 11, 6, 19, 30, 0), 18000m, 80, "Sala 3", "Todo público"));
            eventos.Add(new Cine(2, "Galaxia Perdida", new DateTime(2026, 11, 7, 21, 0, 0), 22000m, 100, "Sala VIP 1", "+12"));

            // 2 conciertos
            eventos.Add(new Concierto(3, "Noche de Salsa en el Malecón", new DateTime(2026, 11, 14, 20, 0, 0), 50000m, 300, "Orquesta La Sabrosura"));
            eventos.Add(new Concierto(4, "Rock en la Arenosa", new DateTime(2026, 11, 28, 18, 0, 0), 120000m, 500, "Los Brisas del Norte"));

            // 2 talleres
            eventos.Add(new Taller(5, "Introducción a la Fotografía", new DateTime(2026, 11, 21, 9, 0, 0), 80000m, 15, "Camila Ortega", 4));
            eventos.Add(new Taller(6, "Cocina Costeña Tradicional", new DateTime(2026, 12, 5, 10, 0, 0), 60000m, 20, "Rafael Mendoza", 3));

            // Reto 1: nuevo tipo de evento
            eventos.Add(new Festival(7, "Festival Sonidos del Caribe", new DateTime(2026, 12, 11, 15, 0, 0), 90000m, 1000, 3));
        }

        public IReadOnlyList<Evento> ObtenerEventos()
        {
            return eventos.AsReadOnly();
        }

        public Evento BuscarEvento(int idEvento)
        {
            Evento? evento = eventos.FirstOrDefault(e => e.Id == idEvento);
            if (evento == null)
                throw new EventoNoEncontradoException(idEvento);
            return evento;
        }

        // ---------------------- USUARIOS ----------------------

        public Usuario? BuscarUsuario(string documento)
        {
            return usuarios.FirstOrDefault(u => u.Documento == documento.Trim());
        }

        public Usuario RegistrarUsuario(string documento, string nombre)
        {
            if (BuscarUsuario(documento) != null)
                throw new InvalidOperationException($"Ya existe un usuario con el documento {documento}.");

            Usuario nuevo = new Usuario(documento, nombre);
            usuarios.Add(nuevo);
            return nuevo;
        }

        public IReadOnlyList<Reserva> ObtenerReservasDeUsuario(string documento)
        {
            Usuario? usuario = BuscarUsuario(documento);
            if (usuario == null)
                throw new KeyNotFoundException($"No hay ningún usuario registrado con el documento {documento}.");

            return usuario.Reservas;
        }

        // ---------------------- RESERVAS ----------------------

        // Revisa que la cantidad sea válida para ese evento. Lanza excepción si no lo es.
        public void ValidarCantidad(Evento evento, int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad de entradas debe ser mayor que cero.");

            if (cantidad > evento.CuposDisponibles)
                throw new InvalidOperationException(
                    $"No hay cupos suficientes. Disponibles: {evento.CuposDisponibles}, solicitados: {cantidad}.");
        }

        public Reserva Reservar(Usuario usuario, int idEvento, int cantidad)
        {
            Evento evento = BuscarEvento(idEvento);
            ValidarCantidad(evento, cantidad);

            // Primero se crea la reserva (valida datos) y luego se descuentan los cupos
            Reserva reserva = new Reserva(siguienteIdReserva, usuario, evento, cantidad);
            evento.DescontarCupos(cantidad);

            reservas.Add(reserva);
            usuario.AgregarReserva(reserva);
            siguienteIdReserva++;

            return reserva;
        }

        public IReadOnlyList<Reserva> ObtenerReservas()
        {
            return reservas.AsReadOnly();
        }

        public Reserva BuscarReserva(int idReserva)
        {
            Reserva? reserva = reservas.FirstOrDefault(r => r.Id == idReserva);
            if (reserva == null)
                throw new KeyNotFoundException($"No existe ninguna reserva con el ID {idReserva}.");
            return reserva;
        }

        public Reserva CancelarReserva(int idReserva)
        {
            Reserva reserva = BuscarReserva(idReserva);
            reserva.Cancelar(); // si ya estaba cancelada, lanza InvalidOperationException
            return reserva;
        }

        // ---------------------- RESUMEN ----------------------

        public int ContarReservasActivas()
        {
            return reservas.Count(r => r.EstaActiva());
        }

        public int ContarReservasCanceladas()
        {
            return reservas.Count(r => !r.EstaActiva());
        }

        public int TotalEntradasReservadas()
        {
            return reservas.Where(r => r.EstaActiva()).Sum(r => r.Cantidad);
        }

        public decimal ValorTotalReservasActivas()
        {
            return reservas.Where(r => r.EstaActiva()).Sum(r => r.ValorTotal);
        }
    }
}
