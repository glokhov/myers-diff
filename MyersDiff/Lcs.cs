namespace MyersDiff;

/// <summary>
///  Longest Common Subsequence — string convenience overloads.
/// </summary>
public static class Lcs
{
    /// <summary>
    ///  Builds the longest common subsequence of two strings using the default character comparer.
    /// </summary>
    /// <param name="a">The original string.</param>
    /// <param name="b">The modified string.</param>
    /// <returns>The longest common subsequence as a string.</returns>
    public static string Build(string a, string b)
    {
        return Build(a, b, EqualityComparer<char>.Default);
    }

    /// <summary>
    ///  Builds the longest common subsequence of two strings using a custom character comparer.
    /// </summary>
    /// <param name="a">The original string.</param>
    /// <param name="b">The modified string.</param>
    /// <param name="comparer">The equality comparer used to compare characters.</param>
    /// <returns>The longest common subsequence as a string.</returns>
    public static string Build(string a, string b, EqualityComparer<char> comparer)
    {
        return new string(Lcs<char>.Build(a, b, comparer));
    }
}

/// <summary>
///  Longest Common Subsequence — generic overloads.
/// </summary>
/// <typeparam name="T">The type of elements in the sequences.</typeparam>
public static class Lcs<T> where T : IEquatable<T>
{
    /// <summary>
    ///  Builds the longest common subsequence of two sequences using the default equality comparer.
    /// </summary>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <returns>An array containing the longest common subsequence.</returns>
    public static T[] Build(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
    {
        return Build(a, b, EqualityComparer<T>.Default);
    }

    /// <summary>
    ///  Builds the longest common subsequence of two sequences using a custom equality comparer.
    /// </summary>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <param name="comparer">The equality comparer used to compare elements.</param>
    /// <returns>An array containing the longest common subsequence.</returns>
    public static T[] Build(ReadOnlySpan<T> a, ReadOnlySpan<T> b, EqualityComparer<T> comparer)
    {
        var list = new List<T>();
        var path = new Path();

        Algorithm.LcsSes(a, b, comparer, path);

        var trace = new Trace(path, Trace.Filter.Eq);

        foreach (var item in trace.Enumerate(a.Length, b.Length))
        {
            list.Add(a[item.X - 1]);
        }

        return list.ToArray();
    }
}