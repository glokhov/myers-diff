namespace MyersDiff.Tests;

public class CommandTests
{
    [Fact]
    public void Test_Delete()
    {
        Command<char> command = new Command<char>.Delete(1);

        switch (command)
        {
            case Command<char>.Delete del:
                Assert.Equal(1, del.Position);
                break;
            default:
                Assert.Fail("Unexpected command.");
                break;
        }
    }

    [Fact]
    public void Test_Insert()
    {
        Command<char> command = new Command<char>.Insert(1, 'A');

        switch (command)
        {
            case Command<char>.Insert ins:
                Assert.Equal(1, ins.Position);
                Assert.Equal('A', ins.Element);
                break;
            default:
                Assert.Fail("Unexpected command.");
                break;
        }
    }
}