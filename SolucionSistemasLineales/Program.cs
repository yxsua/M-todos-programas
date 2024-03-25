using System;

public class Program
{
    const int MAX_SIZE = 10;

    static void SwapRows(float[,] matrix, int row1, int row2, int n)
    {
        for (int j = 0; j < n; j++)
        {
            float temp = matrix[row1, j];
            matrix[row1, j] = matrix[row2, j];
            matrix[row2, j] = temp;
        }
    }

    static void PrintMatrix(float[,] matrix, int m, int n)
    {
        Console.WriteLine("Matriz:");
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write($"{matrix[i, j],10:F2} ");
            }
            Console.WriteLine();
        }
    }

    static void SolveMatrix(float[,] matrix, int m, int n)
    {
        for (int k = 0; k < m; k++)
        {
            int maxRow = k;
            float maxVal = Math.Abs(matrix[k, k]);

            for (int i = k + 1; i < m; i++)
            {
                if (Math.Abs(matrix[i, k]) > maxVal)
                {
                    maxVal = Math.Abs(matrix[i, k]);
                    maxRow = i;
                }
            }

            if (maxRow != k)
            {
                SwapRows(matrix, k, maxRow, n);
            }

            for (int i = k + 1; i < m; i++)
            {
                float factor = matrix[i, k] / matrix[k, k];
                for (int j = k; j < n; j++)
                {
                    matrix[i, j] -= factor * matrix[k, j];
                }
            }
        }

        Console.WriteLine("Matriz triangular superior:");
        PrintMatrix(matrix, m, n);

        Console.WriteLine("Resultados:");
        for (int i = m - 1; i >= 0; i--)
        {
            float sum = 0;
            for (int j = i + 1; j < n - 1; j++)
            {
                sum += matrix[i, j] * matrix[j, n - 1];
            }
            matrix[i, n - 1] = (matrix[i, n - 1] - sum) / matrix[i, i];
            Console.WriteLine($"x{i + 1} = {matrix[i, n - 1]}");
        }
    }

    public static void Main()
    {
        int m, n;
        Console.WriteLine("Ingrese el tamaño de filas de la matriz:");
        m = int.Parse(Console.ReadLine());
        Console.WriteLine("Ingrese el tamaño de columnas de la matriz:");
        n = int.Parse(Console.ReadLine());

        if (m > MAX_SIZE || n > MAX_SIZE)
        {
            Console.WriteLine("Error: Tamaño de matriz demasiado grande.");
            return;
        }

        float[,] matriz = new float[MAX_SIZE, MAX_SIZE];
        Console.WriteLine("Ingrese los elementos de la matriz:");
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write($"Matriz[{i + 1}][{j + 1}]: ");
                matriz[i, j] = float.Parse(Console.ReadLine());
            }
        }

        SolveMatrix(matriz, m, n);
    }
}