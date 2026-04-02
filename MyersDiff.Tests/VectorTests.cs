namespace MyersDiff.Tests;

public sealed class VectorTests
{
    [Fact]
    public void Test_Vector()
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var vector = new Vector(1);

        vector[-1] = 1;
        vector[0] = 2;
        vector[1] = 3;

        Assert.Equal(1, vector[-1]);
        Assert.Equal(2, vector[0]);
        Assert.Equal(3, vector[1]);
    }
}