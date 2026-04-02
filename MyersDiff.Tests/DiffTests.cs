namespace MyersDiff.Tests;

public sealed class DiffTests
{
    [Fact]
    public void PatternMatch_Equal()
    {
        Diff diff = new Diff.Equal(1, 2);

        switch (diff)
        {
            case Diff.Equal eq:
                Assert.Equal(1, eq.X);
                Assert.Equal(2, eq.Y);
                break;
            default:
                Assert.Fail("Unexpected diff.");
                break;
        }
    }

    [Fact]
    public void PatternMatch_Delete()
    {
        Diff diff = new Diff.Delete(4);

        switch (diff)
        {
            case Diff.Delete del:
                Assert.Equal(4, del.X);
                break;
            default:
                Assert.Fail("Unexpected diff.");
                break;
        }
    }

    [Fact]
    public void PatternMatch_Insert()
    {
        Diff diff = new Diff.Insert(6);

        switch (diff)
        {
            case Diff.Insert ins:
                Assert.Equal(6, ins.Y);
                break;
            default:
                Assert.Fail("Unexpected diff.");
                break;
        }
    }
}