namespace MyersDiff.Tests;

public sealed class AlgorithmTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";

    [Fact]
    public void LcsSesTest()
    {
        Assert.Equal(5, Algorithm.LcsSes<char>(A, B));
    }
}