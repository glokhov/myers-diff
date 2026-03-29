using System.Runtime.InteropServices;

namespace MyersDiff;

/// <summary>
///  Shortest Edit Script — string convenience overloads.
/// </summary>
public static class Ses
{
    /// <summary>
    ///  Builds the shortest edit script between two strings using the default character comparer.
    /// </summary>
    /// <param name="a">The original string.</param>
    /// <param name="b">The modified string.</param>
    /// <returns>A span of edit commands that transform <paramref name="a"/> into <paramref name="b"/>.</returns>
    public static ReadOnlySpan<Cmd> Build(string a, string b)
    {
        return Build(a, b, EqualityComparer<char>.Default);
    }

    /// <summary>
    ///  Builds the shortest edit script between two strings using an explicit character comparer.
    /// </summary>
    /// <param name="a">The original string.</param>
    /// <param name="b">The modified string.</param>
    /// <param name="comparer">The equality comparer used to compare characters.</param>
    /// <returns>A span of edit commands that transform <paramref name="a"/> into <paramref name="b"/>.</returns>
    public static ReadOnlySpan<Cmd> Build(string a, string b, IEqualityComparer<char> comparer)
    {
        return Ses<char>.Build(a, b, comparer, CmdDel, CmdIns);
    }

    private static Cmd CmdDel(int x) => new Cmd.Del(x);

    private static Cmd CmdIns(int x, char c) => new Cmd.Ins(x, c);

    /// <summary>
    ///  Represents an edit command in the shortest edit script.
    /// </summary>
    public abstract record Cmd
    {
        /// <summary>
        ///  A command to delete a character at the specified position.
        /// </summary>
        /// <param name="Pos">The 1-based position in the original string.</param>
        public sealed record Del(int Pos) : Cmd;

        /// <summary>
        ///  A command to insert a character at the specified position.
        /// </summary>
        /// <param name="Pos">The 1-based position in the original string after which to insert.</param>
        /// <param name="Item">The character to insert.</param>
        public sealed record Ins(int Pos, char Item) : Cmd;
    }
}

/// <summary>
///  Shortest Edit Script — generic overloads.
/// </summary>
/// <typeparam name="T">The type of elements in the sequences.</typeparam>
public static class Ses<T> where T : IEquatable<T>
{
    /// <summary>
    ///  Builds the shortest edit script between two sequences using the default equality comparer.
    /// </summary>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <returns>A span of edit commands that transform <paramref name="a"/> into <paramref name="b"/>.</returns>
    public static ReadOnlySpan<Cmd> Build(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
    {
        return Build(a, b, EqualityComparer<T>.Default);
    }

    /// <summary>
    ///  Builds the shortest edit script between two sequences using an explicit equality comparer.
    /// </summary>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <param name="comparer">The equality comparer used to compare elements.</param>
    /// <returns>A span of edit commands that transform <paramref name="a"/> into <paramref name="b"/>.</returns>
    public static ReadOnlySpan<Cmd> Build(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer)
    {
        return Build(a, b, comparer, CmdDel, CmdIns);
    }

    internal static ReadOnlySpan<TCmd> Build<TCmd>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer, Func<int, TCmd> del, Func<int, T, TCmd> ins)
    {
        var list = new List<TCmd>();

        var path = Algorithm.LcsSes(a, b, comparer);

        var trace = new Trace(path, Trace.Filter.Del | Trace.Filter.Ins);

        foreach (var edit in trace.EnumerateEdits(a.Length, b.Length))
        {
            switch (edit.Op)
            {
                case Trace.Op.Del:
                    list.Add(del(edit.X));
                    break;
                case Trace.Op.Ins:
                    list.Add(ins(edit.X, b[edit.Y - 1]));
                    break;
                case Trace.Op.Eq:
                    break;
                default:
                    throw new InvalidOperationException("Unexpected operation.");
            }
        }

        return CollectionsMarshal.AsSpan(list);
    }

    private static Cmd CmdDel(int x) => new Cmd.Del(x);

    private static Cmd CmdIns(int x, T i) => new Cmd.Ins(x, i);

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