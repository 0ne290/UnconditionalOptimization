using UnconditionalOptimization.Interfaces;

namespace UnconditionalOptimization.Logic;

public class OneDimensionalSearch
{
    public OneDimensionalSearch(ILogger ui, Func<double, double> objectiveFunction)
    {
        ObjectiveFunction = objectiveFunction;
        Ui = ui;
    }
    public OneDimensionalSearch(ILogger ui)
    {
        ObjectiveFunction = argument =>
            Math.Pow(argument, 4) + 2 * Math.Pow(argument, 2) + 4 * argument + 1;
        Ui = ui;
    }

    public ResultOfFunctionMinimizationByPowellsMethod FunctionMinimizationByPowellsMethod(double startingPoint, double increment, double accuracyByArgument, double accuracyByFunction)
    {
        IPoint1Argument[] points = { new Point1Argument(ObjectiveFunction), new Point1Argument(ObjectiveFunction), new Point1Argument(ObjectiveFunction) };
        ClosedArray<double?> resultArguments = new ClosedArray<double?>(new double?[] { null, null, null });
        int resultArgumentsIndex = 0;
        IPoint1Argument minimum, quadraticApproximation;

        ResultOfFunctionMinimizationByPowellsMethod result = new ResultOfFunctionMinimizationByPowellsMethod();
        result.NumberOfIterations = 0;

        Ui.AddARowToTheTable(new[] { "k", "x1", "x2", "x3", "xmin", "x-", "x*", "f(x*)", "error(x)", "error(y)" });

        points[0].X = startingPoint;
        points[1].X = points[0].X + increment;
        if (points[0].Y > points[1].Y)
            points[2].X = points[0].X + 2 * increment;
        else
            points[2].X = points[0].X - increment;
        SortPoints(points);
        do
        {
            minimum = FindMinimumPoint(points);

            quadraticApproximation = new Point1Argument(ObjectiveFunction);
            quadraticApproximation.X = CalculateQuadraticApproximation(points);
            ValidateQuadraticApproximation(quadraticApproximation, points[0].X, points[2].X, increment);

            ArrangePointsRelativeToTheBest(points, minimum, quadraticApproximation);

            result.NumberOfIterations++;
            result.MinimumPoint = ChooseTheBestPoint(minimum, quadraticApproximation);

            resultArguments[resultArgumentsIndex] = result.MinimumPoint.X;
            resultArgumentsIndex++;

            Ui.AddARowToTheTable(new[]
            {
                    result.NumberOfIterations.ToString(), Math.Round(points[0].X, 5).ToString(),
                    Math.Round(points[1].X, 5).ToString(), Math.Round(points[2].X, 5).ToString(),
                    Math.Round(minimum.X, 5).ToString(), Math.Round(quadraticApproximation.X, 5).ToString(),
                    Math.Round(result.MinimumPoint.X, 5).ToString(), Math.Round(result.MinimumPoint.Y, 5).ToString(),
                    Math.Round(Math.Abs(minimum.X - quadraticApproximation.X), 5).ToString(), 
                    Math.Round(Math.Abs(minimum.Y - quadraticApproximation.Y), 5).ToString()
                });
        } while (Math.Abs(minimum.X - quadraticApproximation.X) > accuracyByArgument || Math.Abs(minimum.Y - quadraticApproximation.Y) > accuracyByFunction);

        if (Ui is LoggerConsole)
            ((LoggerConsole)Ui).Dispose();

        result.NumberOfObjectiveFunctionCalculations = Point1Argument.NumberOfObjectiveFunctionCalls;
        if (resultArguments[2] == null)
            result.ConvergenceCoefficient = null;
        else
        {
            resultArgumentsIndex--;
            result.ConvergenceCoefficient =
                Math.Abs(resultArguments[resultArgumentsIndex - 1].Value -
                         resultArguments[resultArgumentsIndex].Value) / Math.Abs(
                    resultArguments[resultArgumentsIndex - 2].Value -
                    resultArguments[resultArgumentsIndex - 1].Value);
        }

        return result;
    }

    public IPoint1Argument FindMinimumPoint(IPoint1Argument[] points)
    {
        double minimumY = points[0].Y;
        IPoint1Argument result = points[0];

        foreach (IPoint1Argument point in points)
        {
            if (point.Y < minimumY)
            {
                minimumY = point.Y;
                result = point;
            }
        }

        return result;
    }

    public double CalculateQuadraticApproximation(IPoint1Argument[] points)
    {
        double[] coefficients = CalculateCoefficientsForQuadraticApproximation(points);

        double result = (points[1].X + points[0].X) / 2 - coefficients[0] / (2 * coefficients[1]);

        return result;
    }

    public double[] CalculateCoefficientsForQuadraticApproximation(IPoint1Argument[] points)
    {
        double[] coefficients = new double[2];

        coefficients[0] = (points[1].Y - points[0].Y) / (points[1].X - points[0].X);
        coefficients[1] = 1 / (points[2].X - points[1].X) * ((points[2].Y - points[0].Y) / (points[2].X - points[0].X) - (points[1].Y - points[0].Y) / (points[1].X - points[0].X));

        return coefficients;
    }

    public void ValidateQuadraticApproximation(IPoint1Argument quadraticApproximation, double leftLimit, double rightLimit, double increment)
    {
        if (quadraticApproximation.X < leftLimit - increment)
            quadraticApproximation.X = leftLimit - increment;
        else if (quadraticApproximation.X > rightLimit + increment)
            quadraticApproximation.X = rightLimit + increment;
    }

    public void ArrangePointsRelativeToTheBest(IPoint1Argument[] points, IPoint1Argument minimum, IPoint1Argument quadraticApproximation)
    {
        IPoint1Argument[] tempPoints = { points[0], points[1], points[2], quadraticApproximation };

        SortPoints(tempPoints);

        IPoint1Argument best = ChooseTheBestPoint(minimum, quadraticApproximation);

        if (best == tempPoints[3])
        {
            points[2] = tempPoints[3];
            points[1] = tempPoints[2];
            points[0] = tempPoints[1];
        }
        else if (best == tempPoints[0])
        {
            points[0] = tempPoints[0];
            points[1] = tempPoints[1];
            points[2] = tempPoints[2];
        }
        else
        {
            for (int i = 1; i < 3; i++)
            {
                if (best == tempPoints[i])
                {
                    points[1] = tempPoints[i];
                    points[0] = tempPoints[i - 1];
                    points[2] = tempPoints[i + 1];
                    break;
                }
            }
        }
    }

    public void SortPoints(IPoint1Argument[] points)
    {
        for (int i = 1; i < points.Length; i++)
            for (int j = 0; j < points.Length - i; j++)
                if (points[j].X > points[j + 1].X)
                    Swap(ref points[j], ref points[j + 1]);
    }

    public void Swap(ref IPoint1Argument point1, ref IPoint1Argument point2)
    {
        IPoint1Argument pointTemp = point1;
        point1 = point2;
        point2 = pointTemp;
    }

    public IPoint1Argument ChooseTheBestPoint(IPoint1Argument minimum, IPoint1Argument quadraticApproximation)
    {
        if (quadraticApproximation.Y < minimum.Y)
            return quadraticApproximation;
        return minimum;
    }
	
	public ILogger Ui { get; set; }
    public Func<double, double> ObjectiveFunction { get; set; }
}