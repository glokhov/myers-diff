namespace MyersDiff;

/// <summary>
///  Longest Common Subsequence
/// </summary>
public static class Lcs
{
    public static string Build(string a, string b)
    {
        return new string(Lcs<char>.Build(a, b));
    }
}

/// <summary>
///  Longest Common Subsequence
/// </summary>
public static class Lcs<T>
{
    public static T[] Build(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
    {
        var list = new List<T>();
        var path = new Path();

        Algorithm.LcsSes(a, b, EqualityComparer<T>.Default, path);

        var trace = new Trace(path, Trace.Filter.Eq);

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var item in trace.Enumerate(a.Length, b.Length))
        {
            list.Add(a[item.X - 1]);
        }

        return list.ToArray();
    }
}