namespace MyersDiff;

/// <summary>
///  Backtracks through vector snapshots to reconstruct the edit graph path,
///  yielding <see cref="Edit"/> records for each step.
/// </summary>
/// <param name="path">The path containing vector snapshots.</param>
/// <param name="filter">The filter controlling which operations are yielded.</param>
public sealed class Trace(Path path, Trace.Filter filter)
{
    /// <summary>
    ///  Enumerates the edit operations in forward order.
    /// </summary>
    /// <param name="n">The length of the original sequence.</param>
    /// <param name="m">The length of the modified sequence.</param>
    /// <returns>An enumerable of <see cref="Edit"/> records describing each edit step.</returns>
    public IEnumerable<Edit> Enumerate(int n, int m)
    {
        var stack = new Stack<Edit>();

        var x = n;
        var y = m;

        for (var i = path.Snapshots.Count - 1; i >= 0; i--)
        {
            switch (FindNearest(i, x, y))
            {
                case (true, false):
                    if (filter.HasFlag(Filter.Del)) stack.Push(new Edit(x, y, Op.Del));
                    x--;
                    break;
                case (false, true):
                    if (filter.HasFlag(Filter.Ins)) stack.Push(new Edit(x, y, Op.Ins));
                    y--;
                    break;
                case (false, false):
                    i++;
                    if (filter.HasFlag(Filter.Eq)) stack.Push(new Edit(x, y, Op.Eq));
                    x--;
                    y--;
                    break;
                default:
                    throw new InvalidOperationException("Unexpected state.");
            }
        }

        // Emit any remaining diagonal (equal) moves from the leading snake
        // that preceded the first snapshotted d-step.
        while (x > 0 && y > 0)
        {
            if (filter.HasFlag(Filter.Eq)) stack.Push(new Edit(x, y, Op.Eq));
            x--;
            y--;
        }

        return stack;
    }

    private (bool Left, bool Above) FindNearest(int i, int x, int y)
    {
        var v = path.Snapshots[i];
        var k = x - y;

        if (v.HasDiagonal(k + 1) && v[k + 1] == x)
        {
            return (false, true);
        }

        if (v.HasDiagonal(k - 1) && v[k - 1] == x - 1)
        {
            return (true, false);
        }

        return (false, false);
    }

    /// <summary>
    ///  Represents a single edit in the edit sequence.
    /// </summary>
    /// <param name="X">The position in the original sequence (1-based for operations).</param>
    /// <param name="Y">The position in the modified sequence (1-based for operations).</param>
    /// <param name="Op">The type of operation: delete, insert, or equal.</param>
    public readonly record struct Edit(int X, int Y, Op Op);

    /// <summary>
    ///  Represents the type of an edit operation.
    /// </summary>
    public enum Op
    {
        /// <summary>A deletion from the original sequence.</summary>
        Del,

        /// <summary>An insertion from the modified sequence.</summary>
        Ins,

        /// <summary>An element common to both sequences.</summary>
        Eq
    }

    /// <summary>
    ///  Specifies which edit operations to include when enumerating the trace.
    /// </summary>
    [Flags]
    public enum Filter
    {
        /// <summary>Includes delete operations.</summary>
        Del = 1,

        /// <summary>Includes insert operations.</summary>
        Ins = 2,

        /// <summary>Includes equal (diagonal) operations.</summary>
        Eq = 4
    }
}