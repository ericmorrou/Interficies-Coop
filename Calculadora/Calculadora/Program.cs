using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculadoraMejorada
{
    // Contiene los métodos para realizar las operaciones matemáticas.
    public class Calculadora
    {
        public double Sumar(double a, double b) => a + b;
        public double Restar(double a, double b) => a - b;
        public double Multiplicar(double a, double b) => a * b;

        public double Dividir(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("No se puede dividir por cero.");
            return a / b;
        }

        public double Potencia(double a, double b) => Math.Pow(a, b);

        public double RaizCuadrada(double a)
        {
            if (a < 0)
                throw new ArgumentException("No se puede calcular la raíz cuadrada de un número negativo.");
            return Math.Sqrt(a);
        }

        public long Factorial(int n)
        {
            if (n < 0)
                throw new ArgumentException("El factorial no está definido para números negativos.");
            if (n > 20) // El factorial de 21 supera la capacidad de un 'long'
                throw new OverflowException("El número es demasiado grande para calcular su factorial (máximo 20).");
            if (n == 0)
                return 1;

            long resultado = 1;
            for (int i = 2; i <= n; i++)
            {
                resultado *= i;
            }
            return resultado;
        }
    }

    class Program
    {
        // Almacena las últimas 5 operaciones usando una Cola para eficiencia.
        private static readonly Queue<string> historial = new Queue<string>(5);

        static void Main(string[] args)
        {
            Calculadora calc = new Calculadora();
            bool salir = false;

            // Bucle principal del programa
            while (!salir)
            {
                MostrarMenu();
                string opcion = Console.ReadLine();
                Console.Clear();

                try
                {
                    switch (opcion)
                    {
                        case "1": // Suma
                            {
                                (double a, double b) = PedirDosNumeros("Sumar");
                                double resultado = calc.Sumar(a, b);
                                string reg = $"{a} + {b} = {resultado}";
                                MostrarResultado(reg);
                                AñadirAHistorial(reg);
                                break;
                            }
                        case "2": // Resta
                            {
                                (double a, double b) = PedirDosNumeros("Restar");
                                double resultado = calc.Restar(a, b);
                                string reg = $"{a} - {b} = {resultado}";
                                MostrarResultado(reg);
                                AñadirAHistorial(reg);
                                break;
                            }
                        case "3": // Multiplicación
                            {
                                (double a, double b) = PedirDosNumeros("Multiplicar");
                                double resultado = calc.Multiplicar(a, b);
                                string reg = $"{a} * {b} = {resultado}";
                                MostrarResultado(reg);
                                AñadirAHistorial(reg);
                                break;
                            }
                        case "4": // División
                            {
                                (double a, double b) = PedirDosNumeros("Dividir", "Dividendo", "Divisor");
                                double resultado = calc.Dividir(a, b);
                                string reg = $"{a} / {b} = {resultado}";
                                MostrarResultado(reg);
                                AñadirAHistorial(reg);
                                break;
                            }
                        case "5": // Potencia
                            {
                                (double a, double b) = PedirDosNumeros("Potencia", "Base", "Exponente");
                                double resultado = calc.Potencia(a, b);
                                string reg = $"{a} ^ {b} = {resultado}";
                                MostrarResultado(reg);
                                AñadirAHistorial(reg);
                                break;
                            }
                        case "6": // Raíz Cuadrada
                            {
                                double a = PedirUnNumero("Raíz Cuadrada");
                                double resultado = calc.RaizCuadrada(a);
                                string reg = $"√({a}) = {resultado}";
                                MostrarResultado(reg);
                                AñadirAHistorial(reg);
                                break;
                            }
                        case "7": // Factorial
                            {
                                double numDouble = PedirUnNumero("Factorial");
                                if (numDouble % 1 != 0) // Comprueba si tiene decimales
                                    throw new FormatException("El factorial solo se puede calcular para números enteros.");

                                int num = Convert.ToInt32(numDouble);
                                long resultado = calc.Factorial(num);
                                string reg = $"!{num} = {resultado}";
                                MostrarResultado(reg);
                                AñadirAHistorial(reg);
                                break;
                            }
                        case "8": // Calculadora de Precios
                            CalcularPrecioFinal();
                            break;
                        case "9": // Historial
                            MostrarHistorial();
                            break;
                        case "10": // Salir
                            salir = true;
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("¡Hasta la próxima!");
                            Console.ResetColor();
                            break;
                        default:
                            MostrarError("Opción no válida. Por favor, intente de nuevo.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MostrarError($"Error: {ex.Message}");
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        // --- Lógica de la Práctica 3 ---
        static void CalcularPrecioFinal()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("--- Calculadora de Precios (IVA y Descuentos) ---");
            Console.ResetColor();

            // 1. Pedir datos usando los nuevos helpers
            double precioBase = PedirDouble("Introduce el precio base del producto: ");
            int cantidad = PedirInt("Introduce la cantidad de productos: ");
            double porcentajeIVA = PedirDouble("Introduce el porcentaje de IVA a aplicar (ej. 21): ");

            Console.Write("¿El cliente es socio? (S/N): ");
            bool esSocio = Console.ReadLine().ToUpper() == "S";

            // 2. Cálculos
            double subtotal = precioBase * cantidad;
            double importeIVA = subtotal * (porcentajeIVA / 100.0);
            double totalConIVA = subtotal + importeIVA;
            double descuentoSocio = esSocio ? totalConIVA * 0.05 : 0; // 5%
            double descuentoVolumen = (cantidad > 10) ? totalConIVA * 0.10 : 0; // 10%
            double totalDescuentos = descuentoSocio + descuentoVolumen;
            double precioFinal = totalConIVA - totalDescuentos;

            // 3. Mostrar resumen
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n--- Resumen del Cálculo ---");
            Console.ResetColor();
            Console.WriteLine($"Precio base por unidad: {precioBase:C}");
            Console.WriteLine($"Cantidad de productos:  {cantidad}");
            Console.WriteLine($"-----------------------------------");
            Console.WriteLine($"Subtotal:                 {subtotal:C}");
            Console.WriteLine($"IVA ({porcentajeIVA}%):              + {importeIVA:C}");
            Console.WriteLine($"Total con IVA:            {totalConIVA:C}");
            if (descuentoSocio > 0)
                Console.WriteLine($"Descuento de socio (5%):  - {descuentoSocio:C}");
            if (descuentoVolumen > 0)
                Console.WriteLine($"Descuento por volumen (>10): - {descuentoVolumen:C}");
            Console.WriteLine($"-----------------------------------");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"PRECIO FINAL:             {precioFinal:C}");
            Console.ResetColor();

            string registro = $"Cálculo Precio: {cantidad}x{precioBase:C} + {porcentajeIVA}% IVA - Dtos = {precioFinal:C}";
            AñadirAHistorial(registro);
        }

        // --- Helpers de Entrada/Salida ---

        static void MostrarMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔═════════════════════════════════════╗");
            Console.WriteLine("║       CALCULADORA MEJORADA          ║");
            Console.WriteLine("╚═════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine("--- Operaciones Básicas ---");
            Console.WriteLine("1. Suma\n2. Resta\n3. Multiplicación\n4. División");
            Console.WriteLine("\n--- Operaciones Científicas ---");
            Console.WriteLine("5. Potencia\n6. Raíz Cuadrada\n7. Factorial");
            Console.WriteLine("\n--- Utilidades ---");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("8. Calculadora de Precios (IVA y Descuentos)");
            Console.ResetColor();
            Console.WriteLine("9. Ver Historial");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("10. Salir");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nSeleccione una opción: ");
            Console.ResetColor();
        }

        // Helpers reutilizables para pedir datos
        static double PedirDouble(string prompt)
        {
            Console.Write(prompt);
            return Convert.ToDouble(Console.ReadLine());
        }

        static int PedirInt(string prompt)
        {
            Console.Write(prompt);
            return Convert.ToInt32(Console.ReadLine());
        }

        static double PedirUnNumero(string operacion)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"--- {operacion} ---");
            Console.ResetColor();
            return PedirDouble("Ingrese el número: ");
        }

        static (double, double) PedirDosNumeros(string operacion, string etiqueta1 = "Primer número", string etiqueta2 = "Segundo número")
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"--- {operacion} ---");
            Console.ResetColor();
            double a = PedirDouble($"Ingrese el {etiqueta1}: ");
            double b = PedirDouble($"Ingrese el {etiqueta2}: ");
            return (a, b);
        }

        static void MostrarError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(mensaje);
            Console.ResetColor();
        }

        static void MostrarResultado(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nResultado: {mensaje}");
            Console.ResetColor();
        }
        
        static void AñadirAHistorial(string operacion)
        {
            if (historial.Count >= 5)
            {
                historial.Dequeue(); // Elimina el elemento más antiguo
            }
            historial.Enqueue(operacion); // Añade el elemento más nuevo
        }

        static void MostrarHistorial()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("--- Historial de Operaciones ---");
            Console.ResetColor();

            if (historial.Any())
            {
                foreach (var op in historial)
                {
                    Console.WriteLine($"- {op}");
                }
            }
            else
            {
                Console.WriteLine("El historial está vacío.");
            }
        }
    }
}