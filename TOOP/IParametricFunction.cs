using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions
{
    public interface IParametricFunction
    {
        IFunction Bind(IVector parameters);
    }

    // Функция имеет вид y = a_0 + a_1 * x_1 + a_2 * x_2 + ... + a_n * x_n
    class LinearFunction : IParametricFunction
    {
        private IVector _parameters;

        public IFunction Bind(IVector parameters)
        {
            if (parameters.Count < 1)
                throw new ArgumentException("Список параметров пуст.");
            _parameters = parameters;
            return new InternalLinearFunction(_parameters);
        }

        class InternalLinearFunction : IDifferentiableFunction
        {
            private readonly IVector _parameters;

            public InternalLinearFunction(IVector parameters)
            {
                _parameters = parameters;
            }

            public double Value(IVector point)
            {
                if (_parameters.Count < 1)
                    throw new ArgumentException("Параметры не заданы.");

                if (point.Count != _parameters.Count - 1)
                    throw new ArgumentException("Размерность точки должна быть на единицу меньше числа параметров.");

                double res = _parameters[0];
                for (int i = 0; i < point.Count; i++)
                {
                    res += _parameters[i + 1] * point[i];
                }
                return res;
            }

            public IVector Gradient(IVector point)
            {
                if (_parameters.Count < 1)
                    throw new ArgumentException("Параметры не заданы.");

                if (point.Count != _parameters.Count - 1)
                    throw new ArgumentException("Размерность точки должна быть на единицу меньше числа параметров.");

                var gradient = new Vector();

                for (int i = 0; i < gradient.Count; i++)
                {
                    // Производная по xi равна соответствующему параметру ai, поскольку функция линейная
                    gradient.Add(_parameters[i + 1]);
                }
                return gradient;
            }
        }
    }

    // Функция имеет вид y = a_0 + a_1 * x + a_2 * x^2 + ... + a_n * x^n
    class PolynomFunction : IParametricFunction
    {
        private IVector _parameters;

        public IFunction Bind(IVector parameters)
        {
            if (parameters.Count < 1)
                throw new ArgumentException("Список параметров пуст.");
            _parameters = parameters;
            return new InternalPolynomFunction(_parameters);
        }

        class InternalPolynomFunction : IFunction
        {
            private readonly IVector _parameters;

            public InternalPolynomFunction(IVector parameters)
            {
                _parameters = parameters;
            }

            public double Value(IVector point)
            {
                if (_parameters.Count < 1)
                    throw new ArgumentException("Параметры не заданы.");

                if (point.Count != 1)
                    throw new ArgumentException("Размерность точки должна равна 1.");

                double res = _parameters[0];

                for(int i = 1; i < _parameters.Count; i++)
                    res += _parameters[i] * Math.Pow(point[0], i);

                return res;
            }
        }
    }

    public class PolyLinearFunction : IParametricFunction
    {
        private IVector _parameters;
        private IVector _mesh;

        public IFunction Bind(IVector parameters)
        {
            if(parameters.Count < 1)
                throw new ArgumentException("Список параметров пуст.");
            _parameters = parameters;
            return new InternalPolyLinearFunction(_mesh, _parameters);
        }

        public PolyLinearFunction(IVector mesh)
        {
            if (mesh.Count < 2)
                throw new ArgumentException("Задано слишком мало точек.");
            _mesh = mesh;
        }

        class InternalPolyLinearFunction : IDifferentiableFunction
        {
            private readonly IVector _parameters;
            private readonly IVector _mesh;

            public InternalPolyLinearFunction(IVector mesh, IVector parameters)
            {
                _mesh = mesh;
                _parameters = parameters;
            }

            (double xk, double xk1, double yk, double yk1) FindInterval(double input, out int pos)
            {
                pos = Array.BinarySearch(_mesh.ToArray(), input);
                if (pos < 0)
                    pos = ~pos;
                try
                {
                    return (_mesh[pos - 1], _mesh[pos], _parameters[pos - 1], _parameters[pos]);
                }
                catch
                {
                    throw new ArgumentException("Точка находится вне заданной области.");
                }
            }

            public double Value(IVector point)
            {
                if (_parameters.Count < 1)
                    throw new ArgumentException("Параметры не заданы.");

                if (point.Count != 1)
                    throw new ArgumentException("Размерность точки должна быть равна 1.");

                try
                {
                    int pos;
                    (double x1, double x2, double y1, double y2) = FindInterval(point[0], out pos);
                    return y1 + (y2 - y1) / (x2 - x1) * (point[0] - x1);
                }
                catch
                {
                    if (point[0] < _mesh[0])
                        return _parameters[0];
                    else
                        return _parameters[^1];
                }

            }

            public IVector Gradient(IVector point)
            {
                if (_parameters.Count < 1)
                    throw new ArgumentException("Параметры не заданы.");

                if (point.Count != 1)
                    throw new ArgumentException("Размерность точки должна быть равна 1.");

                Vector res = new Vector();
                res.AddRange(new double[_parameters.Count]);

                try 
                {
                    int pos;
                    (double x1, double x2, double y1, double y2) = FindInterval(point[0], out pos);


                    res[pos - 1] = (x2 - point[0]) / (x2 - x1);
                    res[pos] = (point[0] - x1) / (x2 - x1);
                    return res;
                }
                catch
                {
                    if (point[0] < _mesh[0])
                    {
                        res[0] = 1;
                        return res;
                    }
                    else
                    {
                        res[^1] = 1;
                        return res;
                    }
                }
            }
        }
    }
}
