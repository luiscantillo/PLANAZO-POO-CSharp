namespace Planazo.Utilidades
{
    /// Métodos para leer datos del teclado sin que el programa se caiga
    /// si el usuario escribe algo incorrecto.
    public static class Entrada
    {
        public static int LeerEntero(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string? texto = Console.ReadLine();

                if (int.TryParse(texto, out int numero))
                    return numero;

                Pantalla.Error("Debe ingresar un número entero válido. Intente de nuevo.");
            }
        }

        public static string LeerTexto(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string? texto = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(texto))
                    return texto.Trim();

                Pantalla.Error("Este campo no puede estar vacío.");
            }
        }

        public static string LeerDocumento(string mensaje)
        {
            while (true)
            {
                string documento = LeerTexto(mensaje);

                if (documento.All(char.IsDigit) && documento.Length >= 5)
                    return documento;

                Pantalla.Error("El documento debe tener solo números (mínimo 5 dígitos).");
            }
        }

        public static bool Confirmar(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje + " (S/N): ");
                string respuesta = (Console.ReadLine() ?? "").Trim().ToUpper();

                if (respuesta == "S") return true;
                if (respuesta == "N") return false;

                Pantalla.Error("Responda S para sí o N para no.");
            }
        }
    }
}
