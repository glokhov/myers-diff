namespace MyersDiff.Tests;

public sealed class SesTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";

    [Fact]
    public void Test_Build_String()
    {
        var ses = Ses.Build(A, B);

        var a = new Ses.Cmd.Del(1);
        var b = new Ses.Cmd.Del(2);
        var c = new Ses.Cmd.Ins(3, 'b');
        var d = new Ses.Cmd.Del(6);
        var e = new Ses.Cmd.Ins(7, 'c');

        Assert.Equal([a, b, c, d, e], ses);
    }

    [Fact]
    public void Test_Cmd_Del_String()
    {
        Ses.Cmd cmd = new Ses.Cmd.Del(1);

        switch (cmd)
        {
            case Ses.Cmd.Del del:
                Assert.Equal(1, del.Pos);
                break;
            default:
                Assert.Fail("Unexpected command.");
                break;
        }
    }

    [Fact]
    public void Test_Cmd_Ins_String()
    {
        Ses.Cmd cmd = new Ses.Cmd.Ins(1, 'A');

        switch (cmd)
        {
            case Ses.Cmd.Ins ins:
                Assert.Equal(1, ins.Pos);
                Assert.Equal('A', ins.Char);
                break;
            default:
                Assert.Fail("Unexpected command.");
                break;
        }
    }

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
}