namespace MyersDiff.Tests;

public sealed class LcsTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";
    private const string S = "caba";

    [Fact]
    public void Test_Build_Comparer_String()
    {
        Assert.Equal(S, Lcs.Build(A, B, EqualityComparer<char>.Default));
    }

    [Fact]
    public void Test_Build_Comparer()
    {
        Assert.Equal(S, Lcs<char>.Build(A, B, EqualityComparer<char>.Default));
    }

    [Fact]
    public void Test_Build_String()
    {
        Assert.Equal(S, Lcs.Build(A, B));
    }

    [Fact]
    public void Test_Build()
    {
        Assert.Equal(S, Lcs<char>.Build(A, B));
    }

    [Fact]
    public void Test_Build_BothEmpty()
    {
        Assert.Equal("", Lcs.Build("", ""));
    }

    [Fact]
    public void Test_Build_FirstEmpty()
    {
        Assert.Equal("", Lcs.Build("", "abc"));
    }

    [Fact]
    public void Test_Build_SecondEmpty()
    {
        Assert.Equal("", Lcs.Build("abc", ""));
    }

    [Fact]
    public void Test_Build_Identical()
    {
        Assert.Equal("abc", Lcs.Build("abc", "abc"));
    }

    [Fact]
    public void Test_Build_Disjoint()
    {
        Assert.Equal("", Lcs.Build("abc", "xyz"));
    }

    [Fact]
    public void Test_Build_SingleCharSame()
    {
        Assert.Equal("a", Lcs.Build("a", "a"));
    }

    [Fact]
    public void Test_Build_SingleCharDifferent()
    {
        Assert.Equal("", Lcs.Build("a", "b"));
    }

    [Fact]
    public void Test_Build_Prefix()
    {
        Assert.Equal("abc", Lcs.Build("abc", "abcd"));
    }

    [Fact]
    public void Test_Build_Suffix()
    {
        Assert.Equal("bcd", Lcs.Build("abcd", "bcd"));
    }

    [Fact]
    public void Test_Build_ExplicitComparer_String()
    {
        Assert.Equal("abc", Lcs.Build("abc", "ABC", ExplicitComparer.Instance));
    }

    [Fact]
    public void Test_Build_ExplicitComparer()
    {
        Assert.Equal("abc", Lcs<char>.Build("abc", "ABC", ExplicitComparer.Instance));
    }
}