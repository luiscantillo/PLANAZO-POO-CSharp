namespace Planazo.Utilidades
{
    /// Métodos para mostrar mensajes con colores en la consola.
    public static class Pantalla
    {
        public static void Titulo(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine($"--- {texto.ToUpper()} ---");
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void Exito(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] {mensaje}");
            Console.ResetColor();
        }

        public static void Error(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {mensaje}");
            Console.ResetColor();
        }

        public static void Aviso(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(mensaje);
            Console.ResetColor();
        }

        public static void Pausar()
        {
            Console.WriteLine();
            Console.Write("Presione ENTER para volver al menú...");
            Console.ReadLine();
        }

        public static void Limpiar()
        {
            // Console.Clear() falla si la salida está redirigida, por eso el try/catch
            try
            {
                Console.Clear();
            }
            catch (IOException)
            {
                Console.WriteLine();
            }
        }
    }
}
