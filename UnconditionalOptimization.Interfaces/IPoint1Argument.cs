namespace UnconditionalOptimization.Interfaces;

public interface IPoint1Argument
{
	Func<double, double> ObjectiveFunction { get; set; }
    double X { get; set; }
    double Y { get; }
	static int NumberOfObjectiveFunctionCalls { get; }
}