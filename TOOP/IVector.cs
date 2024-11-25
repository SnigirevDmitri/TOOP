using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IVector : IList<double> { }

public class Vector : List<double>, IVector
{
    public Vector() { }
    public Vector(int N) : base(Enumerable.Repeat(0.0, N)) { }
    public Vector(IEnumerable<double> data) : base(data) { }

}

