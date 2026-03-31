namespace MyersDiff;

/// <summary>
///  Backtracks through vector snapshots to reconstruct the edit graph path,
///  returning <see cref="Edit"/> records for each step.
/// </summary>
public static class Trace
{
    /// <summary>
    ///  Returns the edit operations in forward order.
    /// </summary>
    /// <param name="path">The path containing vector snapshots.</param>
    /// <param name="operation">The filter controlling which operations are included.</param>
    /// <returns>A stack of <see cref="Edit"/> records describing each edit step.</returns>
    public static Stack<Edit> GetEdits(Path path, Operation operation)
    {
        var stack = new Stack<Edit>();

        var x = path.N;
        var y = path.M;

        for (var i = path.Length - 1; i >= 0; i--)
        {
            switch (FindNearest(path[i], x, y))
            {
                case (true, false):
                    if (operation.HasFlag(Operation.Delete)) stack.Push(new Edit(x, y, Operation.Delete));
                    x--;
                    break;
                case (false, true):
                    if (operation.HasFlag(Operation.Insert)) stack.Push(new Edit(x, y, Operation.Insert));
                    y--;
                    break;
                case (false, false):
                    i++;
                    if (operation.HasFlag(Operation.Equal)) stack.Push(new Edit(x, y, Operation.Equal));
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
            if (operation.HasFlag(Operation.Equal)) stack.Push(new Edit(x, y, Operation.Equal));
            x--;
            y--;
        }

        return stack;
    }

    private static (bool Left, bool Above) FindNearest(Vector v, int x, int y)
    {
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
    ///  Represents a single step in the edit sequence.
    /// </summary>
    /// <param name="X">The 1-based position in the original sequence.</param>
    /// <param name="Y">The 1-based position in the modified sequence.</param>
    /// <param name="Operation">The type of operation: delete, insert, or equal.</param>
    public readonly record struct Edit(int X, int Y, Operation Operation);

    /// <summary>
    ///  Represents the type of an edit operation.
    /// </summary>
    [Flags]
    public enum Operation
    {
        /// <summary>A deletion from the original sequence.</summary>
        Delete = 1,

        /// <summary>An insertion of an element from the modified sequence.</summary>
        Insert = 2,

        /// <summary>An element common to both sequences.</summary>
        Equal = 4
    }
}