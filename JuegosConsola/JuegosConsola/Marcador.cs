using System;

namespace JuegosConsola
{
    public static class Marcador
    {
        public static int Victorias { get; private set; }
        public static int Derrotas { get; private set; }
        public static int Empates { get; private set; }

        public static void RegistrarVictoria()
        {
            Victorias++;
        }

        public static void RegistrarDerrota()
        {
            Derrotas++;
        }

        public static void RegistrarEmpate()
        {
            Empates++;
        }

        public static void MostrarMarcadorActual()
        {
            GestorUI.MostrarTitulo("Marcador General");
            GestorUI.MostrarInfo($"Victorias: {Victorias}");
            GestorUI.MostrarInfo($"Derrotas:   {Derrotas}");
            GestorUI.MostrarInfo($"Empates:    {Empates}");
        }
    }
}