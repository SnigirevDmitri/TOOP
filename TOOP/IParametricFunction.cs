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
}
