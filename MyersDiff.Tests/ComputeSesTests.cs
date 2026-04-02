namespace MyersDiff.Tests;

public sealed class ComputeSesTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";

    [Fact]
    public void Test()
    {
        var ses = Algorithm.ComputeSes<char>(A, B);

        var a = new Command<char>.Insert(0, 'c');
        var b = new Command<char>.Delete(1);
        var c = new Command<char>.Delete(3);
        var d = new Command<char>.Delete(6);
        var e = new Command<char>.Insert(7, 'c');

        Assert.Equal([a, b, c, d, e], ses);
    }

    [Fact]
    public void Test_Comparer()
    {
        var ses = Algorithm.ComputeSes(A, B, EqualityComparer<char>.Default);

        var a = new Command<char>.Insert(0, 'c');
        var b = new Command<char>.Delete(1);
        var c = new Command<char>.Delete(3);
        var d = new Command<char>.Delete(6);
        var e = new Command<char>.Insert(7, 'c');

        Assert.Equal([a, b, c, d, e], ses);
    }

    [Fact]
    public void Test_BothEmpty()
    {
        Assert.Empty(Algorithm.ComputeSes<char>("", ""));
    }

    [Fact]
    public void Test_FirstEmpty()
    {
        var ses = Algorithm.ComputeSes<char>("", "abc");

        Assert.Equal(
            [
                new Command<char>.Insert(0, 'a'),
                new Command<char>.Insert(0, 'b'),
                new Command<char>.Insert(0, 'c')
            ],
            ses);
    }

    [Fact]
    public void Test_SecondEmpty()
    {
        var ses = Algorithm.ComputeSes<char>("abc", "");

        Assert.Equal(
            [
                new Command<char>.Delete(1),
                new Command<char>.Delete(2),
                new Command<char>.Delete(3)
            ],
            ses);
    }

    [Fact]
    public void Test_Identical()
    {
        Assert.Empty(Algorithm.ComputeSes<char>("abc", "abc"));
    }

    [Fact]
    public void Test_Disjoint()
    {
        var ses = Algorithm.ComputeSes<char>("abc", "xyz");

        Assert.Equal(
            [
                new Command<char>.Insert(0, 'x'),
                new Command<char>.Insert(0, 'y'),
                new Command<char>.Insert(0, 'z'),
                new Command<char>.Delete(1),
                new Command<char>.Delete(2),
                new Command<char>.Delete(3)
            ],
            ses);
    }

    [Fact]
    public void Test_SingleCharSame()
    {
        Assert.Empty(Algorithm.ComputeSes<char>("a", "a"));
    }

    [Fact]
    public void Test_SingleCharDifferent()
    {
        var ses = Algorithm.ComputeSes<char>("a", "b");

        Assert.Equal(
            [
                new Command<char>.Insert(0, 'b'),
                new Command<char>.Delete(1)
            ],
            ses);
    }

    [Fact]
    public void Test_Prefix()
    {
        var ses = Algorithm.ComputeSes<char>("abc", "abcd");

        Assert.Equal([new Command<char>.Insert(3, 'd')], ses);
    }

    [Fact]
    public void Test_Suffix()
    {
        var ses = Algorithm.ComputeSes<char>("abcd", "bcd");

        Assert.Equal([new Command<char>.Delete(1)], ses);
    }

    [Fact]
    public void Test_ExplicitComparer_Identical()
    {
        Assert.Empty(Algorithm.ComputeSes("abc", "ABC", ExplicitComparer.Instance));
    }

    [Fact]
    public void Test_ExplicitComparer_Mixed()
    {
        var ses = Algorithm.ComputeSes("abX", "ABY", ExplicitComparer.Instance);

        Assert.Equal(
            [
                new Command<char>.Insert(2, 'Y'),
                new Command<char>.Delete(3)
            ],
            ses);
    }
}
