using UnconditionalOptimization.Interfaces;

namespace UnconditionalOptimization.Logic;

public class PointXArgument : IPointXArgument
{
    public double this[int index]
    {
        get
        {
            return _x[index];
        }
        set
        {
			_xPrevious[index] = _x[index];
            _x[index] = value;
        }
    }
	public double[] X
	{
		get => _x;
		set
		{
			if (value.Length != Dimension)
				throw new Exception($"The number of arguments to the calculated point must be {Dimension}.");
			_x = value;
			_xPrevious = (double[])_x.Clone();
            _xPrevious[0] = _x[0] + 1;
		}
	}
    public double Y
    {
        get
        {
            if (ArgumentsHaveChanged())
            {
                _y = ObjectiveFunction(_x);
                NumberOfObjectiveFunctionCalls++;
            }
            return _y;
        }
    }
	private bool ArgumentsHaveChanged()
	{
		for (int i = 0; i < Dimension; i++)
			if (_x[i] != _xPrevious[i])
				return true;
		return false;
	}

    public PointXArgument(Func<double[], double> objectiveFunction, int dimension)
    {
        ObjectiveFunction = objectiveFunction;
		Dimension = dimension;
		_x = new double[Dimension];
		_xPrevious = new double[Dimension];
        _xPrevious[0] = _x[0] + 1;
    }

    public override string ToString()
    {
        string result = "";
        for (int i = 0; i < Dimension; i++)
            result += $"{Math.Round(_x[i], 5)}; ";
        return result;
    }

    public static int NumberOfObjectiveFunctionCalls { get; private set; } = 0;
    public Func<double[], double> ObjectiveFunction { get; set; }
	public int Dimension { get; private set; }

    private double[] _x, _xPrevious;
	private double _y;
}