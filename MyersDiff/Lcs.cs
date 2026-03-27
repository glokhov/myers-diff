namespace MyersDiff;

/// <summary>
///  Longest Common Subsequence
/// </summary>
/// <param name="a">Source sequence</param>
/// <typeparam name="T">Type of elements</typeparam>
public sealed class Lcs<T>(IReadOnlyList<T> a) : Tracer
{
    protected override bool ReturnDelete => false;

    protected override bool ReturnInsert => false;

    protected override bool ReturnEqual => true;

    public T[] Build()
    {
        return Enumerate(N, M).Select(item => a[item.X - 1]).ToArray();
    }
}