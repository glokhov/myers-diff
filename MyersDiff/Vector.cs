namespace MyersDiff;

public sealed class Vector(int max)
{
    private readonly int[] _v = new int[max * 2 + 1];

    public int this[int k]
    {
        get => _v[k + max];
        set => _v[k + max] = value;
    }

    public bool HasDiagonal(int k) => Math.Abs(k) <= max && (k + max) % 2 == 0;

    public Vector Copy(int d)
    {
        var v = new Vector(d);

        Array.Copy(_v, -d + max, v._v, 0, v._v.Length);

        return v;
    }
}