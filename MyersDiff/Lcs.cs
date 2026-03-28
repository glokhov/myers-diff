namespace MyersDiff;

/// <summary>
///  Longest Common Subsequence
/// </summary>
/// <param name="a">Source sequence</param>
/// <typeparam name="T">Type of elements</typeparam>
public sealed class Lcs<T>(IReadOnlyList<T> a, Path path) : Trace(path, Config.Lcs)
{
    public T[] Build(int n, int m)
    {
        return Enumerate(n, m).Select(item => a[item.X - 1]).ToArray();
    }
}