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

    // Функция имеет вид y = a0 + a1 * x1 + a2 * x2 + ... + an * xn
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

}
