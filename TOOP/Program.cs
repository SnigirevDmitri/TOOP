using System;
using Functions;

class Program
{
    static void Main()
    {
        // Создаем вектор параметров [n, a0, a1] для функции f(x) = 3 + 5 * x
        IVector parameters = new Vector(new double[] { 3, 3, 5 });

        // Инициализация линейной функции
        IParametricFunction polynomFunc = new PolynomFunction();

        // Привязка параметров (получаем конкретную функцию)
        var boundFunc = polynomFunc.Bind(parameters);

        // Точки для вычисления значения и градиента
        IVector point1 = new Vector(new double[] { 4, 2 });
        IVector point2 = new Vector(new double[] { -3 });
        IVector point3 = new Vector(new double[] { 10 });

        // Вычисление значений функции в этих точках
        Console.WriteLine($"Значение функции в точке 4: {boundFunc.Value(point1)}");
        Console.WriteLine($"Значение функции в точке -3: {boundFunc.Value(point2)}");
        Console.WriteLine($"Значение функции в точке 10: {boundFunc.Value(point3)}");
    }
}

