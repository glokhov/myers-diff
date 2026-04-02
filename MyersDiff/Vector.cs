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
}