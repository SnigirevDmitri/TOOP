﻿using Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functionals
{
    public interface IFunctional
    {
        double Value(IFunction function);
    }
}
