using Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functionals
{
    interface IFunctional
    {
        double Value(IFunction function);
    }

     class Linf: IFunctional
     {
         private readonly IList<IVector> _points1, _points2;
         private readonly List<(double x, double y)> _points;
         private readonly IVector _values;

         public Linf(IList<IVector> point1, IVector point2)
         {
             if (point1.Count != point2.Count)
                 throw new ArgumentException("Different dimensions");

             _points1 = point1;
             _values = point2;
         }

         public double Value(IFunction function)
         {
             var max = new Vector(_points1.Count);

             for(int i = 0; i < _points1.Count;  i++)
                 max[i] = Math.Abs(function.Value(_points1[i]) - _values[i]);

             return max.Max();
         }
     }
}
