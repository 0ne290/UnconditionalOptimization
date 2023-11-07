using UnconditionalOptimization.Interfaces;

namespace UnconditionalOptimization.Logic;

public class Factories
{
    public OneDimensionalSearch OneDimensionalSearchFactory<T>() where T : ILogger, new() =>
        new OneDimensionalSearch(new T());
    public MultiDimensionalSearch MultiDimensionalSearchFactory<T>() where T : ILogger, new() =>
		new MultiDimensionalSearch(new T());
}