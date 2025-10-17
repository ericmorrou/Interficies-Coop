using System;
using System.Threading; // Necesario para Thread.Sleep

namespace MiJuegoDePacman
{
    class Program
    {
        // =======================================================
        // 1. ÁREA DE VARIABLES GLOBALES
        // =======================================================

        // El tablero (0,0 es la esquina superior izquierda)
        // GetLength(0) son las FILAS (Y)
        // GetLength(1) son las COLUMNAS (X)
        static char[,] mapa =
        {
            // 0    1    2    3    4    5    6    7    8    9
            {'#', '#', '#', '#', '#', '#', '#', '#', '#', '#'}, // 0
            {'#', '.', '.', '.', '#', '.', '.', '.', '.', '#'}, // 1
            {'#', '.', '#', '.', '#', '.', '#', '#', '.', '#'}, // 2
            {'#', '.', '#', '.', '.', '.', '.', '#', '.', '#'}, // 3
            {'#', '.', '.', '.', '#', '#', '.', '.', '.', '#'}, // 4
            {'#', '#', '#', '#', '#', '#', '#', '#', '#', '#'}  // 5
        };

        // Símbolos del mapa
        const char PARED = '#';
        const char COMIDA = '.';
        const char VACIO = ' ';
        const char JUGADOR = 'P';
        const char FANTASMA = 'G';

        // Posición del jugador
        static int pacManX = 1;
        static int pacManY = 1;

        // Posición del fantasma
        static int fantasmaX = 8;
        static int fantasmaY = 3;

        // Puntuación
        static int puntuacion = 0;

        // Para controlar el bucle del juego
        static bool gameOver = false;

        // Para generar movimientos aleatorios
        static Random random = new Random();


        // =======================================================
        // 2. EL MOTOR DEL JUEGO (Main)
        // =======================================================
        static void Main(string[] args)
        {
            // --- Configuración Inicial ---
            Console.Title = "PAC-MAN de Consola";
            Console.CursorVisible = false; // Oculta el cursor parpadeante

            // Coloca al jugador y fantasma en el mapa
            mapa[pacManY, pacManX] = JUGADOR;
            mapa[fantasmaY, fantasmaX] = FANTASMA;

            // --- El Game Loop ---
            while (!gameOver)
            {
                // 1. DIBUJAR
                DibujarMapa();

                // 2. LEER INPUT
                LeerInput();

                // 3. ACTUALIZAR LÓGICA
                if (!gameOver) // Solo mueve al fantasma si el jugador no ha perdido
                {
                    MoverFantasma();
                }

                // Controla la velocidad del juego (100ms = 10 "frames" por segundo)
                Thread.Sleep(100);
            }

            // --- Fin del Juego ---
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  ____                         ___                 ");
            Console.WriteLine(" / ___| __ _ _ __ ___   ___   / _ \\__   _____ _ __ ");
            Console.WriteLine("| |  _ / _` | '_ ` _ \\ / _ \\ | | | \\ \\ / / _ \\ '__|");
            Console.WriteLine("| |_| | (_| | | | | | |  __/ | |_| |\\ V /  __/ |   ");
            Console.WriteLine(" \\____|\\__,_|_| |_| |_|\\___|  \\___/  \\_/ \\___|_|   ");
            Console.ResetColor();
            Console.WriteLine($"\n\nPuntuación final: {puntuacion}");
            Console.ReadKey();
        }


        // =======================================================
        // 3. ÁREA DE FUNCIONES (Las "Herramientas")
        // =======================================================

        /// <summary>
        /// Dibuja el mapa completo en la consola
        /// </summary>
        static void DibujarMapa()
        {
            // ¡TRUCO CLAVE! Esto evita el parpadeo.
            // Mueve el cursor a la esquina superior izquierda (0,0)
            Console.SetCursorPosition(0, 0);

            // Recorremos las FILAS (Y)
            for (int y = 0; y < mapa.GetLength(0); y++)
            {
                // Recorremos las COLUMNAS (X)
                for (int x = 0; x < mapa.GetLength(1); x++)
                {
                    char simbolo = mapa[y, x];

                    // Pinta cada símbolo de su color
                    switch (simbolo)
                    {
                        case JUGADOR:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case FANTASMA:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case PARED:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case COMIDA:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                    }
                    Console.Write(simbolo);
                }
                Console.WriteLine(); // Salto de línea al final de cada fila
            }

            Console.ResetColor();
            Console.WriteLine($"\nPUNTUACIÓN: {puntuacion}");
        }

        /// <summary>
        /// Comprueba y procesa la entrada del teclado
        /// </summary>
        static void LeerInput()
        {
            // Si no se ha pulsado ninguna tecla, no hagas nada
            if (!Console.KeyAvailable)
            {
                return;
            }

            // Lee la tecla (true para no mostrarla en pantalla)
            ConsoleKeyInfo tecla = Console.ReadKey(true);

            // Guardamos la posición antigua por si tenemos que deshacer el movimiento
            int prevX = pacManX;
            int prevY = pacManY;

            // Borramos a Pac-Man de su posición actual
            mapa[pacManY, pacManX] = VACIO;

            // Actualizamos las coordenadas según la tecla
            switch (tecla.Key)
            {
                case ConsoleKey.UpArrow:
                    pacManY--;
                    break;
                case ConsoleKey.DownArrow:
                    pacManY++;
                    break;
                case ConsoleKey.LeftArrow:
                    pacManX--;
                    break;
                case ConsoleKey.RightArrow:
                    pacManX++;
                    break;
                case ConsoleKey.Q: // Tecla para salir
                    gameOver = true;
                    break;
            }

            // --- Comprobación de Colisiones del Jugador ---
            char simboloEnNuevaPos = mapa[pacManY, pacManX];

            if (simboloEnNuevaPos == PARED)
            {
                // ¡Choque! Deshacemos el movimiento
                pacManX = prevX;
                pacManY = prevY;
            }
            else if (simboloEnNuevaPos == COMIDA)
            {
                // ¡Comida!
                puntuacion += 10;
            }
            else if (simboloEnNuevaPos == FANTASMA)
            {
                // ¡El fantasma te ha pillado!
                gameOver = true;
            }

            // Colocamos a Pac-Man en su nueva posición (o la antigua si chocó)
            mapa[pacManY, pacManX] = JUGADOR;
        }

        /// <summary>
        /// Mueve al fantasma (IA muy simple)
        /// </summary>
        static void MoverFantasma()
        {
            int prevX = fantasmaX;
            int prevY = fantasmaY;

            // Borramos al fantasma de su posición actual
            // (Si borra comida, ¡mala suerte! Pac-Man no la puede coger)
            // (Esto se puede mejorar guardando qué había "debajo" del fantasma)
            mapa[prevY, prevX] = VACIO;

            // Elegir dirección aleatoria (0=Arriba, 1=Abajo, 2=Izda, 3=Dcha)
            int direccion = random.Next(0, 4);

            switch (direccion)
            {
                case 0: fantasmaY--; break;
                case 1: fantasmaY++; break;
                case 2: fantasmaX--; break;
                case 3: fantasmaX++; break;
            }

            // Comprobar colisión del fantasma
            char simboloEnNuevaPos = mapa[fantasmaY, fantasmaX];

            if (simboloEnNuevaPos == PARED)
            {
                // Chocó, deshacer movimiento
                fantasmaX = prevX;
                fantasmaY = prevY;
            }

            // Game Over si choca con Pac-Man
            if (fantasmaX == pacManX && fantasmaY == pacManY)
            {
                gameOver = true;
            }

            // Dibujamos al fantasma en su nueva posición
            mapa[fantasmaY, fantasmaX] = FANTASMA;
        }

    }
}