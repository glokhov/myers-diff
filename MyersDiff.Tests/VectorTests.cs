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
    public void Test_Points_Zero()
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var vector = new Vector(3);

        vector[-1] = 1;
        vector[0] = 2;
        vector[1] = 3;

        var points = vector.Clone(0).Points.ToArray();

        Assert.Equal((2, 2), points[0]);
    }

    [Fact]
    public void Test_Points_Odd()
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

        var points = vector.Clone(1).Points.ToArray();

        Assert.Equal((3, 4), points[0]);
        Assert.Equal((5, 4), points[1]);
    }

    [Fact]
    public void Test_Points_Even()
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

        var points = vector.Clone(2).Points.ToArray();

        Assert.Equal((2, 4), points[0]);
        Assert.Equal((4, 4), points[1]);
        Assert.Equal((6, 4), points[2]);
    }
}