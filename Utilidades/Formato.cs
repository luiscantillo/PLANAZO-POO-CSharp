using System.Globalization;

namespace Planazo.Utilidades
{
    /// Métodos de ayuda para mostrar valores en pantalla.
    public static class Formato
    {
        private static readonly CultureInfo CulturaColombia = new CultureInfo("es-CO");

        // Muestra un decimal como pesos colombianos, ej: $57.500
        public static string Pesos(decimal valor)
        {
            return "$" + valor.ToString("N0", CulturaColombia);
        }

        // Corta un texto largo para que quepa en las tablas
        public static string Recortar(string texto, int largo)
        {
            if (texto.Length <= largo)
                return texto;
            return texto.Substring(0, largo - 3) + "...";
        }
    }
}
