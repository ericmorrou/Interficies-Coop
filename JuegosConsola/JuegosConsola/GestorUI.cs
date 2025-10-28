using System;

namespace JuegosConsola
{
    public static class GestorUI
    {
        public static void MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================");
            Console.WriteLine("     SALA DE JUEGOS (M07)");
            Console.WriteLine("===============================");
            Console.ResetColor();
            Console.WriteLine("1. Tres en Raya");
            Console.WriteLine("2. Ahorcado");
            Console.WriteLine("3. Piedra, Papel, Tijeras, Lagarto, Spock");
            Console.WriteLine("-------------------------------");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("4. Mostrar Marcador General");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("5. Salir");
            Console.ResetColor();
            Console.Write("\nElige un juego: ");
        }

        public static void MostrarTitulo(string titulo)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"--- {titulo.ToUpper()} ---");
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void MostrarError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nERROR: {mensaje}");
            Console.ResetColor();
        }

        public static void MostrarVictoria(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n¡VICTORIA! {mensaje}");
            Console.ResetColor();
        }
        
        public static void MostrarDerrota(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n¡DERROTA! {mensaje}");
            Console.ResetColor();
        }

        public static void MostrarInfo(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(mensaje);
            Console.ResetColor();
        }

        // Pide al usuario que presione Enter para continuar
        public static void Pausa()
        {
            Console.WriteLine("\nPresiona Enter para continuar...");
            Console.ReadLine();
        }

        // Método robusto para pedir un número en un rango
        public static int PedirNumero(string prompt, int min, int max)
        {
            int numero;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out numero) && numero >= min && numero <= max)
                {
                    return numero;
                }
                else
                {
                    MostrarError($"Por favor, introduce un número válido entre {min} y {max}.");
                }
            }
        }
    }
}