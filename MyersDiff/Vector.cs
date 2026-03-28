namespace MyersDiff;

public sealed class Vector(int max)
{
    private readonly int[] _v = new int[max * 2 + 1];

    public int this[int k]
    {
        get => _v[k + max];
        set => _v[k + max] = value;
    }

    public IEnumerable<(int X, int Y)> Points
    {
        get
        {
            for (var k = -max; k <= max; k += 2)
            {
                yield return (this[k], this[k] - k);
            }
        }
    }

    public Vector Clone(int d)
    {
        var v = new Vector(d);

        Array.Copy(_v, -d + max, v._v, 0, v._v.Length);

        return v;
    }
}