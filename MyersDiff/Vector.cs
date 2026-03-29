namespace MyersDiff;

public sealed class Vector(int max)
{
    private readonly int[] _v = new int[max * 2 + 1];

    public int this[int k]
    {
        get => _v[k + max];
        set => _v[k + max] = value;
    }

    public (int X, int Y)[] GetReversePoints()
    {
        var i = 0;
        var points = new (int X, int Y)[max + 1];

        for (var k = max; k >= -max; k -= 2)
        {
            points[i++] = (this[k], this[k] - k);
        }

        return points;
    }

    public Vector Copy(int d)
    {
        var v = new Vector(d);

        Array.Copy(_v, -d + max, v._v, 0, v._v.Length);

        return v;
    }
}