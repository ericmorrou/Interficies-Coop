# Introducción

**Calculadora Mejorada** es una aplicación de consola desarrollada en C# con .NET 8 que implementa operaciones matemáticas básicas y científicas.

## Objetivo del proyecto

El proyecto demuestra buenas prácticas de programación en C#:

- **Separación de responsabilidades:** La clase `Calculadora` contiene únicamente la lógica matemática, mientras que `Program` gestiona la interfaz de usuario.
- **Reutilización de código:** Métodos helper como `PedirDouble` o `PedirDosNumeros` evitan la duplicación de código.
- **Manejo de excepciones:** Cada operación valida sus entradas y lanza excepciones descriptivas cuando los datos son inválidos.
- **Estructura de datos adecuada:** El historial usa una `Queue<string>` para gestionar eficientemente un límite de 5 entradas.

## Tecnologías

- **Lenguaje:** C# 12
- **Framework:** .NET 8
- **Documentación:** DocFX 2.x
