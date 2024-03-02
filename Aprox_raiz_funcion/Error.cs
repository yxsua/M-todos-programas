using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aprox_raiz_funcion
{
    public class Error : Exception
    {
        // Clase básica para un error, que sobreescribe a la superclase Exception
        public Error(string mensaje, StreamWriter log) : base("Error: " + mensaje)
        {
            log.WriteLine("Error: " + mensaje);
        }

        public Error(string mensaje) : base("Error: " + mensaje)
        {
            Console.WriteLine("Error: " + mensaje);
        }
    }
}