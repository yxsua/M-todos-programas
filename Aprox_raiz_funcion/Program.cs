using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace Aprox_raiz_funcion
{
    class Program
    {
        static void Main(string[] args)
        {
            // Pedir al usuario que ingrese la función
            Console.Write("Ingresa la función f(x): ");
            string funcionStr = Console.ReadLine();

            // Convertir la función ingresada en términos del usuario a una expresión 
            // que pueda convertirse en expresión lambda
            try
            {
                using (Funcion F = new Funcion(funcionStr))
                {
                    // Se analizará cada expresión de manera individual
                    // siempre y cuando no se acabe el archivo
                    while (!F.finArchivo())
                    {
                        F.nextExpression();
                    }
                }
            }
            // Hay que manejar las excepciones necesarias en la clase principal
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            StreamReader funcionTransformada = new StreamReader("funcionSalida.log");
            if (!File.Exists("funcionSalida.log"))
            {
                throw new Error("El archivo funcionSalida.log no existe");
            }

            funcionStr = "";
            while (!funcionTransformada.EndOfStream)
            {
                funcionStr += (char)funcionTransformada.Read();
            }

            // Convertir la cadena de texto ingresada por el usuario ya convertida en una expresión lambda
            ParameterExpression x = Expression.Parameter(typeof(double), "x");
            Expression<Func<double, double>> funcionExp = (Expression<Func<double, double>>)System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda(new[] { x }, null, funcionStr);

            // Compilar la expresión lambda
            Func<double, double> f = funcionExp.Compile();

            // Mostrar la función ingresada
            Console.WriteLine("Función ingresada: f(x) = " + funcionStr);

            // Aplicar el método de la secante para encontrar una raíz aproximada
            double x2, error;

            Console.WriteLine("Ingresa 2 valores iniciales: ");
            Console.WriteLine("x0: ");
            if (!double.TryParse(Console.ReadLine(), out double x0))
            {
                Console.WriteLine("La entrada no es un número decimal válido.");
            }
            Console.WriteLine("x1: ");
            if (!double.TryParse(Console.ReadLine(), out double x1))
            {
                Console.WriteLine("La entrada no es un número decimal válido.");
            }

            Console.WriteLine("¿Cuál porcentaje de error se va a usar?");
            if (!double.TryParse(Console.ReadLine(), out double errorMinimo))
            {
                Console.WriteLine("La entrada no es un número decimal válido.");
            }

            // Aplicamos el método de la secante
            do
            {
                x2 = x1 - (f(x1) * (x0 - x1)) / (f(x0) - f(x1));
                error = ErrorRelativo.calcularErrorRelativo(x2, x1);
                x0 = x1;
                x1 = x2;
            } while (error > errorMinimo);

            // Mostrar el resultado
            Console.WriteLine("x = " + x2.Truncar(5));
            Console.WriteLine("f(" + x2.Truncar(5) + ") = " + f(x2) + " ~= 0");
        }
    }

    static class ErrorRelativo
    {
        public static double calcularErrorRelativo(double c1, double c2)
        {
            // Err = (valor actual - valor anterior)/valor actual*100
            return Math.Abs((c1 - c2) / c1) * 100;
        }
    }

    static class Truncate
    {
        public static double Truncar(this double valor, int decimales)
        {
            double auxiliar = Math.Pow(10, decimales);
            return (Math.Truncate(valor * auxiliar) / auxiliar);
        }
    }
}