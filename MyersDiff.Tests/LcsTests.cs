namespace MyersDiff.Tests;

public sealed class LcsTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";
    private const string S = "caba";

    [Fact]
    public void Test_Build_Comparer()
    {
        Assert.Equal(S, Lcs<char>.Build(A, B, EqualityComparer<char>.Default).ToArray());
    }

    [Fact]
    public void Test_Build()
    {
        Assert.Equal(S, Lcs<char>.Build(A, B).ToArray());
    }

    [Fact]
    public void Test_Build_BothEmpty()
    {
        Assert.Equal("", Lcs<char>.Build("", "").ToArray());
    }

    [Fact]
    public void Test_Build_FirstEmpty()
    {
        Assert.Equal("", Lcs<char>.Build("", "abc").ToArray());
    }

    [Fact]
    public void Test_Build_SecondEmpty()
    {
        Assert.Equal("", Lcs<char>.Build("abc", "").ToArray());
    }

    [Fact]
    public void Test_Build_Identical()
    {
        Assert.Equal("abc", Lcs<char>.Build("abc", "abc").ToArray());
    }

    [Fact]
    public void Test_Build_Disjoint()
    {
        Assert.Equal("", Lcs<char>.Build("abc", "xyz").ToArray());
    }

    [Fact]
    public void Test_Build_SingleCharSame()
    {
        Assert.Equal("a", Lcs<char>.Build("a", "a").ToArray());
    }

    [Fact]
    public void Test_Build_SingleCharDifferent()
    {
        Assert.Equal("", Lcs<char>.Build("a", "b").ToArray());
    }

    [Fact]
    public void Test_Build_Prefix()
    {
        Assert.Equal("abc", Lcs<char>.Build("abc", "abcd").ToArray());
    }

    [Fact]
    public void Test_Build_Suffix()
    {
        Assert.Equal("bcd", Lcs<char>.Build("abcd", "bcd").ToArray());
    }

    [Fact]
    public void Test_Build_ExplicitComparer()
    {
        Assert.Equal("abc", Lcs<char>.Build("abc", "ABC", ExplicitComparer.Instance).ToArray());
    }
}