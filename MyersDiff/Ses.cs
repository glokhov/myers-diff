namespace MyersDiff;

/// <summary>
///  Shortest Edit Script.
/// </summary>
/// <typeparam name="T">The type of elements in the sequences.</typeparam>
public static class Ses<T>
{
    private const Trace.Operation Operation = Trace.Operation.Delete | Trace.Operation.Insert;

    /// <summary>
    ///  Builds the shortest edit script between two sequences using the default equality comparer.
    /// </summary>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <returns>A list of edit commands that transform <paramref name="a"/> into <paramref name="b"/>.</returns>
    public static List<Command> Build(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
    {
        return Build(a, b, EqualityComparer<T>.Default);
    }

    /// <summary>
    ///  Builds the shortest edit script between two sequences using an explicit equality comparer.
    /// </summary>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <param name="comparer">The equality comparer used to compare elements.</param>
    /// <returns>A list of edit commands that transform <paramref name="a"/> into <paramref name="b"/>.</returns>
    public static List<Command> Build(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer)
    {
        ArgumentNullException.ThrowIfNull(comparer);

        var list = new List<Command>();

        var path = Algorithm.LcsSes(a, b, comparer);

        foreach (var edit in Trace.GetEdits(path, Operation))
        {
            switch (edit.Operation)
            {
                case Trace.Operation.Delete:
                    list.Add(new Command.Delete(edit.X));
                    break;
                case Trace.Operation.Insert:
                    list.Add(new Command.Insert(edit.X, b[edit.Y - 1]));
                    break;
                case Trace.Operation.Equal:
                    break;
                default:
                    throw new InvalidOperationException("Unexpected operation.");
            }
        }

        return list;
    }

    /// <summary>
    ///  Represents an edit command in the shortest edit script.
    /// </summary>
    public abstract record Command
    {
        /// <summary>
        ///  A command to delete an element at the specified position.
        /// </summary>
        /// <param name="Position">The 1-based position in the original sequence.</param>
        public sealed record Delete(int Position) : Command;

        /// <summary>
        ///  A command to insert an element at the specified position.
        /// </summary>
        /// <param name="Position">The 1-based position in the original sequence after which to insert.</param>
        /// <param name="Element">The element to insert.</param>
        public sealed record Insert(int Position, T Element) : Command;
    }
}