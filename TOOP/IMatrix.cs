using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


interface IMatrix : IList<IList<double>> { }

public class Matrix : List<IList<double>>, IMatrix
{
    public Matrix(int n, int m) : base(n)
    {
        N = n;
        M = m;
        for (int i = 0; i < n; i++)
        {
            this[i] = new List<double>(Enumerable.Repeat(0.0, n));
        }
    }

    public int N { get; init; }
    public int M { get; init; }
    public static Vector operator *(Matrix matrix, Vector vector)
    {
        var product = new Vector(matrix.N);
        for (int i = 0; i < matrix.N; i++)
            for (int j = 0; j < matrix.M; j++)
                product[i] += matrix[i][j] * vector[j];
        return product;
    }

    public static Matrix operator *(Matrix matrix1, Matrix matrix2)
    {
        if (matrix1.M != matrix2.N)
            throw new Exception("матрицы нельзя перемножить");
        var res = new Matrix(matrix1.N, matrix2.M);
        Parallel.For(0, matrix1.N, i =>
        {
            for (int j = 0; j < matrix2.M; j++)
            {
                for (int k = 0; k < matrix1.M; k++)
                {
                    res[i][j] += matrix1[i][k] * matrix2[k][j];
                }
            }
        });
        return res;
    }
}

