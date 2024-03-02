using System;

static class Truncate {
    
    public static double Truncar(this double valor, int decimales) {
        double auxiliar = Math.Pow(10, decimales);
        return (Math.Truncate(valor * auxiliar)/auxiliar);
    }
}