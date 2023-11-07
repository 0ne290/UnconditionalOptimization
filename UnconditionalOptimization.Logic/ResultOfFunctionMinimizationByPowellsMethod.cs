using UnconditionalOptimization.Interfaces;

namespace UnconditionalOptimization.Logic;

public class ResultOfFunctionMinimizationByPowellsMethod
{
    public IPoint1Argument MinimumPoint { get; set; }
    public int NumberOfIterations { get; set; }
    public int NumberOfObjectiveFunctionCalculations { get; set; }
    public double? ConvergenceCoefficient { get; set; }
}