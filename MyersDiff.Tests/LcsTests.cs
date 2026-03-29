namespace MyersDiff.Tests;

public sealed class LcsTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";
    private const string S = "caba";

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
}