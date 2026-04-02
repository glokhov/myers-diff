namespace MyersDiff;

/// <summary>
///  Represents a single step in the diff between two sequences.
/// </summary>
public abstract record Diff
{
    /// <summary>
    ///  An element common to both sequences.
    /// </summary>
    /// <param name="X">The 1-based position in the original sequence.</param>
    /// <param name="Y">The 1-based position in the modified sequence.</param>
    public sealed record Equal(int X, int Y) : Diff;

    /// <summary>
    ///  A deletion from the original sequence.
    /// </summary>
    /// <param name="X">The 1-based position in the original sequence.</param>
    public sealed record Delete(int X) : Diff;

    /// <summary>
    ///  An insertion from the modified sequence.
    /// </summary>
    /// <param name="X">The position in the original sequence after which this element is inserted (0 = before any element).</param>
    /// <param name="Y">The 1-based position in the modified sequence.</param>
    public sealed record Insert(int X, int Y) : Diff;
}