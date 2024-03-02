using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Aprox_raiz_funcion
{
    public class Funcion : Expresion, IDisposable
    {
        // La funcion en términos del usuario se ingresará en un archivo,
        // y en otro archivo irá la función de salida con la función
        // transformada a expresiones lambda
        private StreamReader funcionEntrada;
        public StreamWriter funcionSalida;
        private StreamWriter escribirFuncionEntrada;
        private string funcion;
        public Funcion(string funcion) // Constructor
        {
            // El constructor con defecto trabajará siempre con un archivo "funcion"
            funcionSalida = new StreamWriter("funcionSalida.log");
            funcionSalida.AutoFlush = true;

            // Se arroja esta excepción si no existe el archivo
            if (!File.Exists("funcionEntrada.log"))
            {
                throw new Error("El archivo funcionEntrada.log no existe", funcionSalida);
            }
            escribirFuncionEntrada = new StreamWriter("funcionEntrada.log");
            escribirFuncionEntrada.WriteLine(funcion);
            escribirFuncionEntrada.Close();

            funcionEntrada = new StreamReader("funcionEntrada.log");
        }

        public void Dispose() // Destructor
        {
            funcionEntrada.Close();
            funcionSalida.Close();
        }

        /*
        numero -> D+ (. D+)? (E(+|-)? D+)?
        x^n -> (x*)* x
        seno -> Math.Sin
        coseno -> Math.Cos
        tangente -> Math.Tan
        arcoseno -> Math.Asin
        arcocoseno -> Math.Acos
        arcotangente -> Math.Atan
        seno hiperbólico -> Math.Sinh
        coseno hiperbólico -> Math.Cosh
        tangente hiperbólico -> Math.Tanh
        arcoseno hiperbólico -> Math.Asinh
        arcocoseno hiperbólico -> Math.Acosh
        arcotangente hiperbólico -> Math.Atanh
        exponencial -> Math.Exp
        Raiz cuadrada -> Math.Sqrt
        Raiz cúbica -> Math.Cbrt
        OpTermino -> + | -
        OpFactor -> * | /
        */

        public void nextExpression()
        {
            char c;
            string buffer = "";

            // Tenemos que leer las expresiones ignorando el espacio en blanco
            while (char.IsWhiteSpace(c = (char)funcionEntrada.Read()))
            {
            }

            buffer += c; // buffer = buffer + c;
            if (c == 'e')
            {
                buffer = "Math.Exp(1)";
                if ((c = (char)funcionEntrada.Peek()) == '^')
                {
                    buffer = "Math.Exp";
                    funcionEntrada.Read();
                }
                else if (!char.IsWhiteSpace(c = (char)funcionEntrada.Peek()) && c != ')')
                {
                    buffer += "*";
                }
            }
            else if (c == 'x')
            {
                // Si el identificador empieza con x, entonces lo tratamos como una variable
                // con grado n
                setClasificacion(Tipos.vpot);

                // Si después de la x sigue un ^, entonces hay que añadir la cantidad de x necesarias al archivo de salida
                if ((c = (char)funcionEntrada.Peek()) == '^')
                {
                    funcionEntrada.Read();
                    if (char.IsDigit((c = (char)funcionEntrada.Peek())))
                    {
                        for (int i = 1; i < c - 48; i++)
                        {
                            buffer += "*x";
                        }
                        funcionEntrada.Read();
                    }
                    else
                    {
                        buffer = "Math.Pow(x, ";
                        funcionEntrada.Read();
                    }
                }
            }
            else if (char.IsLetter(c))
            {
                // Si no es una letra o un dígito, entonces no es necesario
                // concatenar más
                while (char.IsLetterOrDigit(c = (char)funcionEntrada.Peek()))
                {
                    buffer += c;
                    funcionEntrada.Read();
                }

                // Convertimos a minúsculas para evitar problemas con mayúsculas
                buffer = buffer.ToLower();

                // Hay que revisar que el identificador ingresado corresponda a alguna función
                // trigonométrica, exponencial o logarítmica
                if (buffer == "cos")
                {
                    setClasificacion(Tipos.cos);
                    buffer = "Math.Cos((1/57.29577)*";
                    funcionEntrada.Read();
                }
                else if (buffer == "sen" || buffer == "sin")
                {
                    setClasificacion(Tipos.sen);
                    buffer = "Math.Sin((1/57.29577)*";
                    funcionEntrada.Read();
                }
                else if (buffer == "tan")
                {
                    setClasificacion(Tipos.tan);
                    buffer = "Math.Tan((1/57.29577)*";
                    funcionEntrada.Read();
                }
                else if (buffer == "arccos")
                {
                    setClasificacion(Tipos.acos);
                    buffer = "Math.Acos";
                }
                else if (buffer == "arcsen" || buffer == "arcsin")
                {
                    setClasificacion(Tipos.asen);
                    buffer = "Math.Asin";
                }
                else if (buffer == "arctan")
                {
                    setClasificacion(Tipos.atan);
                    buffer = "Math.Atan";
                }
                else if (buffer == "cosh")
                {
                    setClasificacion(Tipos.cosh);
                    buffer = "Math.Cosh";
                }
                else if (buffer == "senh")
                {
                    setClasificacion(Tipos.senh);
                    buffer = "Math.Sinh";
                }
                else if (buffer == "tanh")
                {
                    setClasificacion(Tipos.tanh);
                    buffer = "Math.Tanh";
                }
                else if (buffer == "cos")
                {
                    setClasificacion(Tipos.acosh);
                    buffer = "Math.Acosh";
                }
                else if (buffer == "sen")
                {
                    setClasificacion(Tipos.asenh);
                    buffer = "Math.Asinh";
                }
                else if (buffer == "tan")
                {
                    setClasificacion(Tipos.atanh);
                    buffer = "Math.Atanh";
                }
                else if (buffer == "ln")
                {
                    setClasificacion(Tipos.log);
                    buffer = "Math.Log";
                }
                else if (buffer == "sqrt" || buffer == "raizcua")
                {
                    setClasificacion(Tipos.log);
                    buffer = "Math.Sqrt";
                }
                else if (buffer == "cbrt" || buffer == "raizcub")
                {
                    setClasificacion(Tipos.log);
                    buffer = "Math.Cbrt";
                }
            }
            else if (char.IsDigit(c))
            {
                // Si empieza con un dígito, entonces es un número
                // Un número puede tener una cantidad n de dígitos,
                // tener parte fraccionaría y una parte exponencial
                // con o sin signo (sin signo es considerado positivo)
                setClasificacion(Tipos.con);

                // Si no hay más dígitos, no es necesario seguir concatenando
                while (char.IsDigit(c = (char)funcionEntrada.Peek()))
                {
                    buffer += c;
                    funcionEntrada.Read();
                }

                // Si después del dígito hay un punto, concatenamos la parte fraccional
                if (c == '.')
                {
                    // Parte fraccional
                    funcionEntrada.Read();
                    buffer += c;
                    if (char.IsDigit(c = (char)funcionEntrada.Peek()))
                    {
                        // Se concatenan números hasta que ya no haya más dígitos
                        while (char.IsDigit(c = (char)funcionEntrada.Peek()))
                        {
                            buffer += c;
                            funcionEntrada.Read();
                        }
                    }
                }

                // Revisar si el número tiene otra expresión a la que está multiplicando
                if (!char.IsWhiteSpace(c = (char)funcionEntrada.Peek()) && c != ')' && !char.IsDigit(c) && c != '/' && c != '*' && c != '+' && c != '-')
                {
                    buffer += "*";
                }

            }
            else if (c == '+' || c == '-')
            {
                // Los símbolos de + o - separan las expresiones
                setClasificacion(Tipos.OpTermino);
            }
            else if (c == '*' || c == '/' || c == '%')
            {
                // Los símbolos de * o / pueden unir a varias funciones
                setClasificacion(Tipos.OpFactor);

            }
            else if (c == '(')
            {
                // Los argumentos de las funciones empiezan con (
                setClasificacion(Tipos.InicioArgumento);
            }
            else if (c == ')')
            {
                // Y terminan con un )
                setClasificacion(Tipos.FinArgumento);
            }
            else
            {
                buffer = "";
            }

            // Añadimos el contenido el buffer al contenido de la expresión
            setContenido(buffer);
            funcionSalida.Write(getContenido());
        }

        public bool finArchivo()
        {
            return funcionEntrada.EndOfStream;
        }
    }
}