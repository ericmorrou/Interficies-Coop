using System;

namespace JuegosConsola
{
    public static class PiedraPapelTijeras
    {
        private enum Opcion { PIEDRA = 1, PAPEL, TIJERAS, LAGARTO, SPOCK }
        private static Random _random = new Random();

        public static void Jugar()
        {
            bool volverAlMenu = false;
            
            while (!volverAlMenu)
            {
                GestorUI.MostrarTitulo("Piedra, Papel, Tijeras, Lagarto, Spock");
                
                Opcion opcionJugador = ObtenerOpcionJugador();
                Opcion opcionCPU = (Opcion)_random.Next(1, 6);

                GestorUI.MostrarInfo($"\nTu elegiste:     {opcionJugador}");
                GestorUI.MostrarInfo($"La CPU eligió:  {opcionCPU}");
                
                DeterminarGanador(opcionJugador, opcionCPU);
                
                Console.WriteLine("\n¿Jugar de nuevo?");
                Console.WriteLine("1. Sí");
                Console.WriteLine("2. No, volver al menú principal");
                if (Console.ReadLine() == "2")
                {
                    volverAlMenu = true;
                }
            }
        }

        private static Opcion ObtenerOpcionJugador()
        {
            Console.WriteLine("Elige tu jugada:");
            Console.WriteLine("1. Piedra");
            Console.WriteLine("2. Papel");
            Console.WriteLine("3. Tijeras");
            Console.WriteLine("4. Lagarto");
            Console.WriteLine("5. Spock");
            
            int eleccion = GestorUI.PedirNumero("\nIntroduce tu opción (1-5): ", 1, 5);
            return (Opcion)eleccion;
        }

        private static void DeterminarGanador(Opcion jugador, Opcion cpu)
        {
            if (jugador == cpu)
            {
                GestorUI.MostrarInfo("¡EMPATE!");
                Marcador.RegistrarEmpate();
                return;
            }

            bool jugadorGana = false;
            string explicacion = "";
            
            switch (jugador)
            {
                case Opcion.TIJERAS:
                    jugadorGana = (cpu == Opcion.PAPEL || cpu == Opcion.LAGARTO);
                    explicacion = (cpu == Opcion.PAPEL) ? "Tijeras cortan Papel" : "Tijeras decapitan Lagarto";
                    break;
                case Opcion.PAPEL:
                    jugadorGana = (cpu == Opcion.PIEDRA || cpu == Opcion.SPOCK);
                    explicacion = (cpu == Opcion.PIEDRA) ? "Papel tapa Piedra" : "Papel desautoriza Spock";
                    break;
                case Opcion.PIEDRA:
                    jugadorGana = (cpu == Opcion.LAGARTO || cpu == Opcion.TIJERAS);
                    explicacion = (cpu == Opcion.LAGARTO) ? "Piedra aplasta Lagarto" : "Piedra aplasta Tijeras";
                    break;
                case Opcion.LAGARTO:
                    jugadorGana = (cpu == Opcion.SPOCK || cpu == Opcion.PAPEL);
                    explicacion = (cpu == Opcion.SPOCK) ? "Lagarto envenena Spock" : "Lagarto devora Papel";
                    break;
                case Opcion.SPOCK:
                    jugadorGana = (cpu == Opcion.TIJERAS || cpu == Opcion.PIEDRA);
                    explicacion = (cpu == Opcion.TIJERAS) ? "Spock rompe Tijeras" : "Spock vaporiza Piedra";
                    break;
            }

            if (jugadorGana)
            {
                GestorUI.MostrarVictoria($"¡Ganas! {explicacion}.");
                Marcador.RegistrarVictoria();
            }
            else
            {
                GestorUI.MostrarDerrota($"¡Pierdes! La CPU te gana.");
                Marcador.RegistrarDerrota();
            }
        }
    }
}