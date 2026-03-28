namespace MyersDiff.Tests;

public sealed class LcsTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";
    private const string S = "caba";

    [Fact]
    public void Test_Build()
    {
        var path = new Path();

        Algorithm.LcsSes(A, B, EqualityComparer<char>.Default, path);

        var lcs = new Lcs<char>(A.ToCharArray(), path).Build(A.Length, B.Length);

        Assert.Equal(S.ToCharArray(), lcs);
    }
}