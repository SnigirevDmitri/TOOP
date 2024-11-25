using System;
using Functions;
using Functionals;

class Program
{
    static void Main()
    {
        // Создаем вектор параметров [n, a0, a1] для функции f(x) = 3 + 5 * x
        IVector parameters = new Vector(new double[] { 3 });

        // Инициализация линейной функции
        IParametricFunction polynomFunc = new PolynomFunction();

        IParametricFunction line = new LineFunction();

        // Привязка параметров (получаем конкретную функцию)
        var boundFunc = polynomFunc.Bind(parameters);

        IList<IVector> points = [new Vector { 5.0 }, new Vector { 2.0 }, new Vector { 3.0 }];
        IVector value = new Vector { 2.0, 1.0, 4.0 };

        var function = line.Bind(new Vector { 1.0, 1.0 });
  
        var functional = new L1Norm(points,value);
        //var functional = new L2Norm(points1, points2);
        var result = functional.Value(function);

        Console.WriteLine(result);
    }
}

