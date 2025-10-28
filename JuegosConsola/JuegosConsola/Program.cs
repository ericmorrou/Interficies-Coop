using System;

namespace JuegosConsola
{
    class Program
    {
        static void Main(string[] args)
        {
            bool salir = false;
    
            while (!salir)
            {
                GestorUI.MostrarMenuPrincipal();
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        TresEnRaya.Jugar();
                        break;
                    case "2":
                        Ahorcado.Jugar();
                        break;
                    case "3":
                        PiedraPapelTijeras.Jugar(); 
                        break;
                    case "4":
                        Marcador.MostrarMarcadorActual();
                        GestorUI.Pausa();
                        break;
                    case "5":
                        salir = true;
                        Console.WriteLine("¡Gracias por jugar!");
                        break;
                    default:
                        GestorUI.MostrarError("Opción no válida. Inténtalo de nuevo.");
                        GestorUI.Pausa();
                        break;
                }
            }
        }
    }
}