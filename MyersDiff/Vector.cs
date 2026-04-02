namespace MyersDiff;

/// <summary>
///  An integer array indexed by diagonal <c>k ∈ [-max, +max]</c>, used by the Myers algorithm
///  to track the furthest-reaching x-coordinate on each diagonal.
/// </summary>
/// <param name="max">The maximum absolute diagonal index.</param>
public sealed class Vector(int max)
{
    private readonly int[] _v = new int[max * 2 + 1];

    /// <summary>
    ///  Gets or sets the furthest-reaching x-coordinate on diagonal <paramref name="k"/>.
    /// </summary>
    /// <param name="k">The diagonal index.</param>
    public int this[int k]
    {
        get => _v[k + max];
        set => _v[k + max] = value;
    }

    /// <summary>
    ///  Creates a copy of this vector trimmed to the range <c>[-d, +d]</c>.
    /// </summary>
    /// <param name="d">The d-step value defining the copy range.</param>
    /// <returns>A new <see cref="Vector"/> containing only the diagonals in <c>[-d, +d]</c>.</returns>
    public Vector Copy(int d)
    {
        var v = new Vector(d);

        Array.Copy(_v, -d + max, v._v, 0, v._v.Length);

        return v;
    }
}