using System.Reflection.Metadata.Ecma335;
using UnconditionalOptimization.Logic;

namespace UnconditionalOptimization.UiConsole
{
    internal class Program
    {
        static void Main()
        {
			PerformAOneDimensionalSearch();
            PerformAMultiDimensionalSearch();
			Console.WriteLine();
			Console.Write("Для завершения программы нажмите любую клавишу...");
			Console.ReadKey();
        }

        static void PerformAOneDimensionalSearch()
        {
			Factories factories = new Factories();
            OneDimensionalSearch oneDimensionalSearch = factories.OneDimensionalSearchFactory<LoggerConsole>();

            Console.WriteLine("Минимизация целевой функции (ЦФ) методом Пауэлла. По умолчанию выбран второй вариант ЦФ:");
            Console.WriteLine();

            Console.Write("Введите значение начальной точки: ");
            double startingPoint = Convert.ToDouble(Console.ReadLine());
            Console.Write("Введите значение приращения: ");
            double increment = Convert.ToDouble(Console.ReadLine());
            Console.Write("Введите значение точности по аргументу: ");
            double accuracyByArgument = Convert.ToDouble(Console.ReadLine());
            Console.Write("Введите значение точности по функции: ");
            double accuracyByFunction = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine();
            var result = oneDimensionalSearch.FunctionMinimizationByPowellsMethod
            (
                startingPoint, increment,
				accuracyByArgument, accuracyByFunction
            );

            Console.WriteLine();
            Console.WriteLine($"За {result.NumberOfIterations} итераций минимизации ЦФ методом Пауэлла была найдена точка оптимума {result.MinimumPoint.X} (значение ЦФ в точке равно {result.MinimumPoint.Y}). Кол-во вычислений ЦФ за все время выполнения алгоритма при выше написанном кол-ве итераций равно {result.NumberOfObjectiveFunctionCalculations}. Коэффициент сходимости {result.ConvergenceCoefficient?.ToString() ?? "вычислить невозможно, т. к. было найдено недостаточное кол-во приближений точки оптимума (необходимо минимум 3)"}");
        }

        static void PerformAMultiDimensionalSearch()
        {
			Factories factories = new Factories();
            MultiDimensionalSearch multiDimensionalSearch = factories.MultiDimensionalSearchFactory<LoggerConsole>();

            Console.WriteLine("Минимизация целевой функции (ЦФ) методом сопряженных направлений Пауэлла. По умолчанию выбран второй вариант ЦФ (размерность = 2):");
            Console.WriteLine();

            Console.Write("Введите первую координату начальной точки: ");
            double startingPointX1 = Convert.ToDouble(Console.ReadLine());
            Console.Write("Введите вторую координату начальной точки: ");
            double startingPointX2 = Convert.ToDouble(Console.ReadLine());
            Console.Write("Введите значение точности по аргументу: ");
            double accuracyByArgument = Convert.ToDouble(Console.ReadLine());
            Console.Write("Введите значение точности по функции: ");
            double accuracyByFunction = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine();
            var result =
                multiDimensionalSearch.FunctionMinimizationByPowellsConjugateDirectionsMethod(new double[] { startingPointX1, startingPointX2 }, accuracyByArgument, accuracyByFunction);

            Console.WriteLine();
            Console.WriteLine($"За {result.NumberOfIterations} итераций минимизации ЦФ методом сопряженных направлений Пауэлла была найдена точка оптимума ({result.MinimumPoint[0]}; {result.MinimumPoint[1]}) (значение ЦФ в точке равно {result.MinimumPoint.Y}). Кол-во вычислений ЦФ за все время выполнения алгоритма при выше написанном кол-ве итераций равно {result.NumberOfObjectiveFunctionCalculations}");
        }
    }
}