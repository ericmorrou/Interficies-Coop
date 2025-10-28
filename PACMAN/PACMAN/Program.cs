using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PacManSinParpadeo
{
    class Program
    {
        // El mapa (¡no importa si las líneas no son idénticas!)
        static string[] map = new string[]
        {
            "#################################################", // 49 chars
            "#C.............................................M#",
            "#...............................................#",
            "#...######...#####...######....######...##..##...#",
            "#...#.....#..#...#...#.....#...#........###.##...#",
            "#...#.....#..#...#...#.....#...#........##.###...#",
            "#...######...#...#...######....####.....##..##...#",
            "#...#...#....#...#...#.....#...#........##..##...#",
            "#...#....#...#...#...#.....#...#........##..##...#",
            "#...#.....#..#####...######....######...##..##...#",
            "#...............................................#",
            "#...............................................#",
            "#################################################"
        };

        // ARREGLO 2 (Warning CS8618): Inicializamos a 'default!'
        // para decirle al compilador que confíe en nosotros.
        static char[,] gameMap = default!;
        static int playerX, playerY;
        static int enemyX, enemyY;
        static int score = 0;
        static int totalDots = 0;
        static bool gameRunning = true;

        // --- Variables de control ---
        static int uiOffset = 4;
        static int enemyMoveCounter = 0;
        static int playerMoveCounter = 0;
        static int maxWidth = 0;
        
        static ConsoleKey playerDirection = ConsoleKey.Enter; 

        /// <summary>
        /// Método principal, contiene el Bucle de Juego.
        /// </summary>
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            InitializeGame();

            while (gameRunning)
            {
                int oldPlayerX = playerX, oldPlayerY = playerY;
                int oldEnemyX = enemyX, oldEnemyY = enemyY;

                // 1. Comprobar si se ha pulsado una NUEVA tecla.
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.UpArrow || 
                        keyInfo.Key == ConsoleKey.DownArrow || 
                        keyInfo.Key == ConsoleKey.LeftArrow || 
                        keyInfo.Key == ConsoleKey.RightArrow)
                    {
                        playerDirection = keyInfo.Key;
                    }
                    else if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        gameRunning = false;
                    }
                }

                // 2. Mover al jugador
                if (playerMoveCounter <= 0) 
                { 
                    UpdatePlayer(); 
                    playerMoveCounter = 1; // Velocidad del jugador
                }
                playerMoveCounter--;

                // 3. Mover al enemigo
                if (enemyMoveCounter <= 0) 
                { 
                    UpdateEnemy(); 
                    enemyMoveCounter = 2; // El fantasma es 2 veces más lento
                }
                enemyMoveCounter--;

                // 4. Dibujar cambios y comprobar colisiones
                DrawChanges(oldPlayerX, oldPlayerY, oldEnemyX, oldEnemyY);
                CheckCollisions();
                Thread.Sleep(50);
            }

            ShowEndScreen();
        }

        /// <summary>
        /// Prepara el juego: robusto contra mapas "irregulares".
        /// </summary>
        static void InitializeGame()
        {
            for (int y = 0; y < map.Length; y++)
            {
                if (map[y].Length > maxWidth)
                {
                    maxWidth = map[y].Length;
                }
            }
            
            CheckWindowSize(); 

            gameMap = new char[map.Length, maxWidth];
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    char c = map[y][x];
                    gameMap[y, x] = c; 
                    if (c == 'C') { playerX = x; playerY = y; gameMap[y, x] = ' '; }
                    else if (c == 'M') { enemyX = x; enemyY = y; gameMap[y, x] = ' '; }
                    else if (c == '.') { totalDots++; }
                }
                for (int x = map[y].Length; x < maxWidth; x++)
                {
                    gameMap[y, x] = ' ';
                }
            }
            
            Console.Clear();
            DrawUI();
            DrawMap();
            DrawAt(playerX, playerY, 'C', ConsoleColor.Yellow);
            DrawAt(enemyX, enemyY, 'M', ConsoleColor.Red);
        }

        /// <summary>
        /// Dibuja la UI, ahora usa 'maxWidth'.
        /// </summary>
        static void DrawUI()
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Puntuación: {score} / {totalDots}  ¡Suerte!");
            Console.WriteLine("Muros: #  Fantasma: M  Tú: C");
            Console.WriteLine("Usa las flechas. ¡Escapa con 'Escape'!");
            Console.WriteLine(new string('-', maxWidth)); // Usa maxWidth
        }

        /// <summary>
        /// Dibuja el mapa estático (muros y puntos) una sola vez.
        /// </summary>
        static void DrawMap()
        {
            for (int y = 0; y < gameMap.GetLength(0); y++)
            {
                Console.SetCursorPosition(0, y + uiOffset);
                for (int x = 0; x < gameMap.GetLength(1); x++)
                {
                    if (gameMap[y, x] == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write('#');
                    }
                    else if (gameMap[y, x] == '.')
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write('.');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Dibuja SÓLO los cambios en el jugador y el enemigo.
        /// </summary>
        static void DrawChanges(int oldPlayerX, int oldPlayerY, int oldEnemyX, int oldEnemyY)
        {
            DrawAt(oldPlayerX, oldPlayerY, gameMap[oldPlayerY, oldPlayerX]);
            DrawAt(oldEnemyX, oldEnemyY, gameMap[oldEnemyY, oldEnemyX]);
            DrawAt(enemyX, enemyY, 'M', ConsoleColor.Red);
            DrawAt(playerX, playerY, 'C', ConsoleColor.Yellow);
        }

        /// <summary>
        /// Función de ayuda para dibujar un carácter en coordenadas del MAPA.
        /// </summary>
        static void DrawAt(int x, int y, char c, ConsoleColor color = ConsoleColor.White)
        {
            Console.SetCursorPosition(x, y + uiOffset);
            Console.ForegroundColor = color;
            Console.Write(c);
            Console.ResetColor();
        }

        /// <summary>
        /// Mueve al jugador basándose en la variable 'playerDirection' guardada.
        /// </summary>
        static void UpdatePlayer()
        {
            int newX = playerX, newY = playerY;

            // Moverse según la dirección guardada
            switch (playerDirection)
            {
                case ConsoleKey.UpArrow: newY--; break;
                case ConsoleKey.DownArrow: newY++; break;
                case ConsoleKey.LeftArrow: newX--; break;
                case ConsoleKey.RightArrow: newX++; break;
                default: return; // Si es 'Enter' (inicio), no hacer nada
            }

            // Comprobar colisión con muros
            if (gameMap[newY, newX] != '#')
            {
                // Es un movimiento válido, actualizar posición
                playerX = newX;
                playerY = newY;

                // Comprobar si come un punto
                if (gameMap[newY, newX] == '.')
                {
                    score++;
                    gameMap[newY, newX] = ' '; 
                    
                    // Actualizar UI
                    Console.SetCursorPosition(12, 0);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{score} / {totalDots}  "); 
                    Console.ResetColor();
                }
            }
        }

        /// <summary>
        /// ¡NUEVA IA DE PERSECUCIÓN!
        /// El fantasma ahora te persigue calculando el camino más corto.
        /// </summary>
        static void UpdateEnemy()
        {
            // 1. Encontrar todos los movimientos válidos (que no sean un muro)
            var validMoves = new List<(int x, int y)>();
            if (gameMap[enemyY - 1, enemyX] != '#') validMoves.Add((enemyX, enemyY - 1)); // Arriba
            if (gameMap[enemyY + 1, enemyX] != '#') validMoves.Add((enemyX, enemyY + 1)); // Abajo
            if (gameMap[enemyY, enemyX - 1] != '#') validMoves.Add((enemyX - 1, enemyY)); // Izquierda
            if (gameMap[enemyY, enemyX + 1] != '#') validMoves.Add((enemyX + 1, enemyY)); // Derecha

            if (validMoves.Count == 0) return; // Atrapado, no hacer nada

            // 2. Calcular cuál de esos movimientos le acerca más al jugador
            (int x, int y) bestMove = validMoves[0];
            int bestDistance = 10000; // Un número inicial muy grande

            foreach (var move in validMoves)
            {
                // Usamos la "Distancia Manhattan" (la más simple y rápida)
                int distance = Math.Abs(move.x - playerX) + Math.Abs(move.y - playerY);

                // 3. Si este movimiento es MEJOR (distancia más corta)
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestMove = move;
                }
            }

            // 4. Mover al fantasma al mejor movimiento encontrado
            enemyX = bestMove.x;
            enemyY = bestMove.y;
        }

        /// <summary>
        /// Comprueba las condiciones de fin de juego.
        /// </summary>
        static void CheckCollisions()
        {
            if (playerX == enemyX && playerY == enemyY) gameRunning = false;
            if (score == totalDots) gameRunning = false;
        }

        /// <summary>
        /// Comprueba el tamaño de la ventana, ahora usa 'maxWidth'.
        /// </summary>
        static void CheckWindowSize()
        {
            int requiredWidth = maxWidth + 1;
            int requiredHeight = map.Length + uiOffset + 1;

            while (Console.WindowWidth < requiredWidth || Console.WindowHeight < requiredHeight)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("¡AGRANDA LA VENTANA!");
                Console.WriteLine($"Necesario: {requiredWidth} Ancho x {requiredHeight} Alto");
                
                // ¡ARREGLO 1 (Error CS0117)!
                // Aquí estaba el error. Cambiado 'Console.Height' por 'Console.WindowHeight'.
                Console.WriteLine($"Actual:     {Console.WindowWidth} Ancho x {Console.WindowHeight} Alto");
                
                Console.ResetColor();
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Muestra la pantalla final de victoria o derrota.
        /// </summary>
        static void ShowEndScreen()
        {
            Console.Clear();
            Console.SetCursorPosition(5, 5);
            if (score == totalDots)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("¡HAS GANADO! ¡Felicidades!");
                Console.SetCursorPosition(5, 6);
                Console.WriteLine("¡Seguro que 'RUBEN' está orgulloso!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("¡GAME OVER! Te ha pillado el fantasma.");
            }
            Console.ResetColor();
            Console.SetCursorPosition(5, 8);
            Console.WriteLine($"Puntuación final: {score} / {totalDots}");
            Console.SetCursorPosition(5, 10);
            Console.WriteLine("Presiona cualquier tecla para salir.");
            Console.CursorVisible = true;
            Console.ReadKey(true);
        }
    }
}