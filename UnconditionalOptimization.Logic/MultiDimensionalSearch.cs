using UnconditionalOptimization.Interfaces;

namespace UnconditionalOptimization.Logic;

public class MultiDimensionalSearch
{
	public MultiDimensionalSearch(ILogger ui, Func<double[], double> objectiveFunction, int dimension)
    {
        ObjectiveFunction = objectiveFunction;
        Ui = ui;
		Dimension = dimension;
    }
    public MultiDimensionalSearch(ILogger ui, Func<double[], double> objectiveFunction)
    {
        ObjectiveFunction = objectiveFunction;
        Ui = ui;
		Dimension = 2;
    }
    public MultiDimensionalSearch(ILogger ui)
    {
        ObjectiveFunction = arguments =>
			2 * Math.Pow(arguments[0] - 4, 2) + Math.Pow(arguments[1] - 6, 2);
        Ui = ui;
		Dimension = 2;
    }

    public ResultOfFunctionMinimizationByPowellsConjugateDirectionsMethod FunctionMinimizationByPowellsConjugateDirectionsMethod(double[] startingPoint, double accuracyByArgument, double accuracyByFunction, double increment, double incrementReductionFactor)
    {
		if (startingPoint.Length != Dimension)
			throw new Exception($"The number of coordinates of the starting point must be {Dimension}.");
        if (!(incrementReductionFactor > 0 && incrementReductionFactor < 1))
            throw new Exception("The increment reduction factor must be greater than zero and less than one.");
		
        ClosedArray<double[]> conjugateDirectionVectors = new ClosedArray<double[]>(new double[Dimension + 1][]);
        ClosedArray<IPointXArgument> points = new ClosedArray<IPointXArgument>(new IPointXArgument[Dimension + 1]);
		for (int j = Dimension; j > 0; j--)
        {
            conjugateDirectionVectors[j] = new double[Dimension];
            conjugateDirectionVectors[j][j - 1] = 1;
			points[j] = new PointXArgument(ObjectiveFunction, Dimension);
		}
        conjugateDirectionVectors[0] = conjugateDirectionVectors[Dimension];
        points[0] = new PointXArgument(ObjectiveFunction, Dimension);

		Factories factories = new Factories();
		OneDimensionalSearch oneDimensionalSearch = factories.OneDimensionalSearchFactory<LoggerFake>();
        ResultOfFunctionMinimizationByPowellsConjugateDirectionsMethod result = new ResultOfFunctionMinimizationByPowellsConjugateDirectionsMethod();
        double coefficient;

        Ui.AddARowToTheTable(new[] { "i", "x", "d", "xmin", "f(xmin)", "error(x)", "error(y)" });

        points[Dimension].X = startingPoint;
        int i;
        for (i = 0; true; i++)
        {
            oneDimensionalSearch.ObjectiveFunction = GenerateFunctionForOneDimensionalSearch(conjugateDirectionVectors[i], points[i - 1]);
            coefficient = oneDimensionalSearch.FunctionMinimizationByPowellsMethod(startingPoint[0], increment, accuracyByArgument, accuracyByFunction).MinimumPoint.X;
            increment *= incrementReductionFactor;
            points[i] = VectorSum(points[i - 1], NumberMultipliedByvector(coefficient, conjugateDirectionVectors[i]));

            Ui.AddARowToTheTable(new[]
            {
                i.ToString(), points[i - 1].ToString(),
                ArrayOfDoubleToString(conjugateDirectionVectors[i]),
                points[i].ToString(), Math.Round(points[i].Y, 5).ToString(),
                Math.Round(CalculateVectorLength(VectorSub(points[i], points[i - 1])), 5).ToString(),
                Math.Round(Math.Abs(points[i].Y - points[i - 1].Y), 5).ToString()
            });

            if (AccuracyAchieved(points[i], points[i - 1], accuracyByArgument, accuracyByFunction))
                break;

            if (((i + 1) % (Dimension + 1)) == 0)
            {
				for (int j = Dimension - 1; j > 0; j--)
					conjugateDirectionVectors[j] = conjugateDirectionVectors[j - 1];
                conjugateDirectionVectors[0] = VectorSub(points[i], points[i - Dimension]);
                conjugateDirectionVectors[Dimension] = conjugateDirectionVectors[0];
            }
        }

        if (Ui is LoggerConsole)
            ((LoggerConsole)Ui).Dispose();

        result.MinimumPoint = points[i];
        result.NumberOfIterations = i;
        result.NumberOfObjectiveFunctionCalculations = PointXArgument.NumberOfObjectiveFunctionCalls;
        
        return result;
    }

    public Func<double, double> GenerateFunctionForOneDimensionalSearch(double[] factors, IPointXArgument terms) => argument =>
    {
        var arguments = new double[Dimension];
        for (int i = 0; i < Dimension; i++)
            arguments[i] = factors[i] * argument + terms[i];
        return ObjectiveFunction(arguments);
    };

    public IPointXArgument VectorSum(IPointXArgument vector1, double[] vector2)
    {
        IPointXArgument result = new PointXArgument(ObjectiveFunction, Dimension);

        for (int i = 0; i < Dimension; i++)
            result[i] = vector1[i] + vector2[i];
        
        return result;
    }

    public double[] NumberMultipliedByvector(double number, double[] vector)
    {
        var result = new double[Dimension];

        for (int i = 0; i < Dimension; i++)
            result[i] = number * vector[i];

        return result;
    }

    private string ArrayOfDoubleToString(double[] array)
    {
        string result = "";
        for (int i = 0; i < Dimension; i++)
            result += $"{Math.Round(array[i], 5)}; ";
        return result;
    }

    public bool AccuracyAchieved(IPointXArgument point2, IPointXArgument point1, double accuracyByArgument, double accuracyByFunction) =>
        CalculateVectorLength(VectorSub(point2, point1)) < accuracyByArgument && Math.Abs(point2.Y - point1.Y) < accuracyByFunction;

    public double CalculateVectorLength(double[] vector)
    {
        double result = 0;

        for (int i = 0; i < Dimension; i++)
            result += Math.Pow(vector[i], 2);

        return Math.Sqrt(result);
    }

    public double[] VectorSub(IPointXArgument vector2, IPointXArgument vector1)
    {
        var result = new double[Dimension];

        for (int i = 0; i < Dimension; i++)
            result[i] = vector2[i] - vector1[i];
        
        return result;
    }

	public ILogger Ui { get; set; }
    public Func<double[], double> ObjectiveFunction { get; set; }
	public int Dimension { get; set; }
}