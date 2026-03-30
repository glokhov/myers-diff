namespace MyersDiff.Tests;

public sealed class AlgorithmTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";

    [Fact]
    public void Test_LcsSes()
    {
        var path = Algorithm.LcsSes(A, B, EqualityComparer<char>.Default);

        // [(0, 0)]
        // [(0, 1), (1, 0)]
        // [(2, 4), (2, 2), (3, 1)]
        // [(3, 6), (4, 5), (5, 4), (5, 2)]
        // [(3, 7), (4, 6), (5, 5), (7, 5), (7, 3)]

        Assert.Equal(5, path.Snapshots.Count);

        // d=0: k=0 → x=0
        var s0 = path.Snapshots[0];
        Assert.True(s0.HasDiagonal(0));
        Assert.Equal(0, s0[0]);

        // d=1: k=-1 → x=0, k=1 → x=1
        var s1 = path.Snapshots[1];
        Assert.True(s1.HasDiagonal(-1));
        Assert.True(s1.HasDiagonal(1));
        Assert.Equal(0, s1[-1]);
        Assert.Equal(1, s1[1]);

        // d=2: k=-2 → x=2, k=0 → x=2, k=2 → x=3
        var s2 = path.Snapshots[2];
        Assert.True(s2.HasDiagonal(-2));
        Assert.True(s2.HasDiagonal(0));
        Assert.True(s2.HasDiagonal(2));
        Assert.Equal(2, s2[-2]);
        Assert.Equal(2, s2[0]);
        Assert.Equal(3, s2[2]);

        // d=3: k=-3 → x=3, k=-1 → x=4, k=1 → x=5, k=3 → x=5
        var s3 = path.Snapshots[3];
        Assert.True(s3.HasDiagonal(-3));
        Assert.True(s3.HasDiagonal(-1));
        Assert.True(s3.HasDiagonal(1));
        Assert.True(s3.HasDiagonal(3));
        Assert.Equal(3, s3[-3]);
        Assert.Equal(4, s3[-1]);
        Assert.Equal(5, s3[1]);
        Assert.Equal(5, s3[3]);

        // d=4:
        // k=-4 → x=3, k=-2 → x=4, k=0 → x=5, k=2 → x=7, k=4 → x=7
        var s4 = path.Snapshots[4];
        Assert.True(s4.HasDiagonal(-4));
        Assert.True(s4.HasDiagonal(-2));
        Assert.True(s4.HasDiagonal(0));
        Assert.True(s4.HasDiagonal(2));
        Assert.True(s4.HasDiagonal(4));
        Assert.Equal(3, s4[-4]);
        Assert.Equal(4, s4[-2]);
        Assert.Equal(5, s4[0]);
        Assert.Equal(7, s4[2]);
        Assert.Equal(7, s4[4]);
    }

    [Fact]
    public void Test_LcsSes_Empty()
    {
        Assert.Empty(Algorithm.LcsSes("", "", EqualityComparer<char>.Default).Snapshots);
    }

    [Fact]
    public void Test_LcsSes_ExplicitComparer()
    {
        Assert.Empty(Algorithm.LcsSes("abc", "ABC", ExplicitComparer.Instance).Snapshots);
    }
}