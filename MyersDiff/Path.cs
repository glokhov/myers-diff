namespace MyersDiff;

/// <summary>
///  Records vector snapshots captured during the Myers algorithm forward pass.
/// </summary>
/// <param name="n">The length of the original sequence.</param>
/// <param name="m">The length of the modified sequence.</param>
public sealed class Path(int n, int m)
{
    private readonly List<Vector> _snapshots = [];

    /// <summary>
    ///  The length of the original sequence.
    /// </summary>
    public int N { get; } = n;

    /// <summary>
    ///  The length of the modified sequence.
    /// </summary>
    public int M { get; } = m;

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