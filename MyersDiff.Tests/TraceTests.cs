namespace MyersDiff.Tests;

public sealed class TraceTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";

    private sealed class Proxy(List<Vector> path) : Trace(path, new Configuration { ReturnEqual = true })
    {
        public (int X, int Y, Cmd Cmd)[] Build(int n, int m)
        {
            return Enumerate(n, m).ToArray();
        }
    }

    [Fact]
    public void Test_Build()
    {
        var path = new List<Vector>();

        Algorithm.LcsSes(A, B, EqualityComparer<char>.Default, path);

        var trace = new Proxy(path).Build(A.Length, B.Length);

        Assert.Equal([(3, 1, Cmd.Eq), (4, 3, Cmd.Eq), (5, 4, Cmd.Eq), (7, 5, Cmd.Eq)], trace);
    }
}