namespace MyersDiff.Tests;

public sealed class AlgorithmTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";

    [Fact]
    public void Test_LcsSes()
    {
        var path = new Path();

        Algorithm.LcsSes(A, B, EqualityComparer<char>.Default, path);

        Assert.Equal(5, path.Snapshots.Count);
        Assert.Equal([(0, 0)], path.Snapshots[0].GetReversePoints());
        Assert.Equal([(1, 0), (0, 1)], path.Snapshots[1].GetReversePoints());
        Assert.Equal([(3, 1), (2, 2), (2, 4)], path.Snapshots[2].GetReversePoints());
        Assert.Equal([(5, 2), (5, 4), (4, 5), (3, 6)], path.Snapshots[3].GetReversePoints());
        Assert.Equal([(7, 3), (7, 5), (5, 5), (4, 6), (3, 7)], path.Snapshots[4].GetReversePoints());
    }

    [Fact]
    public void Test_LcsSes_Empty()
    {
        var path = new Path();

        Algorithm.LcsSes("", "", EqualityComparer<char>.Default, path);

        Assert.Empty(path.Snapshots);
    }
}