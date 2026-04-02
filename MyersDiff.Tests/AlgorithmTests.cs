namespace MyersDiff.Tests;

public sealed class AlgorithmTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";

    private static readonly IEqualityComparer<char> Comparer = EqualityComparer<char>.Default;

    [Fact]
    public void Test_FindMiddleSnake()
    {
        var (d, x, y, u, v) = Algorithm.FindMiddleSnake(A, B, Comparer);

        Assert.True(d > 0);
        Assert.Equal(u - x, v - y); // diagonal
    }

    [Fact]
    public void Test_FindMiddleSnake_Identical()
    {
        var (d, x, y, u, v) = Algorithm.FindMiddleSnake("abc", "abc", Comparer);

        Assert.Equal(0, d);
        Assert.Equal(0, x);
        Assert.Equal(0, y);
        Assert.Equal(3, u);
        Assert.Equal(3, v);
    }

    [Fact]
    public void Test_FindMiddleSnake_SingleCharSame()
    {
        var (d, x, y, u, v) = Algorithm.FindMiddleSnake("a", "a", Comparer);

        Assert.Equal(0, d);
        Assert.Equal(0, x);
        Assert.Equal(0, y);
        Assert.Equal(1, u);
        Assert.Equal(1, v);
    }

    [Fact]
    public void Test_FindMiddleSnake_SingleCharDifferent()
    {
        var (d, _, _, _, _) = Algorithm.FindMiddleSnake("a", "b", Comparer);

        Assert.Equal(2, d);
    }

    [Fact]
    public void Test_FindMiddleSnake_Disjoint()
    {
        var (d, x, y, u, v) = Algorithm.FindMiddleSnake("abc", "xyz", Comparer);

        Assert.Equal(6, d);
        Assert.Equal(u - x, v - y);
    }

    [Fact]
    public void Test_FindMiddleSnake_Prefix()
    {
        var (d, _, _, _, _) = Algorithm.FindMiddleSnake("abc", "abcd", Comparer);

        Assert.Equal(1, d);
    }

    [Fact]
    public void Test_FindMiddleSnake_Suffix()
    {
        var (d, _, _, _, _) = Algorithm.FindMiddleSnake("abcd", "bcd", Comparer);

        Assert.Equal(1, d);
    }

    /// <summary>
    ///  Odd n+m sums with fully disjoint sequences require ⌈(n+m)/2⌉ iterations.
    ///  These cases directly caught the ceiling-division bug.
    /// </summary>
    [Theory]
    [InlineData("ab", "c", 3)]
    [InlineData("a", "bc", 3)]
    [InlineData("abc", "de", 5)]
    [InlineData("ab", "cde", 5)]
    [InlineData("abcde", "fg", 7)]
    [InlineData("ab", "cdefg", 7)]
    public void Test_FindMiddleSnake_OddSumDisjoint(string a, string b, int expectedD)
    {
        var (d, x, y, u, v) = Algorithm.FindMiddleSnake(a, b, Comparer);

        Assert.Equal(expectedD, d);
        Assert.Equal(u - x, v - y);
    }

    /// <summary>
    ///  Verifies the snake diagonal elements actually match in the original sequences.
    /// </summary>
    [Theory]
    [InlineData("abcabba", "cbabac")]
    [InlineData("axb", "ay")]
    [InlineData("abxcd", "abyc")]
    [InlineData("hello world", "hallo welt")]
    public void Test_FindMiddleSnake_DiagonalElementsMatch(string a, string b)
    {
        var (_, x, y, u, v) = Algorithm.FindMiddleSnake(a, b, Comparer);

        var snakeLen = u - x;

        Assert.Equal(snakeLen, v - y);

        for (var i = 0; i < snakeLen; i++)
        {
            Assert.Equal(a[x + i], b[y + i]);
        }
    }

    /// <summary>
    ///  Verifies snake coordinates are within bounds.
    /// </summary>
    [Theory]
    [InlineData("abcabba", "cbabac")]
    [InlineData("a", "b")]
    [InlineData("abc", "xyz")]
    [InlineData("ab", "c")]
    [InlineData("a", "bc")]
    [InlineData("abc", "abcd")]
    [InlineData("abcd", "bcd")]
    public void Test_FindMiddleSnake_BoundsValid(string a, string b)
    {
        var (d, x, y, u, v) = Algorithm.FindMiddleSnake(a, b, Comparer);

        Assert.True(d >= 0);
        Assert.True(x >= 0 && x <= a.Length);
        Assert.True(y >= 0 && y <= b.Length);
        Assert.True(u >= x && u <= a.Length);
        Assert.True(v >= y && v <= b.Length);
    }
}