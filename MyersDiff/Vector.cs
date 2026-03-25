namespace MyersDiff;

internal class Vector<T>(int length)
{
    private readonly T[] _vector = new T[length * 2 + 1];

    public T this[int i]
    {
        get => _vector[i + length];
        set => _vector[i + length] = value;
    }
}