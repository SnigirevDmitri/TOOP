using System;
using System.Collections.Generic;
using Functions;
using Functionals;
using Optimizators;
using Xunit;

namespace OptimizationTests
{
    public class OptimizerTests
    {
        //[Fact]
        //public void SimulatedAnnealing_Minimize_ReturnsCorrectParameters()
        //{
        //    // Arrange
        //    var points = new List<IVector> { new Vector { 1 }, new Vector { 2 }, new Vector { 3 } };
        //    var values = new Vector { 2, 4, 6 };
        //    var linf = new Linf(points, values);
        //    var linearFunction = new LinearFunction();
        //    var initialParameters = new Vector { 0, 0 };
        //    var optimizer = new SimulatedAnnealing(100, 1000);

        //    // Act
        //    var result = optimizer.Minimize(linf, linearFunction, initialParameters);

        //    // Assert
        //    Assert.Equal(0, result[0], 1);
        //    Assert.Equal(2, result[1], 1);
        //}

        [Fact]
        public void ConjugateGradient_Minimize_ReturnsCorrectParameters()
        {
            // Arrange
            var points = new List<IVector> { new Vector { 1 }, new Vector { 2 }, new Vector { 3 } };
            var values = new Vector { 2, 4, 6 };
            var l2Norm = new L2Norm(points, values);
            var linearFunction = new LinearFunction();
            var initialParameters = new Vector { 0, 0 };
            var optimizer = new ConjugateGradient(0.001, 1000);

            // Act
            var result = optimizer.Minimize(l2Norm, linearFunction, initialParameters);

            // Assert
            Assert.Equal(0, result[0], 1);
            Assert.Equal(2, result[1], 1);
        }

        [Fact]
        public void ConjugateGradient_Minimize_ThrowsArgumentException_IfNotDifferentiable()
        {
            // Arrange
            var points = new List<IVector> { new Vector { 1 }, new Vector { 2 }, new Vector { 3 } };
            var values = new Vector { 2, 4, 6 };
            var linf = new Linf(points, values);
            var linearFunction = new LinearFunction();
            var initialParameters = new Vector { 0, 0 };
            var optimizer = new ConjugateGradient(0.001, 1000);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => optimizer.Minimize(linf, linearFunction, initialParameters));
        }

        [Fact]
        public void GaussNewton_Minimize_ReturnsCorrectParameters()
        {
            // Arrange
            var points = new List<IVector> { new Vector { 1 }, new Vector { 2 }, new Vector { 3 } };
            var values = new Vector { 3, 5, 7 };
            var l2Norm = new L2Norm(points, values);
            var linearFunction = new LinearFunction();
            var initialParameters = new Vector { 0, 0 };
            var optimizer = new GaussNewton(0.001, 1000);

            // Act
            var result = optimizer.Minimize(l2Norm, linearFunction, initialParameters);

            // Assert
            Assert.Equal(1, result[0], 1);
            Assert.Equal(2, result[1], 1);
        }

        [Fact]
        public void GaussNewton_Minimize_ThrowsArgumentException_IfNotLeastSquares()
        {
            // Arrange
            var points = new List<IVector> { new Vector { 1 }, new Vector { 2 }, new Vector { 3 } };
            var values = new Vector { 2, 4, 6 };
            var l1Norm = new L1Norm(points, values);
            var linearFunction = new LinearFunction();
            var initialParameters = new Vector { 0, 0 };
            var optimizer = new GaussNewton(0.001, 1000);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => optimizer.Minimize(l1Norm, linearFunction, initialParameters));
        }
    }
}