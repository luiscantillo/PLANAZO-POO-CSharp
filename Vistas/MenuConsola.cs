using Planazo.Excepciones;
using Planazo.Modelos;
using Planazo.Servicios;
using Planazo.Utilidades;

namespace Planazo.Vistas
{
    /// Se encarga de mostrar el menú, pedir los datos al usuario y mostrar resultados.
    /// No tiene reglas de negocio: todo eso se lo pide al GestorReservas.
    public class MenuConsola
    {
        private readonly GestorReservas gestor;

        public MenuConsola(GestorReservas gestor)
        {
            this.gestor = gestor;
        }

        public void Iniciar()
        {
            bool salir = false;

            while (!salir)
            {
                Pantalla.Limpiar();
                MostrarMenu();
                string opcion = (Console.ReadLine() ?? "").Trim();

                // Todas las opciones están dentro del try para que ningún error cierre el programa
                try
                {
                    switch (opcion)
                    {
                        case "1": VerEventos(); break;
                        case "2": ConsultarEvento(); break;
                        case "3": ReservarEntradas(); break;
                        case "4": VerReservas(); break;
                        case "5": CancelarReserva(); break;
                        case "6": VerResumen(); break;
                        case "7": ConsultarReservasPorUsuario(); break;
                        case "0": salir = true; break;
                        default:
                            Pantalla.Error($"La opción '{opcion}' no es válida. Elija un número del menú.");
                            break;
                    }
                }
                catch (EventoNoEncontradoException ex)
                {
                    Pantalla.Error(ex.Message);
                }
                catch (KeyNotFoundException ex)
                {
                    Pantalla.Error(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    Pantalla.Error(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    Pantalla.Error(ex.Message);
                }
                catch (Exception ex)
                {
                    // Por si pasa algo que no esperábamos
                    Pantalla.Error("Ocurrió un error inesperado: " + ex.Message);
                }

                if (!salir)
                    Pantalla.Pausar();
            }

            Console.WriteLine();
            Console.WriteLine("¡Gracias por usar PLANAZO! Hasta pronto.");
        }

        private void MostrarMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("================================");
            Console.WriteLine("        PLANAZO - EVENTOS");
            Console.WriteLine("================================");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("1. Ver eventos disponibles");
            Console.WriteLine("2. Consultar evento");
            Console.WriteLine("3. Reservar entradas");
            Console.WriteLine("4. Ver reservas realizadas");
            Console.WriteLine("5. Cancelar una reserva");
            Console.WriteLine("6. Ver resumen de reservas");
            Console.WriteLine("7. Consultar reservas por usuario");
            Console.WriteLine("0. Salir");
            Console.WriteLine();
            Console.Write("Seleccione una opción: ");
        }

        // 1. Ver eventos disponibles
        private void VerEventos()
        {
            Pantalla.Titulo("Eventos disponibles");

            Console.WriteLine($"{"ID",-3} {"Tipo",-10} {"Nombre",-30} {"Fecha",-17} {"P. base",10} {"P. final",10} {"Cupos",9}");
            Console.WriteLine(new string('-', 95));

            foreach (Evento evento in gestor.ObtenerEventos())
            {
                string cupos = evento.TieneCupos()
                    ? $"{evento.CuposDisponibles}/{evento.CapacidadMaxima}"
                    : "AGOTADO";

                // CalcularPrecioFinal() se llama igual para todos, pero cada tipo da un resultado distinto
                Console.WriteLine(
                    $"{evento.Id,-3} {evento.ObtenerTipo(),-10} {Formato.Recortar(evento.Nombre, 30),-30} " +
                    $"{evento.Fecha:dd/MM/yyyy HH:mm}  {Formato.Pesos(evento.PrecioBase),10} " +
                    $"{Formato.Pesos(evento.CalcularPrecioFinal()),10} {cupos,9}");
            }

            Console.WriteLine();
            Console.WriteLine("Reglas: Cine = precio base | Concierto = +15 % | Taller = -10 % | Festival = (base x días) -20 %");
        }

        // 2. Consultar evento
        private void ConsultarEvento()
        {
            Pantalla.Titulo("Consultar evento");

            int id = Entrada.LeerEntero("Ingrese el ID del evento: ");
            Evento evento = gestor.BuscarEvento(id); // si no existe lanza EventoNoEncontradoException

            Console.WriteLine();
            Console.WriteLine(evento.ObtenerDetalle());
        }

        // 3. Reservar entradas
        private void ReservarEntradas()
        {
            Pantalla.Titulo("Reservar entradas");

            // Datos de la persona (Reto 2: usuarios)
            string documento = Entrada.LeerDocumento("Documento de identidad: ");
            Usuario? usuario = gestor.BuscarUsuario(documento);
            string? nombreNuevo = null;

            if (usuario != null)
            {
                Pantalla.Aviso($"Bienvenido de nuevo, {usuario.Nombre}.");
            }
            else
            {
                Pantalla.Aviso("Usuario nuevo.");
                nombreNuevo = Entrada.LeerTexto("Nombre completo: ");
            }

            // Lista corta de eventos para que el usuario sepa qué ID escribir
            Console.WriteLine();
            foreach (Evento e in gestor.ObtenerEventos())
            {
                Console.WriteLine($"  [{e.Id}] {e.Nombre,-30} {Formato.Pesos(e.CalcularPrecioFinal()),10}   Cupos: {e.CuposDisponibles}");
            }
            Console.WriteLine();

            int idEvento = Entrada.LeerEntero("ID del evento: ");
            Evento evento = gestor.BuscarEvento(idEvento);
            Console.WriteLine($"Evento seleccionado: {evento.Nombre} ({evento.ObtenerTipo()})");
            Console.WriteLine($"Precio por entrada: {Formato.Pesos(evento.CalcularPrecioFinal())} | Cupos disponibles: {evento.CuposDisponibles}");
            Console.WriteLine();

            int cantidad = Entrada.LeerEntero("Cantidad de entradas: ");
            gestor.ValidarCantidad(evento, cantidad); // lanza excepción si es <= 0 o supera los cupos

            decimal total = evento.CalcularPrecioFinal() * cantidad;
            Console.WriteLine($"Total a pagar: {Formato.Pesos(total)}");
            Console.WriteLine();

            if (!Entrada.Confirmar("¿Desea confirmar la reserva?"))
            {
                Pantalla.Aviso("Reserva no confirmada. No se descontaron cupos.");
                return;
            }

            // El usuario nuevo solo se registra cuando confirma
            if (usuario == null)
                usuario = gestor.RegistrarUsuario(documento, nombreNuevo!);

            Reserva reserva = gestor.Reservar(usuario, idEvento, cantidad);

            Console.WriteLine();
            Pantalla.Exito($"Reserva #{reserva.Id} confirmada a nombre de {reserva.NombrePersona}.");
            Console.WriteLine($"     {reserva.Cantidad} entrada(s) para '{evento.Nombre}' por {Formato.Pesos(reserva.ValorTotal)}");
            Console.WriteLine($"     Cupos restantes del evento: {evento.CuposDisponibles}");
        }

        // 4. Ver reservas realizadas
        private void VerReservas()
        {
            Pantalla.Titulo("Reservas realizadas");

            IReadOnlyList<Reserva> lista = gestor.ObtenerReservas();
            if (lista.Count == 0)
            {
                Pantalla.Aviso("Todavía no se ha realizado ninguna reserva.");
                return;
            }

            MostrarTablaReservas(lista);
        }

        // 5. Cancelar una reserva
        private void CancelarReserva()
        {
            Pantalla.Titulo("Cancelar una reserva");

            if (gestor.ObtenerReservas().Count == 0)
            {
                Pantalla.Aviso("No hay reservas para cancelar.");
                return;
            }

            MostrarTablaReservas(gestor.ObtenerReservas());
            Console.WriteLine();

            int idReserva = Entrada.LeerEntero("ID de la reserva a cancelar: ");
            Reserva reserva = gestor.BuscarReserva(idReserva);
            int cuposAntes = reserva.Evento.CuposDisponibles;

            gestor.CancelarReserva(idReserva); // si ya estaba cancelada lanza InvalidOperationException

            Console.WriteLine();
            Pantalla.Exito($"La reserva #{reserva.Id} de {reserva.NombrePersona} fue cancelada.");
            Console.WriteLine($"     Se devolvieron {reserva.Cantidad} cupo(s) a '{reserva.Evento.Nombre}'.");
            Console.WriteLine($"     Cupos antes: {cuposAntes}  ->  Cupos ahora: {reserva.Evento.CuposDisponibles}");
        }

        // 6. Ver resumen de reservas
        private void VerResumen()
        {
            Pantalla.Titulo("Resumen de reservas");

            Console.WriteLine($"Reservas activas:                {gestor.ContarReservasActivas()}");
            Console.WriteLine($"Reservas canceladas:             {gestor.ContarReservasCanceladas()}");
            Console.WriteLine($"Entradas reservadas (activas):   {gestor.TotalEntradasReservadas()}");
            Console.WriteLine($"Valor total de reservas activas: {Formato.Pesos(gestor.ValorTotalReservasActivas())}");
        }

        // 7. Consultar reservas por usuario (Reto 2)
        private void ConsultarReservasPorUsuario()
        {
            Pantalla.Titulo("Reservas por usuario");

            string documento = Entrada.LeerDocumento("Documento del usuario: ");
            IReadOnlyList<Reserva> reservasUsuario = gestor.ObtenerReservasDeUsuario(documento);
            Usuario usuario = gestor.BuscarUsuario(documento)!;

            Console.WriteLine();
            Console.WriteLine($"Usuario: {usuario}");
            Console.WriteLine();

            if (reservasUsuario.Count == 0)
            {
                Pantalla.Aviso("Este usuario no tiene reservas.");
                return;
            }

            MostrarTablaReservas(reservasUsuario);
            Console.WriteLine();
            Console.WriteLine($"Reservas activas: {usuario.ContarReservasActivas()}   Total en reservas activas: {Formato.Pesos(usuario.TotalGastado())}");
        }

        private void MostrarTablaReservas(IEnumerable<Reserva> lista)
        {
            Console.WriteLine($"{"ID",-4} {"Persona",-20} {"Evento",-30} {"Cant.",5} {"Valor total",12}  {"Estado",-10}");
            Console.WriteLine(new string('-', 87));

            foreach (Reserva r in lista)
            {
                if (!r.EstaActiva())
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.WriteLine(
                    $"{r.Id,-4} {Formato.Recortar(r.NombrePersona, 20),-20} {Formato.Recortar(r.Evento.Nombre, 30),-30} " +
                    $"{r.Cantidad,5} {Formato.Pesos(r.ValorTotal),12}  {r.Estado,-10}");

                Console.ResetColor();
            }
        }
    }
}