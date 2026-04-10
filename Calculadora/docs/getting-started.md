# Guía de uso

## Requisitos

- .NET 8 SDK instalado

## Ejecutar la aplicación

```bash
dotnet run --project Calculadora/Calculadora.csproj
```

## Menú principal

Al iniciar la aplicación verás el siguiente menú:

```
╔═════════════════════════════════════╗
║       CALCULADORA MEJORADA          ║
╚═════════════════════════════════════╝
--- Operaciones Básicas ---
1. Suma
2. Resta
3. Multiplicación
4. División

--- Operaciones Científicas ---
5. Potencia
6. Raíz Cuadrada
7. Factorial

--- Utilidades ---
8. Calculadora de Precios (IVA y Descuentos)
9. Ver Historial
10. Salir
```

## Calculadora de precios

La opción **8** permite calcular el precio final de un producto aplicando:

| Condición | Descuento |
|-----------|-----------|
| Cliente socio | 5% |
| Más de 10 unidades | 10% |

Ambos descuentos son acumulables y se aplican sobre el total con IVA.

## Historial

La opción **9** muestra las últimas **5 operaciones** realizadas en la sesión actual.
