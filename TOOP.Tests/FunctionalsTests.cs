using System;
using System.Collections.Generic;
using Functions;
using Functionals;
using Xunit;

namespace OptimizationTests
{
    public class FunctionalTests
    {
        [Fact]
        public void L1Norm_Value_ReturnsCorrectValue()
        {
            // Arrange
            var points = new List<IVector> { new Vector { 1 }, new Vector { 2 }, new Vector { 3 } };
            var values = new Vector { 2, 4, 6 };
            var l1Norm = new L1Norm(points, values);
            var linearFunction = new LinearFunction();
            var parameters = new Vector { 0, 2 }; // y = 2x
            var function = linearFunction.Bind(parameters);

            // Act
            var result = l1Norm.Value(function);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void L1Norm_Value_ThrowsArgumentException_IfPointAndValueCountNotMatch()
        {
            // Arrange
            var points = new List<IVector> { new Vector { 1 }, new Vector { 2 } };
            var values = new Vector { 2, 4, 6 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new L1Norm(points, values));
        }

        [Fact]
        public void L1Norm_Gradient_ReturnsCorrectGradient()
        {
            // Arrange
            var points = new List<IVector> { new Vector { 1 }, new Vector { 2 }, new Vector { 3 } };
            var values = new Vector { 2, 4, 6 };
            var l1Norm = new L1Norm(points, values);
            var linearFunction = new LinearFunction();
            var parameters = new Vector { 0, 2 };
            var function = (IDifferentiableFunction)linearFunction.Bind(parameters);

            // Act
            var gradient = l1Norm.Gradient(function);

            // Assert
            Assert.Equal(0, gradient[0], 5);
        }

        [Fact]
        public void L2Norm_Value_ReturnsCorrectValue()
        {
            // Arrange
            var points = new List<IVector> { new Vector { 1 }, new Vector { 2 }, new Vector { 3 } };
            var values = new Vector { 2, 4, 6 };
            var l2Norm = new L2Norm(points, values);
            var linearFunction = new LinearFunction();
            var parameters = new Vector { 0, 2 }; // y = 2x
            var function = linearFunction.Bind(parameters);

            // Act
            var result = l2Norm.Value(function);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void L2Norm_Value_ThrowsArgumentException_IfPointAndValueCountNotMatch()
        {
            // Arrange
            var points = new List<IVector> { new Vector { 1 }, new Vector { 2 } };
            var values = new Vector { 2, 4, 6 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new L2Norm(points, values));
        }

        [Fact]
        public void L2Norm_Gradient_ReturnsCorrectGradient()
        {
            // Arrange
            var points = new List<IVector> { new Vector { 1 }, new Vector { 2 }, new Vector { 3 } };
            var values = new Vector { 2, 4, 6 };
            var l2Norm = new L2Norm(points, values);
            var linearFunction = new LinearFunction();
            var parameters = new Vector { 0, 2 }; // y = 2x
            var function = (IDifferentiableFunction)linearFunction.Bind(parameters);

            // Act
            var gradient = l2Norm.Gradient(function);

            // Assert
            Assert.Equal(0, gradient[0], 5);
        }

        [Fact]
        public void L2Norm_Jacobian_ReturnsCorrectJacobian()
        {
            // Arrange
            var points = new List<IVector> { new Vector { 1 }, new Vector { 2 }, new Vector { 3 } };
            var values = new Vector { 2, 4, 6 };
            var l2Norm = new L2Norm(points, values);
            var linearFunction = new LinearFunction();
            var parameters = new Vector { 0, 2 };
            var function = (IDifferentiableFunction)linearFunction.Bind(parameters);

            // Act
            var jacobian = l2Norm.Jacobian(function);

            // Assert
            Assert.Equal(3, jacobian.Count);
            Assert.Equal(1, jacobian[0][0]);
            Assert.Equal(1, jacobian[0][1]);
            Assert.Equal(1, jacobian[1][0]);
            Assert.Equal(2, jacobian[1][1]);
            Assert.Equal(1, jacobian[2][0]);
            Assert.Equal(3, jacobian[2][1]);
        }

        [Fact]
        public void L2Norm_Residual_ReturnsCorrectResidual()
        {
            // Arrange
            var points = new List<IVector> { new Vector { 1 }, new Vector { 2 }, new Vector { 3 } };
            var values = new Vector { 3, 5, 7 };
            var l2Norm = new L2Norm(points, values);
            var linearFunction = new LinearFunction();
            var parameters = new Vector { 0, 2 };
            var function = linearFunction.Bind(parameters);

            // Act
            var residual = l2Norm.Residual(function);

            // Assert
            Assert.Equal(3, residual.Count);
            Assert.Equal(1, residual[0], 5);
            Assert.Equal(1, residual[1], 5);
            Assert.Equal(1, residual[2], 5);
        }

        [Fact]
        public void Linf_Value_ReturnsCorrectValue()
        {
            // Arrange
            var points = new List<IVector> { new Vector { 1 }, new Vector { 2 }, new Vector { 3 } };
            var values = new Vector { 3, 5, 7 };
            var linf = new Linf(points, values);
            var linearFunction = new LinearFunction();
            var parameters = new Vector { 0, 2 }; // y = 2x
            var function = linearFunction.Bind(parameters);

            // Act
            var result = linf.Value(function);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Linf_Value_ThrowsArgumentException_IfPointAndValueCountNotMatch()
        {
            // Arrange
            var points = new List<IVector> { new Vector { 1 }, new Vector { 2 } };
            var values = new Vector { 2, 4, 6 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Linf(points, values));
        }

        [Fact]
        public void Integral_Value_ReturnsCorrectValue()
        {
            // Arrange
            var a = new Vector { 0 };
            var b = new Vector { 1 };
            var integral = new Integral(a, b);
            var linearFunction = new LinearFunction();
            var parameters = new Vector { 0, 1 }; // y = x
            var function = linearFunction.Bind(parameters);

            // Act
            var result = integral.Value(function);

            // Assert
            Assert.Equal(0.5, result, 5);
        }

        [Fact]
        public void Integral_Value_ReturnsCorrectValue_2D()
        {
            // Arrange
            var a = new Vector { 0, 0 };
            var b = new Vector { 1, 1 };
            var integral = new Integral(a, b);
            var linearFunction = new LinearFunction();
            var parameters = new Vector { 0, 1, 1 }; // y = x + z
            var function = linearFunction.Bind(parameters);

            // Act
            var result = integral.Value(function);

            // Assert
            Assert.Equal(1, result, 5);
        }

        [Fact]
        public void Integral_Value_ThrowsArgumentException_IfDimensionNotMatch()
        {
            // Arrange
            var a = new Vector { 0 };
            var b = new Vector { 1, 2 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Integral(a, b));
        }
    }
}