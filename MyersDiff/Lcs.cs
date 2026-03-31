namespace MyersDiff;

/// <summary>
///  Longest Common Subsequence.
/// </summary>
/// <typeparam name="T">The type of elements in the sequences.</typeparam>
public static class Lcs<T>
{
    private const Trace.Filter Filter = Trace.Filter.Eq;

    /// <summary>
    ///  Builds the longest common subsequence of two sequences using the default equality comparer.
    /// </summary>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <returns>A list containing the longest common subsequence.</returns>
    public static List<T> Build(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
    {
        return Build(a, b, EqualityComparer<T>.Default);
    }

    /// <summary>
    ///  Builds the longest common subsequence of two sequences using an explicit equality comparer.
    /// </summary>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <param name="comparer">The equality comparer used to compare elements.</param>
    /// <returns>A list containing the longest common subsequence.</returns>
    public static List<T> Build(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer)
    {
        ArgumentNullException.ThrowIfNull(comparer);

        var list = new List<T>();

        var path = Algorithm.LcsSes(a, b, comparer);

        foreach (var edit in Trace.GetEdits(path, Filter))
        {
            list.Add(a[edit.X - 1]);
        }

        return list;
    }
}