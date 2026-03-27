namespace MyersDiff;

/// <summary>
///  Longest Common Subsequence
/// </summary>
/// <param name="a">Source sequence</param>
/// <typeparam name="T">Type of elements</typeparam>
public sealed class Lcs<T>(IReadOnlyList<T> a) : Tracer
{
    public T[] Subsequence()
    {
        return Enumerate(N, M).Select(t => a[t.X - 1]).ToArray();
    }
}