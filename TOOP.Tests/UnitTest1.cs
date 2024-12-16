using System;
using System.Collections.Generic;
using System.Numerics;
using Functions;
using Xunit;

namespace OptimizationTests
{
    public class ParametricFunctionTests
    {
        // Вспомогательные классы для тестов
        private class TestFunction : IFunction
        {
            public Func<IVector, double> ValueFunc { get; set; }

            public double Value(IVector point)
            {
                return ValueFunc(point);
            }
        }

        private class TestParametricFunction : IParametricFunction
        {
            public Func<IVector, IFunction> BindFunc { get; set; }

            public IFunction Bind(IVector parameters)
            {
                return BindFunc(parameters);
            }
        }


        [Fact]
        public void Bind_ReturnsFunction()
        {
            // Arrange
            var parametricFunction = new TestParametricFunction
            {
                BindFunc = (parameters) => new TestFunction { ValueFunc = (point) => parameters[0] * point[0] + parameters[1] }
            };
            var parameters = new Vector { 2, 3 };

            // Act
            var function = parametricFunction.Bind(parameters);

            // Assert
            Assert.NotNull(function);
            Assert.IsAssignableFrom<IFunction>(function);
        }

        [Fact]
        public void Bind_ReturnsCorrectFunctionValue()
        {
            // Arrange
            var parametricFunction = new TestParametricFunction
            {
                BindFunc = (parameters) => new TestFunction { ValueFunc = (point) => parameters[0] * point[0] + parameters[1] }
            };
            var parameters = new Vector { 2, 3 };
            var point = new Vector { 5 };

            // Act
            var function = parametricFunction.Bind(parameters);
            var result = function.Value(point);

            // Assert
            Assert.Equal(13, result); // 2 * 5 + 3 = 13
        }


        [Fact]
        public void Bind_WithDifferentParameters_ReturnsDifferentFunctions()
        {
            // Arrange
            var parametricFunction = new TestParametricFunction
            {
                BindFunc = (parameters) => new TestFunction { ValueFunc = (point) => parameters[0] * point[0] + parameters[1] }
            };
            var parameters1 = new Vector { 2, 3 };
            var parameters2 = new Vector { 1, 4 };
            var point = new Vector { 5 };

            // Act
            var function1 = parametricFunction.Bind(parameters1);
            var function1Result = function1.Value(point);
            var function2 = parametricFunction.Bind(parameters2);
            var function2Result = function2.Value(point);

            // Assert
            Assert.NotEqual(function1Result, function2Result);
            Assert.Equal(13, function1Result); // 2 * 5 + 3 = 13
            Assert.Equal(9, function2Result); // 1 * 5 + 4 = 9
        }
    }
}
