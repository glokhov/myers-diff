namespace MyersDiff.Tests;

public sealed class ComputeLcsTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";
    private const string S = "baba";

    [Fact]
    public void Test_Comparer()
    {
        Assert.Equal(S, Algorithm.ComputeLcs(A, B, EqualityComparer<char>.Default).ToArray());
    }

    [Fact]
    public void Test()
    {
        Assert.Equal(S, Algorithm.ComputeLcs<char>(A, B).ToArray());
    }

    [Fact]
    public void Test_BothEmpty()
    {
        Assert.Equal("", Algorithm.ComputeLcs<char>("", "").ToArray());
    }

    [Fact]
    public void Test_FirstEmpty()
    {
        Assert.Equal("", Algorithm.ComputeLcs<char>("", "abc").ToArray());
    }

    [Fact]
    public void Test_SecondEmpty()
    {
        Assert.Equal("", Algorithm.ComputeLcs<char>("abc", "").ToArray());
    }

    [Fact]
    public void Test_Identical()
    {
        Assert.Equal("abc", Algorithm.ComputeLcs<char>("abc", "abc").ToArray());
    }

    [Fact]
    public void Test_Disjoint()
    {
        Assert.Equal("", Algorithm.ComputeLcs<char>("abc", "xyz").ToArray());
    }

    [Fact]
    public void Test_SingleCharSame()
    {
        Assert.Equal("a", Algorithm.ComputeLcs<char>("a", "a").ToArray());
    }

    [Fact]
    public void Test_SingleCharDifferent()
    {
        Assert.Equal("", Algorithm.ComputeLcs<char>("a", "b").ToArray());
    }

    [Fact]
    public void Test_Prefix()
    {
        Assert.Equal("abc", Algorithm.ComputeLcs<char>("abc", "abcd").ToArray());
    }

    [Fact]
    public void Test_Suffix()
    {
        Assert.Equal("bcd", Algorithm.ComputeLcs<char>("abcd", "bcd").ToArray());
    }

    [Fact]
    public void Test_ExplicitComparer()
    {
        Assert.Equal("abc", Algorithm.ComputeLcs("abc", "ABC", ExplicitComparer.Instance).ToArray());
    }
}