namespace MyersDiff;

public abstract class Trace
{
    protected readonly List<(int X, int Y)[]> Paths = [];

    public int Length => Paths.Count;

    public void AddPath(Vector v, int d)
    {
        Paths.Add(v.Points(d));
    }

    public virtual List<(int X, int Y)> GetBackTrace((int X, int Y) endpoint)
    {
        var trace = new List<(int, int)>();

        // todo: implement

        return trace;

        (int X, int Y) Diagonal() => (endpoint.X - 1, endpoint.Y - 1);

        (int X, int Y) Left() => (endpoint.X - 1, endpoint.Y);

        (int X, int Y) Above() => (endpoint.X, endpoint.Y - 1);
    }
}