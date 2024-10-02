using Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functionals
{
    interface IDifferentiableFunctional : IFunctional
    {
        IVector Gradient(IFunction function);
    }
}
