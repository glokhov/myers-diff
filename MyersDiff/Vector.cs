namespace MyersDiff;

public sealed class Vector(int max)
{
    private readonly int[] _v = new int[max * 2 + 1];

    public int this[int k]
    {
        get => _v[k + max];
        set => _v[k + max] = value;
    }

    public (int X, int Y)[] Points(int d)
    {
        var i = 0;
        var p = new (int X, int Y)[d + 1];

        for (var k = -d; k <= d; k += 2)
        {
            p[i++] = (this[k], this[k] - k);
        }

        return p;
    }
}