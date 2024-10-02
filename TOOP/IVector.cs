using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface IVector : IList<double> { }

public class Vector : List<double>, IVector
{
    public Vector() { }
    public Vector(int N) : base(Enumerable.Repeat(0.0, N)) { }
    public Vector(IEnumerable<double> data) : base(data) { }

    public static Vector operator -(Vector vec1, Vector vec2)
    {
        if (vec1.Count != vec2.Count)
            throw new Exception("Длины векторов не равны");
        var res = new Vector(vec1.Count);
        for (int i = 0; i < vec1.Count; i++)
            res[i] = vec1[i] - vec2[i];
        return res;
    }

    public static Vector operator +(Vector vec1, Vector vec2)
    {
        if (vec1.Count != vec2.Count)
            throw new Exception("Длины векторов не равны");
        var res = new Vector(vec1.Count);
        for (int i = 0; i < vec1.Count; i++)
            res[i] = vec1[i] + vec2[i];
        return res;
    }

    public static double operator *(Vector vec1, Vector vec2)
    {
        if (vec1.Count != vec2.Count)
            throw new Exception("Длины векторов не равны");
        double product = 0;
        for (int i = 0; i < vec1.Count; i++)
            product += vec1[i] * vec2[i];
        return product;
    }

    public static Vector operator *(double number, Vector vec)
    {
        Vector product = new Vector(vec.Count);
        for (int i = 0; i < vec.Count; i++)
            product[i] += number * vec[i];
        return product;
    }
}

