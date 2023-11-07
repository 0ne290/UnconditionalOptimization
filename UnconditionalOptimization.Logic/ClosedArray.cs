namespace UnconditionalOptimization.Logic;

public class ClosedArray<T>
{
    private T[] _array;

    public ClosedArray(T[] array) => _array = array;

    public T this[int i]
    {
        get => _array[(i % _array.Length + _array.Length) % _array.Length];
        set => _array[(i % _array.Length + _array.Length) % _array.Length] = value;
    }
}