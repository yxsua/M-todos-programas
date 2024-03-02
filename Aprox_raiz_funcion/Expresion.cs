using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aprox_raiz_funcion
{
    public class Expresion
    {
        // Todos los tipos de tokens que estamos considerando
        public enum Tipos
        {
            // Variables a la n & constantes
            con, vpot,
            // Otros símbolos
            OpTermino, OpFactor, InicioArgumento, FinArgumento,
            // Funciones trigonométricas regulares
            sen, cos, tan,
            // Funciones trigonométricas inversas
            asen, acos, atan,
            // Funciones trigonométricas regulares hiperbólicas
            senh, cosh, tanh,
            // Funciones trigonométricas inversas hiperbólicas
            asenh, acosh, atanh,
            // Exponencial & logarítmica
            e, log,
            // Raíces
            sqrt, cbrt
        };

        // Los tokens tendrán contenido y clasificación
        private string contenido;
        private Tipos clasificacion;
        public Expresion()
        {
            contenido = "";
        }
        public void setContenido(string contenido)
        {
            this.contenido = contenido;
        }
        public void setClasificacion(Tipos clasificacion)
        {
            this.clasificacion = clasificacion;
        }
        public string getContenido()
        {
            return this.contenido;
        }
        public Tipos getClasificacion()
        {
            return this.clasificacion;
        }
    }
}