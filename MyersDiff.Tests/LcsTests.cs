namespace MyersDiff.Tests;

public sealed class LcsTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";
    private const string S = "caba";

    [Fact]
    public void Test_Build()
    {
        var lcs = new Lcs<char>(A.ToCharArray());

        Algorithm.LcsSes(A, B, EqualityComparer<char>.Default, lcs);

        var subsequence = lcs.Build();

        Assert.Equal(S.ToCharArray(), subsequence);
    }
}