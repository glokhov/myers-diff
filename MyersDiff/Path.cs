namespace MyersDiff;

/// <summary>
///  Records vector snapshots captured during the Myers algorithm forward pass.
/// </summary>
public sealed class Path
{
    /// <summary>
    ///  The list of vector snapshots, one per d-step.
    /// </summary>
    public List<Vector> Snapshots { get; } = [];

    /// <summary>
    ///  Copies the current vector state for the given d-step and appends it to <see cref="Snapshots"/>.
    /// </summary>
    /// <param name="v">The vector to make a snapshot from.</param>
    /// <param name="d">The current d-step value.</param>
    public void MakeSnapshot(Vector v, int d)
    {
        Snapshots.Add(v.Copy(d));
    }
}