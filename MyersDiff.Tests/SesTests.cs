using static MyersDiff.Cmd;

namespace MyersDiff.Tests;

public sealed class SesTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";

    [Fact]
    public void Test_Build()
    {
        var path = new List<Vector>();

        Algorithm.LcsSes(A, B, EqualityComparer<char>.Default, path);

        var ses = new Ses<char>(B.ToCharArray(), path).Build(A.Length, B.Length);

        Assert.Equal([(1, Del, '\0'), (2, Del, '\0'), (3, Ins, 'b'), (6, Del, '\0'), (7, Ins, 'c')], ses);
    }
}