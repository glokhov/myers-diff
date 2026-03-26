namespace MyersDiff;

public abstract class Trace
{
    protected readonly List<(int X, int Y)[]> Paths = [];
    
    public int Length => Paths.Count;

    public void AddPath(Vector v, int d)
    {
        Paths.Add(v.Points(d));
    }
}