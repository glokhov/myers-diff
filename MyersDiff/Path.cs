namespace MyersDiff;

public sealed class Path
{
    public List<Vector> Paths { get; } = [];

    public void OnPathFound(Vector v, int d)
    {
        Paths.Add(v.Clone(d));
    }
}