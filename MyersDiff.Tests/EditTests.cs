namespace MyersDiff.Tests;

public class EditTests
{
    [Fact]
    public void Test_Delete()
    {
        Edit<char> command = new Edit<char>.Delete(1);

        switch (command)
        {
            case Edit<char>.Delete del:
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
        Edit<char> command = new Edit<char>.Insert(1, 'A');

        switch (command)
        {
            case Edit<char>.Insert ins:
                Assert.Equal(1, ins.Position);
                Assert.Equal('A', ins.Element);
                break;
            default:
                Assert.Fail("Unexpected command.");
                break;
        }
    }
}
