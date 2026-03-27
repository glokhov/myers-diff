namespace MyersDiff.Tests;

public sealed class AlgorithmTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";

    [Fact]
    public void Test_LcsSes()
    {
        var path = new List<Vector>();

        Algorithm.LcsSes(A, B, EqualityComparer<char>.Default, path);

        // Assert.Equal(6, path.Count);
        Assert.Equal([(0, 0)], path[0].Points);
        Assert.Equal([(0, 1), (1, 0)], path[1].Points);
        Assert.Equal([(2, 4), (2, 2), (3, 1)], path[2].Points);
        Assert.Equal([(3, 6), (4, 5), (5, 4), (5, 2)], path[3].Points);
        Assert.Equal([(3, 7), (4, 6), (5, 5), (7, 5), (7, 3)], path[4].Points);
        // Assert.Equal([(3, 8), (4, 7), (5, 6), (7, 6), (5, 2), (0, -5)], path[5].Points);
    }
}