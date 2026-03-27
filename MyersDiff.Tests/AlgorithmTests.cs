namespace MyersDiff.Tests;

public sealed class AlgorithmTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";

    [Fact]
    public void Test_LcsSes()
    {
        var tracer = new InternalTracer();

        Algorithm.LcsSes(A, B, EqualityComparer<char>.Default, tracer);

        Assert.Equal(5, tracer.Paths.Count);
        Assert.Equal([(0, 0)], tracer.Paths[0]);
        Assert.Equal([(0, 1), (1, 0)], tracer.Paths[1]);
        Assert.Equal([(2, 4), (2, 2), (3, 1)], tracer.Paths[2]);
        Assert.Equal([(3, 6), (4, 5), (5, 4), (5, 2)], tracer.Paths[3]);
        Assert.Equal([(3, 7), (4, 6), (5, 5), (7, 5), (7, 3)], tracer.Paths[4]);
    }
}