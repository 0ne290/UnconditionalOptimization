using UnconditionalOptimization.Interfaces;

namespace UnconditionalOptimization.Logic;

public class Point1Argument : IPoint1Argument
{
    public double X
    {
        get => _x;
        set
        {
            _x = value;
            Y = ObjectiveFunction(value);
            NumberOfObjectiveFunctionCalls++;
        }
    }
    public double Y { get; private set; }

    public Point1Argument(Func<double, double> objectiveFunction)
    {
        _x = default;
        Y = default;
        ObjectiveFunction = objectiveFunction;
    }
	
	public static int NumberOfObjectiveFunctionCalls { get; private set; } = 0;
	public Func<double, double> ObjectiveFunction { get; set; }
	
    private double _x;
}