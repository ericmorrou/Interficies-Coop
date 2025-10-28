using System;

namespace JuegosConsola
{
    public static class TresEnRaya
    {
        private static char[] _tablero;
        private static char _jugadorActual;
        private static bool _contraCPU;
        private static Random _random = new Random();

        public static void Jugar()
        {
            bool volverAlMenu = false;
            while (!volverAlMenu)
            {
                MostrarSubMenu();
                string opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1": // JvJ
                        JugarPartida(false);
                        GestorUI.Pausa();
                        break;
                    case "2": // JvCPU
                        JugarPartida(true);
                        GestorUI.Pausa();
                        break;
                    case "3":
                        volverAlMenu = true;
                        break;
                    default:
                        GestorUI.MostrarError("Opción no válida.");
                        GestorUI.Pausa();
                        break;
                }
            }
        }

        private static void MostrarSubMenu()
        {
            GestorUI.MostrarTitulo("Tres en Raya");
            Console.WriteLine("1. Jugador vs Jugador");
            Console.WriteLine("2. Jugador vs CPU (Fácil)");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("3. Volver al menú principal");
            Console.ResetColor();
            Console.Write("\nElige un modo: ");
        }

        private static void JugarPartida(bool contraCPU)
        {
            _tablero = new char[9] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            _jugadorActual = 'X';
            _contraCPU = contraCPU;
            bool partidaTerminada = false;

            while (!partidaTerminada)
            {
                Console.Clear();
                DibujarTablero();

                if (_contraCPU && _jugadorActual == 'O')
                {
                    MovimientoCPU();
                }
                else
                {
                    MovimientoJugador();
                }

                if (ComprobarVictoria())
                {
                    Console.Clear();
                    DibujarTablero();
                    GestorUI.MostrarVictoria($"¡Jugador '{_jugadorActual}' ha ganado!");
                    
                    if (_contraCPU)
                    {
                        if (_jugadorActual == 'X') Marcador.RegistrarVictoria();
                        else Marcador.RegistrarDerrota();
                    }
                    
                    partidaTerminada = true;
                }
                else if (ComprobarEmpate())
                {
                    Console.Clear();
                    DibujarTablero();
                    GestorUI.MostrarInfo("¡La partida ha terminado en empate!");
                    Marcador.RegistrarEmpate();
                    partidaTerminada = true;
                }
                else
                {
                    // Cambiar de turno
                    _jugadorActual = (_jugadorActual == 'X') ? 'O' : 'X';
                }
            }
        }

        private static void DibujarTablero()
        {
            Console.WriteLine("     |     |     ");
            Console.WriteLine($"  {_tablero[0]}  |  {_tablero[1]}  |  {_tablero[2]}  ");
            Console.WriteLine("_____|_____|_____");
            Console.WriteLine("     |     |     ");
            Console.WriteLine($"  {_tablero[3]}  |  {_tablero[4]}  |  {_tablero[5]}  ");
            Console.WriteLine("_____|_____|_____");
            Console.WriteLine("     |     |     ");
            Console.WriteLine($"  {_tablero[6]}  |  {_tablero[7]}  |  {_tablero[8]}  ");
            Console.WriteLine("     |     |     \n");
        }

        private static void MovimientoJugador()
        {
            int posicion;
            while (true)
            {
                posicion = GestorUI.PedirNumero($"Turno del Jugador '{_jugadorActual}'. Elige una casilla (1-9): ", 1, 9);
                posicion--; // Ajustar a índice 0-8

                if (_tablero[posicion] == 'X' || _tablero[posicion] == 'O')
                {
                    GestorUI.MostrarError("Esa casilla ya está ocupada.");
                }
                else
                {
                    _tablero[posicion] = _jugadorActual;
                    break;
                }
            }
        }

        private static void MovimientoCPU()
        {
            Console.WriteLine("Turno de la CPU ('O')... pensando...");
            System.Threading.Thread.Sleep(1000); // Simular que piensa
            
            int posicion;
            do
            {
                posicion = _random.Next(0, 9); // Elige casilla aleatoria
            }
            // Repite si la casilla está ocupada
            while (_tablero[posicion] == 'X' || _tablero[posicion] == 'O');
            
            _tablero[posicion] = 'O';
        }

        private static bool ComprobarVictoria()
        {
            // Combinaciones ganadoras (filas, columnas, diagonales)
            int[,] lineas = new int[,]
            {
                {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Filas
                {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Columnas
                {0, 4, 8}, {2, 4, 6}             // Diagonales
            };

            for (int i = 0; i < lineas.GetLength(0); i++)
            {
                if (_tablero[lineas[i, 0]] == _jugadorActual &&
                    _tablero[lineas[i, 1]] == _jugadorActual &&
                    _tablero[lineas[i, 2]] == _jugadorActual)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool ComprobarEmpate()
        {
            // Si no hay victoria y todas las casillas están ocupadas, es empate
            foreach (char c in _tablero)
            {
                if (c != 'X' && c != 'O')
                {
                    return false; // Todavía hay casillas libres
                }
            }
            return true; // No hay casillas libres
        }
    }
}