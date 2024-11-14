using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IMatrix : IList<IList<double>> { }

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
}

