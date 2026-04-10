using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculadoraMejorada
{
    /// <summary>
    /// Clase que contiene los métodos para realizar operaciones matemáticas básicas y científicas.
    /// Separa la lógica de cálculo de la interfaz de usuario.
    /// </summary>
    public class Calculadora
    {
        /// <summary>
        /// Suma dos números reales.
        /// </summary>
        /// <param name="a">Primer sumando.</param>
        /// <param name="b">Segundo sumando.</param>
        /// <returns>La suma de <paramref name="a"/> y <paramref name="b"/>.</returns>
        public double Sumar(double a, double b) => a + b;

        /// <summary>
        /// Resta el segundo número al primero.
        /// </summary>
        /// <param name="a">Minuendo.</param>
        /// <param name="b">Sustraendo.</param>
        /// <returns>La diferencia entre <paramref name="a"/> y <paramref name="b"/>.</returns>
        public double Restar(double a, double b) => a - b;

        /// <summary>
        /// Multiplica dos números reales.
        /// </summary>
        /// <param name="a">Primer factor.</param>
        /// <param name="b">Segundo factor.</param>
        /// <returns>El producto de <paramref name="a"/> y <paramref name="b"/>.</returns>
        public double Multiplicar(double a, double b) => a * b;

        /// <summary>
        /// Divide el dividendo entre el divisor.
        /// </summary>
        /// <param name="a">Dividendo.</param>
        /// <param name="b">Divisor. No puede ser cero.</param>
        /// <returns>El cociente de <paramref name="a"/> entre <paramref name="b"/>.</returns>
        /// <exception cref="DivideByZeroException">Se lanza cuando <paramref name="b"/> es 0.</exception>
        public double Dividir(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("No se puede dividir por cero.");
            return a / b;
        }

        /// <summary>
        /// Calcula la potencia de una base elevada a un exponente.
        /// </summary>
        /// <param name="a">Base de la potencia.</param>
        /// <param name="b">Exponente.</param>
        /// <returns>El resultado de elevar <paramref name="a"/> a la potencia <paramref name="b"/>.</returns>
        public double Potencia(double a, double b) => Math.Pow(a, b);

        /// <summary>
        /// Calcula la raíz cuadrada de un número real no negativo.
        /// </summary>
        /// <param name="a">Número del que se calcula la raíz cuadrada. Debe ser mayor o igual a 0.</param>
        /// <returns>La raíz cuadrada de <paramref name="a"/>.</returns>
        /// <exception cref="ArgumentException">Se lanza cuando <paramref name="a"/> es negativo.</exception>
        public double RaizCuadrada(double a)
        {
            if (a < 0)
                throw new ArgumentException("No se puede calcular la raíz cuadrada de un número negativo.");
            return Math.Sqrt(a);
        }

        /// <summary>
        /// Calcula el factorial de un número entero no negativo.
        /// El factorial de n es el producto de todos los enteros positivos desde 1 hasta n.
        /// Por convenio, el factorial de 0 es 1.
        /// </summary>
        /// <param name="n">Número entero del que se calcula el factorial. Debe estar entre 0 y 20.</param>
        /// <returns>El factorial de <paramref name="n"/> como un valor <c>long</c>.</returns>
        /// <exception cref="ArgumentException">Se lanza cuando <paramref name="n"/> es negativo.</exception>
        /// <exception cref="OverflowException">Se lanza cuando <paramref name="n"/> es mayor que 20, ya que el resultado supera la capacidad de un <c>long</c>.</exception>
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

    /// <summary>
    /// Clase principal que gestiona la interfaz de consola de la Calculadora Mejorada.
    /// Contiene el bucle principal del programa y todos los métodos auxiliares de entrada/salida.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Cola que almacena las últimas 5 operaciones realizadas.
        /// Se usa una <see cref="Queue{T}"/> para eliminar eficientemente el elemento más antiguo cuando se supera el límite.
        /// </summary>
        private static readonly Queue<string> historial = new Queue<string>(5);

        /// <summary>
        /// Punto de entrada de la aplicación. Gestiona el bucle principal del menú
        /// y delega cada operación al método correspondiente.
        /// </summary>
        /// <param name="args">Argumentos de línea de comandos (no utilizados).</param>
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

        /// <summary>
        /// Solicita al usuario los datos necesarios para calcular el precio final de un producto,
        /// aplicando IVA y descuentos opcionales (por ser socio y/o por volumen de compra).
        /// El resultado se muestra en un resumen detallado y se añade al historial.
        /// </summary>
        /// <remarks>
        /// Los descuentos aplicables son:
        /// <list type="bullet">
        ///   <item><description>5% si el cliente es socio.</description></item>
        ///   <item><description>10% si la cantidad de productos es mayor que 10.</description></item>
        /// </list>
        /// Ambos descuentos son acumulables.
        /// </remarks>
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

        /// <summary>
        /// Muestra el menú principal de la aplicación en la consola con formato de colores.
        /// Limpia la pantalla antes de mostrar las opciones disponibles.
        /// </summary>
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

        /// <summary>
        /// Muestra un mensaje de texto al usuario y espera que introduzca un número decimal.
        /// </summary>
        /// <param name="prompt">Texto a mostrar antes de leer la entrada.</param>
        /// <returns>El número decimal introducido por el usuario.</returns>
        static double PedirDouble(string prompt)
        {
            Console.Write(prompt);
            return Convert.ToDouble(Console.ReadLine());
        }

        /// <summary>
        /// Muestra un mensaje de texto al usuario y espera que introduzca un número entero.
        /// </summary>
        /// <param name="prompt">Texto a mostrar antes de leer la entrada.</param>
        /// <returns>El número entero introducido por el usuario.</returns>
        static int PedirInt(string prompt)
        {
            Console.Write(prompt);
            return Convert.ToInt32(Console.ReadLine());
        }

        /// <summary>
        /// Muestra el encabezado de una operación y solicita al usuario un único número decimal.
        /// </summary>
        /// <param name="operacion">Nombre de la operación a mostrar como encabezado.</param>
        /// <returns>El número decimal introducido por el usuario.</returns>
        static double PedirUnNumero(string operacion)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"--- {operacion} ---");
            Console.ResetColor();
            return PedirDouble("Ingrese el número: ");
        }

        /// <summary>
        /// Muestra el encabezado de una operación y solicita al usuario dos números decimales.
        /// Permite personalizar las etiquetas de cada campo.
        /// </summary>
        /// <param name="operacion">Nombre de la operación a mostrar como encabezado.</param>
        /// <param name="etiqueta1">Etiqueta para el primer número. Por defecto "Primer número".</param>
        /// <param name="etiqueta2">Etiqueta para el segundo número. Por defecto "Segundo número".</param>
        /// <returns>Una tupla con los dos números decimales introducidos por el usuario.</returns>
        static (double, double) PedirDosNumeros(string operacion, string etiqueta1 = "Primer número", string etiqueta2 = "Segundo número")
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"--- {operacion} ---");
            Console.ResetColor();
            double a = PedirDouble($"Ingrese el {etiqueta1}: ");
            double b = PedirDouble($"Ingrese el {etiqueta2}: ");
            return (a, b);
        }

        /// <summary>
        /// Muestra un mensaje de error en color rojo en la consola.
        /// </summary>
        /// <param name="mensaje">Texto del mensaje de error a mostrar.</param>
        static void MostrarError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(mensaje);
            Console.ResetColor();
        }

        /// <summary>
        /// Muestra el resultado de una operación en color verde en la consola.
        /// </summary>
        /// <param name="mensaje">Texto con el resultado a mostrar (normalmente en formato "a op b = resultado").</param>
        static void MostrarResultado(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nResultado: {mensaje}");
            Console.ResetColor();
        }

        /// <summary>
        /// Añade una operación al historial. Si el historial ya contiene 5 entradas,
        /// elimina la más antigua antes de insertar la nueva.
        /// </summary>
        /// <param name="operacion">Cadena de texto que describe la operación realizada.</param>
        static void AñadirAHistorial(string operacion)
        {
            if (historial.Count >= 5)
            {
                historial.Dequeue(); // Elimina el elemento más antiguo
            }
            historial.Enqueue(operacion); // Añade el elemento más nuevo
        }

        /// <summary>
        /// Muestra por consola el historial de las últimas operaciones realizadas.
        /// Si el historial está vacío, lo indica al usuario.
        /// </summary>
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
