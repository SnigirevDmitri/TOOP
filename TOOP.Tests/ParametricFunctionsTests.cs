using System;
using System.Collections.Generic;
using Functions;
using Xunit;

namespace OptimizationTests
{
    public class ParametricFunctionTests
    {
        [Fact]
        public void LinearFunction_Bind_ReturnsCorrectFunction()
        {
            // Arrange
            var linearFunction = new LinearFunction();
            var parameters = new Vector { 1, 2, 3 }; // y = 1 + 2x + 3z
            var point = new Vector { 2, 3 };

            // Act
            var function = linearFunction.Bind(parameters);
            var result = function.Value(point);

            // Assert
            Assert.Equal(14, result);
        }

        [Fact]
        public void LinearFunction_Bind_ThrowsArgumentException_IfNoParameters()
        {
            // Arrange
            var linearFunction = new LinearFunction();
            var parameters = new Vector();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => linearFunction.Bind(parameters));
        }

        [Fact]
        public void LinearFunction_InternalLinearFunction_ThrowsArgumentException_IfPointDimensionIncorrect()
        {
            // Arrange
            var linearFunction = new LinearFunction();
            var parameters = new Vector { 1, 2, 3 };
            var function = linearFunction.Bind(parameters);
            var point = new Vector { 2 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => function.Value(point));
        }

        [Fact]
        public void LinearFunction_Gradient_ReturnsCorrectGradient()
        {
            // Arrange
            var linearFunction = new LinearFunction();
            var parameters = new Vector { 1, 2, 3 };
            var point = new Vector { 2, 3 };
            var function = (IDifferentiableFunction)linearFunction.Bind(parameters);

            // Act
            var gradient = function.Gradient(point);

            // Assert
            Assert.Equal(1, gradient[0]);
            Assert.Equal(2, gradient[1]);
            Assert.Equal(3, gradient[2]);
        }

        [Fact]
        public void LinearFunction_Gradient_ThrowsArgumentException_IfPointDimensionIncorrect()
        {
            // Arrange
            var linearFunction = new LinearFunction();
            var parameters = new Vector { 1, 2, 3 };
            var function = (IDifferentiableFunction)linearFunction.Bind(parameters);
            var point = new Vector { 2 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => function.Gradient(point));
        }

        [Fact]
        public void PolynomFunction_Bind_ReturnsCorrectFunction()
        {
            // Arrange
            var polynomFunction = new PolynomFunction();
            var parameters = new Vector { 1, 2, 3 }; // y = 1 + 2x + 3x^2
            var point = new Vector { 2 };

            // Act
            var function = polynomFunction.Bind(parameters);
            var result = function.Value(point);

            // Assert
            Assert.Equal(17, result);
        }

        [Fact]
        public void PolynomFunction_Bind_ThrowsArgumentException_IfNoParameters()
        {
            // Arrange
            var polynomFunction = new PolynomFunction();
            var parameters = new Vector();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => polynomFunction.Bind(parameters));
        }

        [Fact]
        public void PolynomFunction_InternalPolynomFunction_ThrowsArgumentException_IfPointDimensionIncorrect()
        {
            // Arrange
            var polynomFunction = new PolynomFunction();
            var parameters = new Vector { 1, 2, 3 };
            var function = polynomFunction.Bind(parameters);
            var point = new Vector { 2, 3 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => function.Value(point));
        }

        [Fact]
        public void PolyLinearFunction_Bind_ReturnsCorrectFunction()
        {
            // Arrange
            var mesh = new Vector { 0, 1, 2 };
            var polyLinearFunction = new PolyLinearFunction(mesh);
            var parameters = new Vector { 1, 2, 3 };
            var point = new Vector { 0.5 };

            // Act
            var function = polyLinearFunction.Bind(parameters);
            var result = function.Value(point);

            // Assert
            Assert.Equal(1.5, result);
        }

        [Fact]
        public void PolyLinearFunction_Bind_ThrowsArgumentException_IfNoParameters()
        {
            // Arrange
            var mesh = new Vector { 0, 1, 2 };
            var polyLinearFunction = new PolyLinearFunction(mesh);
            var parameters = new Vector();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => polyLinearFunction.Bind(parameters));
        }

        [Fact]
        public void PolyLinearFunction_InternalPolyLinearFunction_ThrowsArgumentException_IfPointDimensionIncorrect()
        {
            // Arrange
            var mesh = new Vector { 0, 1, 2 };
            var polyLinearFunction = new PolyLinearFunction(mesh);
            var parameters = new Vector { 1, 2, 3 };
            var function = polyLinearFunction.Bind(parameters);
            var point = new Vector { 0.5, 0.6 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => function.Value(point));
        }

        [Fact]
        public void PolyLinearFunction_Gradient_ReturnsCorrectGradient()
        {
            // Arrange
            var mesh = new Vector { 0, 1, 2 };
            var polyLinearFunction = new PolyLinearFunction(mesh);
            var parameters = new Vector { 1, 2, 3 };
            var point = new Vector { 0.5 };
            var function = (IDifferentiableFunction)polyLinearFunction.Bind(parameters);

            // Act
            var gradient = function.Gradient(point);

            // Assert
            Assert.Equal(0.5, gradient[0], 5);
            Assert.Equal(0.5, gradient[1], 5);
            Assert.Equal(0, gradient[2], 5);
        }

        [Fact]
        public void PolyLinearFunction_Gradient_ReturnsCorrectGradient_OutOfRange()
        {
            // Arrange
            var mesh = new Vector { 0, 1, 2 };
            var polyLinearFunction = new PolyLinearFunction(mesh);
            var parameters = new Vector { 1, 2, 3 };
            var point = new Vector { -0.5 };
            var function = (IDifferentiableFunction)polyLinearFunction.Bind(parameters);

            // Act
            var gradient = function.Gradient(point);

            // Assert
            Assert.Equal(1, gradient[0], 5);
            Assert.Equal(0, gradient[1], 5);
            Assert.Equal(0, gradient[2], 5);
        }

        [Fact]
        public void PolyLinearFunction_Gradient_ThrowsArgumentException_IfPointDimensionIncorrect()
        {
            // Arrange
            var mesh = new Vector { 0, 1, 2 };
            var polyLinearFunction = new PolyLinearFunction(mesh);
            var parameters = new Vector { 1, 2, 3 };
            var function = (IDifferentiableFunction)polyLinearFunction.Bind(parameters);
            var point = new Vector { 0.5, 0.6 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => function.Gradient(point));
        }

        [Fact]
        public void CubicInterpolationHermiteSplineFunction_Bind_ReturnsCorrectFunction()
        {
            //y(x) = 4*x^3 + 1
            // Arrange
            var mesh = new Vector { 0, 1, 2 };
            var splineFunction = new CubicInterpolationHermiteSplineFunction(mesh);
            var parameters = new Vector { 1, 0, 5, 12, 33, 48 }; // y0, m0, y1, m1, y2, m2
            var point = new Vector { 0.5 };

            // Act
            var function = splineFunction.Bind(parameters);
            var result = function.Value(point);

            // Assert
            Assert.Equal(1.5, result, 5);
        }

        [Fact]
        public void CubicInterpolationHermiteSplineFunction_Bind_ThrowsArgumentException_IfNoParameters()
        {
            // Arrange
            var mesh = new Vector { 0, 1, 2 };
            var splineFunction = new CubicInterpolationHermiteSplineFunction(mesh);
            var parameters = new Vector();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => splineFunction.Bind(parameters));
        }

        [Fact]
        public void CubicInterpolationHermiteSplineFunction_InternalCubicInterpolationHermiteSplineFunction_ThrowsArgumentException_IfPointDimensionIncorrect()
        {
            // Arrange
            var mesh = new Vector { 0, 1, 2 };
            var splineFunction = new CubicInterpolationHermiteSplineFunction(mesh);
            var parameters = new Vector { 1, 2, 3, 4, 5, 6 };
            var function = splineFunction.Bind(parameters);
            var point = new Vector { 0.5, 0.6 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => function.Value(point));
        }
    }
}