using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JuegosConsola
{
    public static class Ahorcado
    {
        // Lista de palabras por defecto si no se encuentra el archivo
        private static string[] _palabrasPorDefecto = {
            "PATATA", "ORDENADOR", "CONSOLA", "INTERFAZ", "PROGRAMA", "JUEGO"
        };
        private static string _palabraSecreta;
        private static char[] _progresoPalabra;
        private static int _intentosRestantes;
        private static List<char> _letrasUsadas;
        private static Random _random = new Random();

        public static void Jugar()
        {
            bool volverAlMenu = false;
            while (!volverAlMenu)
            {
                InicializarPartida();
                JugarPartida(); // El bucle del juego está dentro

                GestorUI.MostrarInfo("\n¿Jugar de nuevo al Ahorcado?");
                Console.WriteLine("1. Sí");
                Console.WriteLine("2. No, volver al menú principal");
                if (Console.ReadLine() == "2")
                {
                    volverAlMenu = true;
                }
            }
        }

        private static void InicializarPartida()
        {
            _palabraSecreta = SeleccionarPalabra();
            _progresoPalabra = new char[_palabraSecreta.Length];
            for (int i = 0; i < _progresoPalabra.Length; i++)
            {
                _progresoPalabra[i] = '_';
            }
            _intentosRestantes = 6;
            _letrasUsadas = new List<char>();
        }

        private static string SeleccionarPalabra()
        {
            // Extensión Opcional: Cargar desde archivo
            try
            {
                string[] palabrasDeArchivo = File.ReadAllLines("palabras.txt");
                if (palabrasDeArchivo.Length > 0)
                {
                    return palabrasDeArchivo[_random.Next(palabrasDeArchivo.Length)].ToUpper();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nota: No se pudo cargar 'palabras.txt'. Usando palabras por defecto. ({ex.Message})");
            }
            return _palabrasPorDefecto[_random.Next(_palabrasPorDefecto.Length)];
        }

        private static void JugarPartida()
        {
            bool partidaTerminada = false;

            while (!partidaTerminada)
            {
                GestorUI.MostrarTitulo("Juego del Ahorcado");
                DibujarMuneco();
                MostrarEstado();

                // Pedir letra o palabra
                Console.Write("\nIntroduce una letra (o '!' para adivinar la palabra): ");
                string entrada = Console.ReadLine().ToUpper();

                if (string.IsNullOrEmpty(entrada)) continue;

                if (entrada.StartsWith("!")) // Extensión: Adivinar palabra
                {
                    if (AdivinarPalabra(entrada.Substring(1)))
                    {
                        partidaTerminada = true;
                    }
                }
                else // Adivinar letra
                {
                    ProcesarLetra(entrada[0]);
                }

                // Comprobar estado del juego
                if (_intentosRestantes <= 0)
                {
                    partidaTerminada = true;
                    GestorUI.MostrarTitulo("Juego del Ahorcado");
                    DibujarMuneco();
                    GestorUI.MostrarDerrota($"¡Has perdido! La palabra era: {_palabraSecreta}");
                    Marcador.RegistrarDerrota();
                }
                else if (!new string(_progresoPalabra).Contains('_'))
                {
                    partidaTerminada = true;
                    GestorUI.MostrarTitulo("Juego del Ahorcado");
                    GestorUI.MostrarVictoria($"¡Has adivinado la palabra: {_palabraSecreta}!");
                    Marcador.RegistrarVictoria();
                }
            }
        }

        private static void ProcesarLetra(char letra)
        {
            if (!Char.IsLetter(letra))
            {
                GestorUI.MostrarError("Eso no es una letra.");
                GestorUI.Pausa();
                return;
            }
            
            if (_letrasUsadas.Contains(letra))
            {
                GestorUI.MostrarInfo($"Ya has usado la letra '{letra}'.");
                GestorUI.Pausa();
                return;
            }

            _letrasUsadas.Add(letra);

            if (_palabraSecreta.Contains(letra))
            {
                // Acierto
                for (int i = 0; i < _palabraSecreta.Length; i++)
                {
                    if (_palabraSecreta[i] == letra)
                    {
                        _progresoPalabra[i] = letra;
                    }
                }
            }
            else
            {
                // Fallo
                GestorUI.MostrarError($"La letra '{letra}' no está.");
                _intentosRestantes--;
                GestorUI.Pausa();
            }
        }
        
        private static bool AdivinarPalabra(string intento)
        {
            if (intento == _palabraSecreta)
            {
                Array.Copy(_palabraSecreta.ToCharArray(), _progresoPalabra, _palabraSecreta.Length);
                return true; // Ganó
            }
            else
            {
                GestorUI.MostrarError("Esa no es la palabra correcta.");
                _intentosRestantes--; // Penalización por fallo
                GestorUI.Pausa();
                return false;
            }
        }

        private static void MostrarEstado()
        {
            // Muestra la palabra con guiones
            Console.WriteLine($"Palabra: {string.Join(" ", _progresoPalabra)}");
            Console.WriteLine($"Intentos restantes: {_intentosRestantes}");
            
            // Muestra las letras usadas
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Letras usadas: {string.Join(", ", _letrasUsadas)}");
            Console.ResetColor();
        }

        private static void DibujarMuneco()
        {
            // Dibuja el estado del ahorcado según los intentos
            Console.WriteLine("  +---+");
            Console.WriteLine($"  |   {(_intentosRestantes < 6 ? "O" : "")}");
            Console.WriteLine($"  |  {(_intentosRestantes < 4 ? "/" : " ")}{(_intentosRestantes < 5 ? "|" : "")}{(_intentosRestantes < 3 ? "\\" : "")}");
            Console.WriteLine($"  |  {(_intentosRestantes < 2 ? "/" : " ")} {(_intentosRestantes < 1 ? "\\" : "")}");
            Console.WriteLine("  |    ");
            Console.WriteLine("=========");
        }
    }
}