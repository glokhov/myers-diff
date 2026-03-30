namespace MyersDiff.Tests;

public sealed class SesTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";

    [Fact]
    public void Test_Build()
    {
        var ses = Ses<char>.Build(A, B);

        var a = new Ses<char>.Cmd.Del(1);
        var b = new Ses<char>.Cmd.Del(2);
        var c = new Ses<char>.Cmd.Ins(3, 'b');
        var d = new Ses<char>.Cmd.Del(6);
        var e = new Ses<char>.Cmd.Ins(7, 'c');

        Assert.Equal([a, b, c, d, e], ses);
    }

    [Fact]
    public void Test_Build_Comparer()
    {
        var ses = Ses<char>.Build(A, B, EqualityComparer<char>.Default);

        var a = new Ses<char>.Cmd.Del(1);
        var b = new Ses<char>.Cmd.Del(2);
        var c = new Ses<char>.Cmd.Ins(3, 'b');
        var d = new Ses<char>.Cmd.Del(6);
        var e = new Ses<char>.Cmd.Ins(7, 'c');

        Assert.Equal([a, b, c, d, e], ses);
    }

    [Fact]
    public void Test_Cmd_Del()
    {
        Ses<char>.Cmd cmd = new Ses<char>.Cmd.Del(1);

        switch (cmd)
        {
            case Ses<char>.Cmd.Del del:
                Assert.Equal(1, del.Pos);
                break;
            default:
                Assert.Fail("Unexpected command.");
                break;
        }
    }

    [Fact]
    public void Test_Cmd_Ins()
    {
        Ses<char>.Cmd cmd = new Ses<char>.Cmd.Ins(1, 'A');

        switch (cmd)
        {
            case Ses<char>.Cmd.Ins ins:
                Assert.Equal(1, ins.Pos);
                Assert.Equal('A', ins.Item);
                break;
            default:
                Assert.Fail("Unexpected command.");
                break;
        }
    }

    [Fact]
    public void Test_Build_BothEmpty()
    {
        Assert.Empty(Ses<char>.Build("", ""));
    }

    [Fact]
    public void Test_Build_FirstEmpty()
    {
        var ses = Ses<char>.Build("", "abc");

        Assert.Equal(
            [
                new Ses<char>.Cmd.Ins(0, 'a'),
                new Ses<char>.Cmd.Ins(0, 'b'),
                new Ses<char>.Cmd.Ins(0, 'c')
            ],
            ses);
    }

    [Fact]
    public void Test_Build_SecondEmpty()
    {
        var ses = Ses<char>.Build("abc", "");

        Assert.Equal(
            [
                new Ses<char>.Cmd.Del(1),
                new Ses<char>.Cmd.Del(2),
                new Ses<char>.Cmd.Del(3)
            ],
            ses);
    }

    [Fact]
    public void Test_Build_Identical()
    {
        Assert.Empty(Ses<char>.Build("abc", "abc"));
    }

    [Fact]
    public void Test_Build_Disjoint()
    {
        var ses = Ses<char>.Build("abc", "xyz");

        Assert.Equal(
            [
                new Ses<char>.Cmd.Del(1),
                new Ses<char>.Cmd.Del(2),
                new Ses<char>.Cmd.Del(3),
                new Ses<char>.Cmd.Ins(3, 'x'),
                new Ses<char>.Cmd.Ins(3, 'y'),
                new Ses<char>.Cmd.Ins(3, 'z')
            ],
            ses);
    }

    [Fact]
    public void Test_Build_SingleCharSame()
    {
        Assert.Empty(Ses<char>.Build("a", "a"));
    }

    [Fact]
    public void Test_Build_SingleCharDifferent()
    {
        var ses = Ses<char>.Build("a", "b");

        Assert.Equal(
            [
                new Ses<char>.Cmd.Del(1),
                new Ses<char>.Cmd.Ins(1, 'b')
            ],
            ses);
    }

    [Fact]
    public void Test_Build_Prefix()
    {
        var ses = Ses<char>.Build("abc", "abcd");

        Assert.Equal([new Ses<char>.Cmd.Ins(3, 'd')], ses);
    }

    [Fact]
    public void Test_Build_Suffix()
    {
        var ses = Ses<char>.Build("abcd", "bcd");

        Assert.Equal([new Ses<char>.Cmd.Del(1)], ses);
    }

    [Fact]
    public void Test_Build_ExplicitComparer_Identical()
    {
        Assert.Empty(Ses<char>.Build("abc", "ABC", ExplicitComparer.Instance));
    }

    [Fact]
    public void Test_Build_ExplicitComparer_Mixed()
    {
        var ses = Ses<char>.Build("abX", "ABY", ExplicitComparer.Instance);

        Assert.Equal(
            [
                new Ses<char>.Cmd.Del(3),
                new Ses<char>.Cmd.Ins(3, 'Y')
            ],
            ses);
    }
}