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
}
