namespace MyersDiff;

/// <summary>
///  Backtracks through vector snapshots to reconstruct the edit graph path,
///  yielding <c>(x, y, Op)</c> tuples for each step.
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
    /// <returns>An enumerable of <c>(X, Y, Op)</c> tuples describing each edit step.</returns>
    public IEnumerable<(int X, int Y, Op Op)> Enumerate(int n, int m)
    {
        var stack = new Stack<(int X, int Y, Op Op)>();

        var x = n;
        var y = m;

        for (var i = path.Snapshots.Count - 1; i >= 0; i--)
        {
            switch (FindNearest(i, x, y))
            {
                case (true, false):
                    if (filter.HasFlag(Filter.Del)) stack.Push((x, y, Op.Del));
                    x--;
                    break;
                case (false, true):
                    if (filter.HasFlag(Filter.Ins)) stack.Push((x, y, Op.Ins));
                    y--;
                    break;
                case (false, false):
                    i++;
                    if (filter.HasFlag(Filter.Eq)) stack.Push((x, y, Op.Eq));
                    x--;
                    y--;
                    break;
                default:
                    throw new InvalidOperationException("Unexpected state.");
            }
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
}