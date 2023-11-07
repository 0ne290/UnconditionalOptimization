using UnconditionalOptimization.Interfaces;

namespace UnconditionalOptimization.Logic;

public class ResultOfFunctionMinimizationByPowellsConjugateDirectionsMethod
{
    public IPointXArgument MinimumPoint { get; set; }
    public int NumberOfIterations { get; set; }
    public int NumberOfObjectiveFunctionCalculations { get; set; }
    public double? ConvergenceCoefficient { get; set; }
}