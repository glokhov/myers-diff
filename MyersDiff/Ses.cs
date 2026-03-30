namespace MyersDiff;

/// <summary>
///  Shortest Edit Script — generic overloads.
/// </summary>
/// <typeparam name="T">The type of elements in the sequences.</typeparam>
public static class Ses<T>
{
    private const Trace.Filter Filter = Trace.Filter.Del | Trace.Filter.Ins;

    /// <summary>
    ///  Builds the shortest edit script between two sequences using the default equality comparer.
    /// </summary>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <returns>A list of edit commands that transform <paramref name="a"/> into <paramref name="b"/>.</returns>
    public static List<Cmd> Build(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
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
    public static List<Cmd> Build(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer)
    {
        ArgumentNullException.ThrowIfNull(comparer);

        var list = new List<Cmd>();

        var path = Algorithm.LcsSes(a, b, comparer);

        foreach (var edit in Trace.EnumerateEdits(path, Filter))
        {
            switch (edit.Op)
            {
                case Trace.Op.Del:
                    list.Add(new Cmd.Del(edit.X));
                    break;
                case Trace.Op.Ins:
                    list.Add(new Cmd.Ins(edit.X, b[edit.Y - 1]));
                    break;
                case Trace.Op.Eq:
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
    public abstract record Cmd
    {
        /// <summary>
        ///  A command to delete an element at the specified position.
        /// </summary>
        /// <param name="Pos">The 1-based position in the original sequence.</param>
        public sealed record Del(int Pos) : Cmd;

        /// <summary>
        ///  A command to insert an element at the specified position.
        /// </summary>
        /// <param name="Pos">The 1-based position in the original sequence after which to insert.</param>
        /// <param name="Item">The element to insert.</param>
        public sealed record Ins(int Pos, T Item) : Cmd;
    }
}