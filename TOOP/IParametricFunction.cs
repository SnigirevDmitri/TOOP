using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions
{
    interface IParametricFunction
    {
        IFunction Bind(IVector parameters);
    }

    // Функция имеет вид y = a_0 + a_1 * x_1 + a_2 * x_2 + ... + a_n * x_n
    class LineFunction : IParametricFunction
    {
        private IVector _parameters;

        public IFunction Bind(IVector parameters)
        {
            _parameters = parameters;
            return new InternalLineFunction(_parameters);
        }

        class InternalLineFunction : IDifferentiableFunction
        {
            private readonly IVector _parameters;

            public InternalLineFunction(IVector parameters)
            {
                _parameters = parameters;
            }

            public double Value(IVector point)
            {
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
                if (point.Count != _parameters.Count - 1)
                    throw new ArgumentException("Размерность точки должна быть на единицу меньше числа параметров.");

                var gradient = new Vector(point.Count);

                for (int i = 0; i < gradient.Count; i++)
                {
                    // Производная по xi равна соответствующему параметру ai, поскольку функция линейная
                    gradient[i] = _parameters[i + 1];
                }
                return gradient;
            }
        }
    }

    // Функция имеет вид y = a_0 + a_1 * x + a_2 * x^2 + ... + a_n * x^n
    // Массив параметров выглядит следующим образом: [n, a_0, a_1, ..., a_n]
    class PolynomFunction : IParametricFunction
    {
        private IVector _parameters;

        public IFunction Bind(IVector parameters)
        {
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
                if (point.Count != 1)
                    throw new ArgumentException("Размерность точки должна равна 1.");

                if (_parameters.Count != _parameters[0] + 1)
                    throw new ArgumentException("Размерность параметров должна быть равна n + 1.");

                double res = _parameters[1];

                for(int i = 2; i < _parameters.Count; i++)
                    res += _parameters[i] * Math.Pow(point[0], i-1);

                return res;
            }
        }
    }
}
