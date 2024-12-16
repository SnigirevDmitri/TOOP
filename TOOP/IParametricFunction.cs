using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions;

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
            if (parameters.Count != mesh.Count)
                throw new ArgumentException("Число параметров неверно.");
            _mesh = mesh;
            _parameters = parameters;
        }

        private (int pos, double xk, double xk1, double yk, double yk1) FindInterval(double input)
        {
            int p = Array.BinarySearch(_mesh.ToArray(), input);
            if (p < 0)
                p = ~p;
            try
            {
                return (p, _mesh[p - 1], _mesh[p], _parameters[p - 1], _parameters[p]);
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
                (int p, double x1, double x2, double y1, double y2) = FindInterval(point[0]);
                return y1 + (y2 - y1) / (x2 - x1) * (point[0] - x1);
            }
            catch
            {
                if (point[0] < _mesh[0])
                    return _parameters[0]; // Возвращаем значение в первой точке
                else // point[0] > mesh[^1]
                    return _parameters[^2]; // Возвращаем значение в последней точке
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
                (int p, double x1, double x2, double y1, double y2) = FindInterval(point[0]);


                res[p - 1] = (x2 - point[0]) / (x2 - x1);
                res[p] = (point[0] - x1) / (x2 - x1);
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

public class CubicInterpolationHermiteSplineFunction : IParametricFunction
{
    private IVector _parameters;
    private IVector _mesh;

    public IFunction Bind(IVector parameters)
    {
        if (parameters.Count < 1)
            throw new ArgumentException("Список параметров пуст.");
        _parameters = parameters;
        return new InternalCubicInterpolationHermiteSplineFunction(_mesh, _parameters);
    }

    public CubicInterpolationHermiteSplineFunction(IVector mesh)
    {
        if (mesh.Count < 2)
            throw new ArgumentException("Задано слишком мало точек.");
        _mesh = mesh;
    }
    

    class InternalCubicInterpolationHermiteSplineFunction : IFunction
    {
        private readonly IVector _parameters;
        private readonly IVector _mesh;

        public InternalCubicInterpolationHermiteSplineFunction(IVector mesh, IVector parameters)
        {
            if (parameters.Count != mesh.Count * 2)
                throw new ArgumentException("Число параметров неверно.");
            _mesh = mesh;
            _parameters = parameters;
        }

        private (int p, double xk, double xk1, double yk, double yk1, double mk, double mk1) FindInterval(double input)
        {
            int p = Array.BinarySearch(_mesh.ToArray(), input);
            if (p < 0)
                p = ~p;
            try
            {
                return (p, _mesh[p - 1], _mesh[p], _parameters[2 * p - 2], _parameters[2 * p], _parameters[2 * p - 1], _parameters[2 * p + 1]);
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
                (int p, double x1, double x2, double y1, double y2, double m1, double m2) = FindInterval(point[0]);

                double res = 0;
                double h = x2 - x1;
                double ksi = (point[0] - x1) / h;

                res += (1 - 3 * ksi * ksi + 2 * ksi * ksi * ksi) * y1;
                res += h * (-ksi - 2 * ksi * ksi + ksi * ksi * ksi) * m1;
                res += (3 * ksi * ksi - 2 * ksi * ksi * ksi) * y2;
                res += h * (-ksi * ksi + ksi * ksi * ksi) * m2;

                return res;
            }
            catch
            {
                if (point[0] < _mesh[0])
                    return _parameters[0]; // Возвращаем значение в первой точке
                else // point[0] > mesh[^1]
                    return _parameters[^2]; // Возвращаем значение в последней точке
            }

        }
    }
}
