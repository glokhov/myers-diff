namespace MyersDiff.Tests;

public sealed class LcsTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";
    private const string S = "caba";

    [Fact]
    public void Test_Subsequence()
    {
        var lcs = new Lcs<char>(A.ToCharArray());

        Algorithm.LcsSes(A, B, EqualityComparer<char>.Default, lcs);

        var subsequence = lcs.Subsequence();

        Assert.Equal(S.ToCharArray(), subsequence);
    }
}