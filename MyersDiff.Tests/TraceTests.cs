namespace MyersDiff.Tests;

public sealed class TraceTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";

    private sealed class TestTrace : Trace;

    [Fact]
    public void Test_GetBackTrace()
    {
        var builder = new TestTrace();

        Algorithm.LcsSes(A, B, EqualityComparer<char>.Default, builder);

        var trace = builder.GetBackTrace((A.Length, B.Length));

        Assert.Equal([(7, 5), (5, 4), (4, 3), (3, 1)], trace);
    }
}