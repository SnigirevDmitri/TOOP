using Functions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Functionals
{
    interface IFunctional
    {
        double Value(IFunction function);
    }

    //l1 норма разности с требуемыми значениями в наборе точек (реализует IDifferentiableFunctional, не реализует ILeastSquaresFunctional)
    class L1Norm : IDifferentiableFunctional
    {
        private readonly IList<IVector> _points;
        private readonly IVector _values;

        public L1Norm(IList<IVector> points, IVector value)
        {
            if (points.Count != value.Count)
                throw new ArgumentException("Different dimensions");

            _points = points;
            _values = value;
        }

        public double Value(IFunction function)
        {
            double sum = 0.0;

            for (int i = 0; i < _points.Count; i++)
                sum += Math.Abs(function.Value(_points[i]) - _values[i]);

            return sum;
        }

        public IVector Gradient(IFunction function)
        {
            double value = 0.0;
            foreach (IVector point in _points)
            {
                 value = function.Value(point);
            }

            var gradient = new Vector();

            for(int i = 0; i < _points.Count; i++)
            {
                gradient[i] = value - _values[i];
            }

            //throw new NotImplementedException();

            return gradient;
        }
    }

    //l2 норма разности с требуемыми значениями в наборе точек(реализует IDifferentiableFunctional, реализует ILeastSquaresFunctional)
    class L2Norm : IDifferentiableFunctional, ILeastSquaresFunctional
    {
        private readonly IList<IVector> _points;
        private readonly IVector _values;

        public L2Norm(IList<IVector> points, IVector value)
        {
            if (points.Count != value.Count)
                throw new ArgumentException("Different dimensions");

            _points = points;
            _values = value;
        }

        public double Value(IFunction function)
        {
            double sum = 0 , s = 0;

            for (int i = 0; i < _points.Count; i++)
            { 
                s = Math.Abs(function.Value(_points[i]) - _values[i]);
                sum += s * s;
            }
            return sum;
        }

        public IVector Gradient(IFunction function)
        {
            //double value = function.Value(_points);
            var gradient = new Vector();
            throw new NotImplementedException();

            return gradient;
        }

        public IMatrix Jacobian(IFunction function)
        {
            var jacobian = new Matrix(0, 0);

            for (int i = 0; i < _points.Count; i++)
                jacobian.AddRange((IEnumerable<IList<double>>)Gradient(_points[i]));

            return jacobian;
        }

        public IVector Residual(IFunction function)
        {
            var residual = new Vector();
            var s1 = 0.0;

            for (int i = 0; i < _points.Count; i++)
                s1 = Math.Abs(function.Value(_points[i]) - _values[i]);

            residual.Add(s1);

            return residual;
        }

        

    }

    //linf норма разности с требуемыми значениями в наборе точек (не реализует IDifferentiableFunctional, не реализует ILeastSquaresFunctional)
    class Linf : IFunctional
    {
        private readonly IList<IVector> _points;
        private readonly IVector _values;

        public Linf(IList<IVector> points, IVector value)
        {
            if (points.Count != value.Count)
                throw new ArgumentException("Different dimensions");

            _points = points;
            _values = value;
        }

        public double Value(IFunction function)
        {
            var max = new Vector(_points.Count);

            for(int i = 0; i < _points.Count;  i++)
                max[i] = Math.Abs(function.Value(_points[i]) - _values[i]);

            return max.Max();
        }
    }
}


