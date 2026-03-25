namespace MyersDiff;

internal sealed class Vector<T>(int max)
{
    private readonly T[] _items = new T[max * 2 + 1];

    public T this[int i]
    {
        get => _items[i + max];
        set => _items[i + max] = value;
    }
}