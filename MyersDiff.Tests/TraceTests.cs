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

    [Fact]
    public void Test_Build_BothEmpty()
    {
        var path = new Path();

        Algorithm.LcsSes("", "", EqualityComparer<char>.Default, path);

        var trace = new Trace(path, Filter).Enumerate(0, 0);

        Assert.Empty(trace);
    }

    [Fact]
    public void Test_Build_FirstEmpty()
    {
        var path = new Path();

        Algorithm.LcsSes("", "abc", EqualityComparer<char>.Default, path);

        var trace = new Trace(path, Filter).Enumerate(0, 3);

        Assert.Equal(
            [
                (0, 1, Trace.Op.Ins),
                (0, 2, Trace.Op.Ins),
                (0, 3, Trace.Op.Ins)
            ],
            trace);
    }

    [Fact]
    public void Test_Build_SecondEmpty()
    {
        var path = new Path();

        Algorithm.LcsSes("abc", "", EqualityComparer<char>.Default, path);

        var trace = new Trace(path, Filter).Enumerate(3, 0);

        Assert.Equal(
            [
                (1, 0, Trace.Op.Del),
                (2, 0, Trace.Op.Del),
                (3, 0, Trace.Op.Del)
            ],
            trace);
    }

    [Fact]
    public void Test_Build_Identical()
    {
        var path = new Path();

        Algorithm.LcsSes("abc", "abc", EqualityComparer<char>.Default, path);

        var trace = new Trace(path, Filter).Enumerate(3, 3);

        Assert.Equal(
            [
                (1, 1, Trace.Op.Eq),
                (2, 2, Trace.Op.Eq),
                (3, 3, Trace.Op.Eq)
            ],
            trace);
    }

    [Fact]
    public void Test_Build_Disjoint()
    {
        var path = new Path();

        Algorithm.LcsSes("abc", "xyz", EqualityComparer<char>.Default, path);

        var trace = new Trace(path, Filter).Enumerate(3, 3);

        Assert.Equal(
            [
                (1, 0, Trace.Op.Del),
                (2, 0, Trace.Op.Del),
                (3, 0, Trace.Op.Del),
                (3, 1, Trace.Op.Ins),
                (3, 2, Trace.Op.Ins),
                (3, 3, Trace.Op.Ins)
            ],
            trace);
    }

    [Fact]
    public void Test_Build_SingleCharDifferent()
    {
        var path = new Path();

        Algorithm.LcsSes("a", "b", EqualityComparer<char>.Default, path);

        var trace = new Trace(path, Filter).Enumerate(1, 1);

        Assert.Equal(
            [
                (1, 0, Trace.Op.Del),
                (1, 1, Trace.Op.Ins)
            ],
            trace);
    }
}