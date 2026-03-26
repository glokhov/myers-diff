namespace MyersDiff.Tests;

public sealed class AlgorithmTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";

    private sealed class TestTrace : Trace
    {
        public IReadOnlyList<(int X, int Y)[]> GetPaths() => Paths;
    }

    [Fact]
    public void Test_LcsSes()
    {
        var trace = new TestTrace();

        Algorithm.LcsSes(A, B, EqualityComparer<char>.Default, trace);

        Assert.Equal(5, trace.Length);

        var paths = trace.GetPaths();

        Assert.Equal([(0, 0)], paths[0]);
        Assert.Equal([(0, 1), (1, 0)], paths[1]);
        Assert.Equal([(2, 4), (2, 2), (3, 1)], paths[2]);
        Assert.Equal([(3, 6), (4, 5), (5, 4), (5, 2)], paths[3]);
        Assert.Equal([(3, 7), (4, 6), (5, 5), (7, 5), (7, 3)], paths[4]);
    }
}