namespace MyersDiff.Tests;

public sealed class TracerTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";

    [Fact]
    public void Test_Trace()
    {
        var tracer = Tracer.Default;

        Algorithm.LcsSes(A, B, EqualityComparer<char>.Default, tracer);

        var trace = tracer.Trace();

        Assert.Equal([(3, 1), (4, 3), (5, 4), (7, 5)], trace);
    }
}