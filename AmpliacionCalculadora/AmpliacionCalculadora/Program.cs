using System;

class Program
{
    static void Main()
    {
        double porcentaje = 0;

        Console.WriteLine("\n=== CALCULADORA DE PRECIOS ===");

        Console.Write("Introduce el precio base del producto: ");
        double pBaseUnit = Convert.ToDouble(Console.ReadLine());

        Console.Write("Cuantos productos te vas a llevar: ");
        int cantidad = Convert.ToInt32(Console.ReadLine());

        double pBase = pBaseUnit * cantidad;
        double precioInicial = pBase;

        Console.Write("Que porcentaje de IVA se aplica? \n 1. 5% \n 2. 12% \n 3. 21%: ");
        int opcion = Convert.ToInt32(Console.ReadLine());

        switch (opcion)
        {
            case 1:
                Console.WriteLine("Se calcula en base al 5%...");
                porcentaje = pBase * 0.05;
                pBase += porcentaje;
                break;

            case 2:
                Console.WriteLine("Se calcula en base al 12%...");
                porcentaje = pBase * 0.12;
                pBase += porcentaje;
                break;

            case 3:
                Console.WriteLine("Se calcula en base al 21%...");
                porcentaje = pBase * 0.21;
                pBase += porcentaje;
                break;

            default:
                Console.WriteLine("¡Error, introduce un valor válido!");
                return;
        }

        double despuesIVA = pBase;

        Console.Write("Eres socio del supermercado? ");
        string socio = Console.ReadLine().ToLower();

        if (socio == "si")
        {
            Console.WriteLine("Se le aplica un descuento de 5%.");
            pBase -= pBase * 0.05;
        }

        double despuesSocio = pBase;

        if (cantidad >= 10)
        {
            Console.WriteLine("Como tienes mas de 10 productos identicos te hacemos un descuento!");
            pBase -= pBase * 0.10;
        }

        double despuesVolumen = pBase;

        Console.WriteLine("El precio final se ha quedado en: " + pBase.ToString("F2") + " €");

        Console.Write("Deseas ver un resumen de los calculos? ");
        string mas = Console.ReadLine().ToLower();

        if (mas == "si")
        {
            Console.WriteLine("\n=== RESUMEN ===");
            Console.WriteLine("Precio inicial: " + precioInicial);
            Console.WriteLine("Despues de IVA: " + despuesIVA);
            Console.WriteLine("Despues de descuento socio: " + despuesSocio);
            Console.WriteLine("Despues de descuento por volumen: " + despuesVolumen);
        }
    }
}
