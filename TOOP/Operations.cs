using Functionals;
using Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatVecOperations;
public interface IOperations
{
    public IVector CopyVector(IVector vector);
    public IVector AddVectors(IVector vec1, IVector vec2);
    public IVector ReduceVectors(IVector vec1, IVector vec2);
    public IVector MultiplyVectorByScalar(IVector vector, double scalar);
    public double VecVecMult(IVector vec1, IVector vec2);
    public double Norm(IVector vector);
    public IVector MatVecMult(IMatrix mat, IVector vec);
    public IVector SolveSLAE(IMatrix m, IVector rhs);
    public IMatrix Transpose(IMatrix mat);
    public IMatrix MatMatMult(IMatrix mat1, IMatrix mat2);

}

public class Operations : IOperations
{
    public int SolverMaxIter { get; set; } = 10000;
    public double SolverEps { get; set; } = 1e-15;
    public IVector CopyVector(IVector vector)
    {
        IVector result = new Vector();
        for (int i = 0; i < vector.Count; i++)
        {
            result.Add(0);
            result[i] = vector[i];
        }
        return result;
    }

    public IVector AddVectors(IVector vec1, IVector vec2)
    {
        if (vec1.Count != vec2.Count)
        {
            throw new ArgumentException("Vectors must have the same dimension for addition.");
        }
        IVector result = new Vector();
        for (int i = 0; i < vec1.Count; i++)
        {
            result.Add(0);
            result[i] = vec1[i] + vec2[i];
        }
        return result;
    }

    public IVector ReduceVectors(IVector vec1, IVector vec2)
    {
        if (vec1.Count != vec2.Count)
        {
            throw new ArgumentException("Vectors must have the same dimension for addition.");
        }
        IVector result = new Vector();
        for (int i = 0; i < vec1.Count; i++)
        {
            result.Add(0);
            result[i] = vec1[i] - vec2[i];
        }
        return result;
    }

    public IVector MultiplyVectorByScalar(IVector vector, double scalar)
    {
        IVector result = new Vector();
        for (int i = 0; i < vector.Count; i++)
        {
            result.Add(0);
            result[i] = vector[i] * scalar;
        }
        return result;
    }

    public double VecVecMult(IVector vec1, IVector vec2)
    {
        if (vec1.Count != vec2.Count)
        {
            throw new ArgumentException("Vectors must have the same dimension for dot product.");
        }
        double result = 0;
        for (int i = 0; i < vec1.Count; i++)
        {
            result += vec1[i] * vec2[i];
        }
        return result;
    }

    public double Norm(IVector vector)
    {
        return Math.Sqrt(VecVecMult(vector, vector));
    }

    public IVector MatVecMult(IMatrix mat, IVector vec)
    {
        int n = mat.Count;
        int m = vec.Count;
        IVector res = new Vector();
        for (int i = 0; i < n; i++)
        {
            res.Add(0);
            for (int j = 0; j < m; j++)
            {
                res[i] += mat[i][j] * vec[j];
            }
        }
        return res;
    }

    public IVector SolveSLAE(IMatrix m, IVector rhs)
    {
        int n = rhs.Count;

        double alpha, beta;
        double squareNorm;
        Vector q = [.. Enumerable.Repeat(0.0, n)];
        Vector z = new();
        IVector r = MatVecMult(m, q);
        for (int i = 0; i < n; i++)
        {
            r[i] = rhs[i] - r[i];
            z.Add(r[i]);
        }

        IVector p = MatVecMult(m, z);
        IVector tmp;

        squareNorm = VecVecMult(r, r);

        for (int index = 0; index < SolverMaxIter && squareNorm > SolverEps; index++)
        {
            alpha = VecVecMult(p, r) / VecVecMult(p, p);
            for (int i = 0; i < n; i++)
            {
                q[i] += alpha * z[i];
            }
            squareNorm = VecVecMult(r, r) - (alpha * alpha) * VecVecMult(p, p);
            for (int i = 0; i < n; i++)
            {
                r[i] -= alpha * p[i];
            }

            tmp = MatVecMult(m, r);

            beta = -VecVecMult(p, tmp) / VecVecMult(p, p);
            for (int i = 0; i < n; i++)
            {
                z[i] = r[i] + beta * z[i];
                p[i] = tmp[i] + beta * p[i];
            }
        }
        return q;
    }

    public IMatrix Transpose(IMatrix mat)
    {
        int n = mat.Count;
        int m = mat[0].Count;
        IMatrix res = new Matrix();
        for (int i = 0; i < m; i++)
        {
            res.Add(new List<double>());
            for (int j = 0; j < n; j++)
            {
                res[i].Add(mat[j][i]);
            }
        }
        return res;
    }

    public IMatrix MatMatMult(IMatrix mat1, IMatrix mat2)
    {
        int n = mat1.Count;
        int m = mat1[0].Count;
        int l = mat2[0].Count;
        IMatrix res = new Matrix();
        for (int i = 0; i < n; i++)
        {
            res.Add(new List<double>());
            for (int j = 0; j < l; j++)
            {
                res[i].Add(0);
                for (int k = 0; k < m; k++)
                {
                    res[i][j] += mat1[i][k] * mat2[k][j];
                }
            }
        }
        return res;
    }
}