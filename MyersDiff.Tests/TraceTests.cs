namespace MyersDiff.Tests;

public sealed class TraceTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";
    private const Trace.Filter Filter = Trace.Filter.Del | Trace.Filter.Ins | Trace.Filter.Eq;

    [Fact]
    public void Test_Build()
    {
        var path = new Path();

        Algorithm.LcsSes(A, B, EqualityComparer<char>.Default, path);

        var trace = new Trace(path, Filter).Enumerate(A.Length, B.Length);

        Assert.Equal(
            [
                (1, 0, Trace.Op.Del),
                (2, 0, Trace.Op.Del),
                (3, 1, Trace.Op.Eq),
                (3, 2, Trace.Op.Ins),
                (4, 3, Trace.Op.Eq),
                (5, 4, Trace.Op.Eq),
                (6, 4, Trace.Op.Del),
                (7, 5, Trace.Op.Eq),
                (7, 6, Trace.Op.Ins)
            ],
            trace);
    }
}