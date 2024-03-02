using System;

class piAproximacion {

    public static void MetodoUno() {
        double aux1 = 0, aux2 = 0;
        double pi = 0;

        for(int i = 0; i < 500000; ++i) {
            aux2 = 1.0/(2*i + 1);
            if(i % 2 == 0) {
                aux1 += aux2;
            } else {
                aux1 -= aux2;
            }
        }

        pi = 4.0*aux1;
        Console.WriteLine("Aproximación: " + pi.Truncar(5));
    }

    public static void MetodoDos() {
        int i = 150000, j = 0;
        double n = 0;

        for(j = 2*i-1; j > 1; j-=2) {
            if(n == 0) {
                n = Math.Pow(j-2, 2)/(2+Math.Pow(j, 2));
            } else {
                n = Math.Pow(j-2, 2)/(2+n);
            }
        }

        n = 4/(1+n);
        Console.WriteLine("Aproximación: " + n.Truncar(5));
    }

    public static void Main(string[] args) {
        
        Console.WriteLine("Método 1:");
        MetodoUno();
        Console.WriteLine("\nMétodo 2:");
        MetodoDos();
    }

}