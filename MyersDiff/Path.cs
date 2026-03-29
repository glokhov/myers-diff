namespace MyersDiff;

/// <summary>
///  Records vector snapshots captured during the Myers algorithm forward pass.
/// </summary>
public sealed class Path
{
    private readonly List<Vector> _snapshots = [];

    /// <summary>
    ///  The vector snapshots, one per d-step.
    /// </summary>
    public IReadOnlyList<Vector> Snapshots => _snapshots;

    /// <summary>
    ///  Copies the current vector state for the given d-step and appends it to <see cref="Snapshots"/>.
    /// </summary>
    /// <param name="v">The vector to make a snapshot from.</param>
    /// <param name="d">The current d-step value.</param>
    public void MakeSnapshot(Vector v, int d)
    {
        _snapshots.Add(v.Copy(d));
    }
}