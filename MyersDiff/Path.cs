namespace MyersDiff;

public sealed class Path
{
    public List<Vector> Snapshots { get; } = [];

    public void MakeSnapshot(Vector v, int d)
    {
        Snapshots.Add(v.Copy(d));
    }
}