﻿using Functionals;
using Functions;
using MatVecOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizators;
public interface IOptimizator
{
    IVector Minimize(IFunctional objective,
                     IParametricFunction function,
                     IVector initialParameters,
                     IVector minimumParameters = default,
                     IVector maximumParameters = default);
}

public class SimulatedAnnealing : IOptimizator
{
    private readonly double _initialTemperature;
    private readonly int _maxIterations;

    public SimulatedAnnealing(double initialTemperature, int maxIterations)
    {
        _initialTemperature = initialTemperature;
        _maxIterations = maxIterations;
    }

    public IVector Minimize(IFunctional objective, IParametricFunction function, IVector initialParameters, IVector minimumParameters = null, IVector maximumParameters = null)
    {
        IVector currentParameters = new Vector();
        foreach(var param in initialParameters)
        {
            currentParameters.Add(param);
        }
        var currentEnergy = objective.Value(function.Bind(currentParameters));

        Random random = new Random();
        for (int i = 1; i < _maxIterations; i++)
        {
            var temperature = GetTemperature(i);
            var newParameters = GenerateNeighbor(currentParameters, minimumParameters, maximumParameters, random);
            var newEnergy = objective.Value(function.Bind(newParameters));

            if (AcceptanceProbability(currentEnergy, newEnergy, temperature) > random.NextDouble())
            {
                currentParameters = newParameters;
                currentEnergy = newEnergy;
            }
        }

        return currentParameters;
    }

    private double GetTemperature(int i)
    {
        return _initialTemperature * 0.1 / i;
    }

    private IVector GenerateNeighbor(IVector currentParameters, IVector minimumParameters, IVector maximumParameters, Random random)
    {
        IVector newParameters = currentParameters;
        for (int i = 0; i < newParameters.Count; i++)
        {
            double delta = (random.NextDouble() - 0.5) * 0.1;
            newParameters[i] += delta;

            // Проверка границ
            if (minimumParameters != null && newParameters[i] < minimumParameters[i])
            {
                newParameters[i] = minimumParameters[i];
            }
            if (maximumParameters != null && newParameters[i] > maximumParameters[i])
            {
                newParameters[i] = maximumParameters[i];
            }
        }
        return newParameters;
    }

    private double AcceptanceProbability(double currentEnergy, double newEnergy, double temperature)
    {
        if (newEnergy < currentEnergy)
        {
            return 1.0;
        }
        return Math.Exp((currentEnergy - newEnergy) / temperature);
    }
}

public class ConjugateGradient : IOptimizator
{
    private readonly IOperations op = new Operations();
    private readonly double _eps;
    private readonly int _maxIterations;

    public ConjugateGradient(double eps, int maxIterations)
    {
        _eps = eps;
        _maxIterations = maxIterations;
    }

    public IVector Minimize(IFunctional objective, IParametricFunction function, IVector initialParameters, IVector minimumParameters = null, IVector maximumParameters = null)
    {
        if (!(objective is IDifferentiableFunctional differentiableObjective))
        {
            throw new ArgumentException("Objective functional must be differentiable for Conjugate Gradient method.");
        }

        var currentParameters = op.CopyVector(initialParameters);
        var currentFunction = function.Bind(currentParameters);
        var currentGradient = differentiableObjective.Gradient(currentFunction);
        var searchDirection = op.MultiplyVectorByScalar(currentGradient, -1); // Минус градиент
        var currentValue = objective.Value(currentFunction);

        int iteration = 0;
        while (currentValue > _eps && iteration < _maxIterations)
        {
            // Определение оптимального шага
            double alpha = LineSearch(differentiableObjective, function, currentParameters, searchDirection);

            // Обновление параметров
            var nextParameters = op.AddVectors(currentParameters, op.MultiplyVectorByScalar(searchDirection, alpha));

            // Проверка границ
            if (minimumParameters != null || maximumParameters != null)
            {
                nextParameters = ApplyConstraints(nextParameters, minimumParameters, maximumParameters);
            }

            var nextFunction = function.Bind(nextParameters);
            var nextGradient = differentiableObjective.Gradient(nextFunction);

            // Вычисление коэффициента beta 
            double beta = op.VecVecMult(nextGradient, nextGradient) / op.VecVecMult(currentGradient, currentGradient);

            // Обновление направления поиска
            searchDirection = op.AddVectors(op.MultiplyVectorByScalar(nextGradient, -1), op.MultiplyVectorByScalar(searchDirection, beta));

            // Обновление текущих значений
            currentParameters = nextParameters;
            currentGradient = nextGradient;
            currentFunction = function.Bind(currentParameters);
            currentValue = objective.Value(currentFunction);

            iteration++;
        }

        return currentParameters;
    }

    // Простой одномерный поиск для определения шага
    private double LineSearch(IDifferentiableFunctional objective, IParametricFunction function, IVector point, IVector direction)
    {
        double alpha = 1;
        var curpoint = new Vector();
        for (int i = 0; i < point.Count; i++)
        {
            curpoint.Add(point[i] + alpha * direction[i]);
        }
        var curfunction = function.Bind(curpoint);
        var lastValue = 0.0;
        var curvalue = objective.Value(curfunction);
        do
        {
            lastValue = curvalue;
            alpha /= 2;
            curpoint = new Vector();
            for (int i = 0; i < point.Count; i++)
            {
                curpoint.Add(point[i] + alpha * direction[i]);
            }
            curfunction = function.Bind(curpoint);
            curvalue = objective.Value(curfunction);
        } while (curvalue < lastValue);
        return alpha * 2;
    }

    // Применение ограничений к вектору параметров
    private IVector ApplyConstraints(IVector parameters, IVector minimumParameters, IVector maximumParameters)
    {
        IVector result = op.CopyVector(parameters);
        for (int i = 0; i < parameters.Count; i++)
        {
            if (minimumParameters != null && result[i] < minimumParameters[i])
            {
                result[i] = minimumParameters[i];
            }
            if (maximumParameters != null && result[i] > maximumParameters[i])
            {
                result[i] = maximumParameters[i];
            }
        }
        return result;
    }
}

public class GaussNewton : IOptimizator
{
    private readonly IOperations op = new Operations();
    private readonly double _eps;
    private readonly int _maxIterations;

    public GaussNewton(double eps, int maxIterations)
    {
        _eps = eps;
        _maxIterations = maxIterations;
    }

    public IVector Minimize(IFunctional objective, IParametricFunction function, IVector initialParameters, IVector minimumParameters = null, IVector maximumParameters = null)
    {
        if (!(objective is ILeastSquaresFunctional leastSquaresObjective))
        {
            throw new ArgumentException("Objective functional must be a least-squares functional for Gauss-Newton method.");
        }

        var currentParameters = op.CopyVector(initialParameters);
        var currentFunction = function.Bind(currentParameters);
        var currentValue = objective.Value(currentFunction);
        var residual = leastSquaresObjective.Residual(currentFunction);

        int iteration = 0;
        while (iteration < _maxIterations && currentValue > _eps)
        {
            var jacobian = leastSquaresObjective.Jacobian(currentFunction);
            var jacobianTranspose = op.Transpose(jacobian);

            // Решение системы J^T * J * delta = -J^T * r*
            var gradient = op.MatVecMult(jacobianTranspose, residual);
            var hessian = op.MatMatMult(jacobianTranspose, jacobian);
            var delta = op.SolveSLAE(hessian, op.MultiplyVectorByScalar(gradient, -1));

            // Обновление параметров
            var nextParameters = op.ReduceVectors(currentParameters, delta);

            // Проверка границ
            if (minimumParameters != null || maximumParameters != null)
            {
                nextParameters = ApplyConstraints(nextParameters, minimumParameters, maximumParameters);
            }

            currentParameters = nextParameters;
            currentFunction = function.Bind(currentParameters);
            currentValue = objective.Value(currentFunction);
            iteration++;
        }

        return currentParameters;
    }

    // Применение ограничений к вектору параметров
    private IVector ApplyConstraints(IVector parameters, IVector minimumParameters, IVector maximumParameters)
    {
        IVector result = op.CopyVector(parameters);
        for (int i = 0; i < parameters.Count; i++)
        {
            if (minimumParameters != null && result[i] < minimumParameters[i])
            {
                result[i] = minimumParameters[i];
            }
            if (maximumParameters != null && result[i] > maximumParameters[i])
            {
                result[i] = maximumParameters[i];
            }
        }
        return result;
    }
}