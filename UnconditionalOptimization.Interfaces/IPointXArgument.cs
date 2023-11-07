namespace UnconditionalOptimization.Interfaces;

public interface IPointXArgument
{
	Func<double[], double> ObjectiveFunction { get; set; }
	double this[int index] { get; set; }
	double[] X { get; set; }
    double Y { get; }
	static int NumberOfObjectiveFunctionCalls { get; }
}