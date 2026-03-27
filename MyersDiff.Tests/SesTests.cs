namespace MyersDiff.Tests;

public sealed class SesTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";

    [Fact]
    public void Test_Build()
    {
        var ses = new Ses<char>(B.ToCharArray());

        Algorithm.LcsSes(A, B, EqualityComparer<char>.Default, ses);

        var script = ses.Build();

        Assert.Equal([(1, Cmd.Del, '\0'), (2, Cmd.Del, '\0'), (3, Cmd.Ins, 'b'), (6, Cmd.Del, '\0'), (7, Cmd.Ins, 'c')], script);
    }
}