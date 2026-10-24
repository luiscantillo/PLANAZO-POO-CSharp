using System.Globalization;
using System.Text;
using Planazo.Servicios;
using Planazo.Vistas;

// Punto de entrada del programa.
// Aquí solo se preparan los objetos; la lógica está en GestorReservas
// y el manejo del menú en MenuConsola.

Console.OutputEncoding = Encoding.UTF8; // para que se vean bien las tildes
CultureInfo.CurrentCulture = new CultureInfo("es-CO");

GestorReservas gestor = new GestorReservas();
MenuConsola menu = new MenuConsola(gestor);

menu.Iniciar();
