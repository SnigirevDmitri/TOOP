using Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functionals;

public interface ILeastSquaresFunctional : IFunctional
{
    IVector Residual(IFunction function);
    IMatrix Jacobian(IFunction function);
}
