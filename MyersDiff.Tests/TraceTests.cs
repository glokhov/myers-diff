namespace MyersDiff.Tests;

public sealed class TraceTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";
    private const Trace.Filter Filter = Trace.Filter.Del | Trace.Filter.Ins | Trace.Filter.Eq;

    [Fact]
    public void Test_Enumerate()
    {
        var path = Algorithm.LcsSes(A, B, EqualityComparer<char>.Default);

        var edits = new Trace(path, Filter).EnumerateEdits();

        Assert.Equal(
            [
                new Trace.Edit(1, 0, Trace.Op.Del),
                new Trace.Edit(2, 0, Trace.Op.Del),
                new Trace.Edit(3, 1, Trace.Op.Eq),
                new Trace.Edit(3, 2, Trace.Op.Ins),
                new Trace.Edit(4, 3, Trace.Op.Eq),
                new Trace.Edit(5, 4, Trace.Op.Eq),
                new Trace.Edit(6, 4, Trace.Op.Del),
                new Trace.Edit(7, 5, Trace.Op.Eq),
                new Trace.Edit(7, 6, Trace.Op.Ins)
            ],
            edits);
    }

    [Fact]
    public void Test_Enumerate_BothEmpty()
    {
        var path = Algorithm.LcsSes("", "", EqualityComparer<char>.Default);

        var edits = new Trace(path, Filter).EnumerateEdits();

        Assert.Empty(edits);
    }

    [Fact]
    public void Test_Enumerate_FirstEmpty()
    {
        var path = Algorithm.LcsSes("", "abc", EqualityComparer<char>.Default);

        var edits = new Trace(path, Filter).EnumerateEdits();

        Assert.Equal(
            [
                new Trace.Edit(0, 1, Trace.Op.Ins),
                new Trace.Edit(0, 2, Trace.Op.Ins),
                new Trace.Edit(0, 3, Trace.Op.Ins)
            ],
            edits);
    }

    [Fact]
    public void Test_Enumerate_SecondEmpty()
    {
        var path = Algorithm.LcsSes("abc", "", EqualityComparer<char>.Default);

        var edits = new Trace(path, Filter).EnumerateEdits();

        Assert.Equal(
            [
                new Trace.Edit(1, 0, Trace.Op.Del),
                new Trace.Edit(2, 0, Trace.Op.Del),
                new Trace.Edit(3, 0, Trace.Op.Del)
            ],
            edits);
    }

    [Fact]
    public void Test_Enumerate_Identical()
    {
        var path = Algorithm.LcsSes("abc", "abc", EqualityComparer<char>.Default);

        var edits = new Trace(path, Filter).EnumerateEdits();

        Assert.Equal(
            [
                new Trace.Edit(1, 1, Trace.Op.Eq),
                new Trace.Edit(2, 2, Trace.Op.Eq),
                new Trace.Edit(3, 3, Trace.Op.Eq)
            ],
            edits);
    }

    [Fact]
    public void Test_Enumerate_Disjoint()
    {
        var path = Algorithm.LcsSes("abc", "xyz", EqualityComparer<char>.Default);

        var edits = new Trace(path, Filter).EnumerateEdits();

        Assert.Equal(
            [
                new Trace.Edit(1, 0, Trace.Op.Del),
                new Trace.Edit(2, 0, Trace.Op.Del),
                new Trace.Edit(3, 0, Trace.Op.Del),
                new Trace.Edit(3, 1, Trace.Op.Ins),
                new Trace.Edit(3, 2, Trace.Op.Ins),
                new Trace.Edit(3, 3, Trace.Op.Ins)
            ],
            edits);
    }

    [Fact]
    public void Test_Enumerate_SingleCharDifferent()
    {
        var path = Algorithm.LcsSes("a", "b", EqualityComparer<char>.Default);

        var edits = new Trace(path, Filter).EnumerateEdits();

        Assert.Equal(
            [
                new Trace.Edit(1, 0, Trace.Op.Del),
                new Trace.Edit(1, 1, Trace.Op.Ins)
            ],
            edits);
    }

    [Fact]
    public void Test_Enumerate_ExplicitComparer()
    {
        var path = Algorithm.LcsSes("abX", "ABY", ExplicitComparer.Instance);

        var edits = new Trace(path, Filter).EnumerateEdits();

        Assert.Equal(
            [
                new Trace.Edit(1, 1, Trace.Op.Eq),
                new Trace.Edit(2, 2, Trace.Op.Eq),
                new Trace.Edit(3, 2, Trace.Op.Del),
                new Trace.Edit(3, 3, Trace.Op.Ins)
            ],
            edits);
    }
}
