using System;

class raizAproximacion {

    public static void Main(string[] args) {

        double peso, error, x1 = 10, xu = 20, xr, fx1, fxr;
        Console.WriteLine("Ingresa tu peso: ");
        peso = double.Parse(Console.ReadLine());

        do {
            xr = (x1 + xu)/2;
            fxr = xr.aplicarFormula(peso); // f(xr)
            fx1 = x1.aplicarFormula(peso); // f(x1)
            error = Formula.calcularErrorRelativo(xr, x1); // valor actual, valor anterior
            if(fxr*fx1 > 0) {
                x1 = xr;
            } else if(fxr*fx1 < 0) {
                xu = xr;
            } else {
                break;
            }
        } while(error > 1);

        Console.WriteLine("El coeficiente de fricción es aproximadamente: " + xr.Truncar(5)); 
    }
}

static class Formula {

    private const double euler = 2.71828;

    public static double aplicarFormula(this double c, double m) {
        // f(c) = am/c*(1- e^(-ct/m)) - 40
        return ((9.81*m) / c)*(1 - Math.Pow(euler, (-c*10)/m)) - 40;
    }

    public static double calcularErrorRelativo(double c1, double c2) {
        // Err = (valor actual - valor anterior)/valor actual*100
        return (c1 - c2)/c1*100;
    }
}

static class Truncate {
    
    public static double Truncar(this double valor, int decimales) {
        double auxiliar = Math.Pow(10, decimales);
        return (Math.Truncate(valor * auxiliar)/auxiliar);
    }
}
