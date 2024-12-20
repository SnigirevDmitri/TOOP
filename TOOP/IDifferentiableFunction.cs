﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions;

public interface IDifferentiableFunction : IFunction
{
    // По параметрам исходной IParametricFunction
    IVector Gradient(IVector point);
}
