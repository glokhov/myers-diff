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

    [Fact]
    public void Test_Copy_Zero()
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var vector = new Vector(3);

        vector[-1] = 1;
        vector[0] = 2;
        vector[1] = 3;

        var copy = vector.Copy(0);

        Assert.Equal(2, copy[0]);
    }

    [Fact]
    public void Test_Copy_Odd()
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var vector = new Vector(3);

        vector[-3] = 1;
        vector[-2] = 2;
        vector[-1] = 3;
        vector[0] = 4;
        vector[1] = 5;
        vector[2] = 6;
        vector[3] = 7;

        var copy = vector.Copy(1);

        Assert.Equal(3, copy[-1]);
        Assert.Equal(5, copy[1]);
    }

    [Fact]
    public void Test_Copy_Even()
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var vector = new Vector(3);

        vector[-3] = 1;
        vector[-2] = 2;
        vector[-1] = 3;
        vector[0] = 4;
        vector[1] = 5;
        vector[2] = 6;
        vector[3] = 7;

        var copy = vector.Copy(2);

        Assert.Equal(2, copy[-2]);
        Assert.Equal(4, copy[0]);
        Assert.Equal(6, copy[2]);
    }
}