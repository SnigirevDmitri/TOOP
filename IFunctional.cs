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
    public interface IFunctional
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
            IDifferentiableFunction function2 = (IDifferentiableFunction)function;

            int grad = function2.Gradient(_points[0]).Count;
            var gradient = new Vector();
            gradient.AddRange(new double[grad]);

            for (int i = 0; i < _points.Count; i++)
            {
                double value = function.Value(_points[i]);
                double diff = value - _values[i];
                var funcGradient = function2.Gradient(_points[i]);

                for (int j = 0; j < grad; j++)
                    gradient[j] += Math.Sign(diff) * funcGradient[j];
            }

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
            IDifferentiableFunction function2 = (IDifferentiableFunction)function;

            int grad = function2.Gradient(_points[0]).Count;
            var gradient = new Vector();
            gradient.AddRange(new double[grad]);

            for (int i = 0; i < _points.Count; i++)
            {
                double value = function.Value(_points[i]);
                double diff = value - _values[i];
                var funcGradient = function2.Gradient(_points[i]);

                for (int j = 0; j < grad; j++)
                    gradient[j] += 2.0 * diff * funcGradient[j];
            }

            return gradient;
        }

        public IMatrix Jacobian(IFunction function)
        {
            var jacobian = new Matrix();
            IDifferentiableFunction function2 = (IDifferentiableFunction) function;
            jacobian.AddRange(_points.Select(function2.Gradient));

            return jacobian;
        }

        public IVector Residual(IFunction function)
        {
            var residual = new Vector();
           
            for (int i = 0; i < _points.Count; i++)
                residual.Add(Math.Abs(function.Value(_points[i]) - _values[i]));

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
            var max = new Vector();

            for(int i = 0; i < _points.Count;  i++)
                max.Add(Math.Abs(function.Value(_points[i]) - _values[i]));

            return max.Max();
        }
    }

    //Интеграл по некоторой области (численно)
    class Integral : IFunctional
    {
        private readonly IVector _a;
        private readonly IVector _b;
        private readonly double[] _gaussNodes = [0.0, Math.Sqrt(3.0 / 5.0), -Math.Sqrt(3.0 / 5.0)];
        private readonly double[] _gaussWeights = [8.0 / 9.0, 5.0 / 9.0, 5.0 / 9.0];

        public Integral(IVector a, IVector b)
        {
            if (a.Count != b.Count)
                throw new ArgumentException("Different dimensions");

            _a = a;
            _b = b;
        }

        public double Value(IFunction function)
        {
            double res = 0.0;
            double volume = 1.0;
            int dim = _b.Count;

            var stack = new Stack<(int depth, double currentProduct, double[] points)>();
            var points = new double[dim];
            stack.Push((0, 1.0, points));

            while (stack.Count > 0)
            {
                var (depth, currentProduct, currentPoints) = stack.Pop();

                if (depth == dim)
                {
                    var p = new Vector();
                    p.AddRange(currentPoints);
                    res+= currentProduct * function.Value(p);
                    continue;
                }

                for (int i = 0; i < 3; i++)
                {
                    double node = _gaussNodes[i];
                    double weight = _gaussWeights[i];
                    double point = 0.5 * ((_a[depth] - _b[depth]) * node + _a[depth] + _b[depth]);

                    currentPoints[depth] = point;
                    double newProduct = currentProduct * weight;
                    stack.Push((depth + 1, newProduct, (double[])currentPoints.Clone()));
                }
            }

            for (int i = 0; i < dim; i++)
                volume *= Math.Abs(_a[i] - _b[i]);

            return res * volume / Math.Pow(2, dim);
        }
    }
}


