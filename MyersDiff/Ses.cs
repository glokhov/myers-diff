namespace MyersDiff;

/// <summary>
///  Shortest Edit Script
/// </summary>
public static class Ses
{
    public static Cmd[] Build(string a, string b)
    {
        return Build(a, b, EqualityComparer<char>.Default);
    }

    public static Cmd[] Build(string a, string b, EqualityComparer<char> comparer)
    {
        return Ses<char>.Build(a, b, comparer, CmdDel, CmdIns);
    }

    private static Cmd CmdDel(int x) => new Cmd.Del(x);

    private static Cmd CmdIns(int x, char c) => new Cmd.Ins(x, c);

    public abstract record Cmd
    {
        public sealed record Del(int Pos) : Cmd;

        public sealed record Ins(int Pos, char Char) : Cmd;
    }
}

/// <summary>
///  Shortest Edit Script
/// </summary>
public static class Ses<T>
{
    public static Cmd[] Build(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
    {
        return Build(a, b, EqualityComparer<T>.Default);
    }

    public static Cmd[] Build(ReadOnlySpan<T> a, ReadOnlySpan<T> b, EqualityComparer<T> comparer)
    {
        return Build(a, b, comparer, CmdDel, CmdIns);
    }

    internal static TCmd[] Build<TCmd>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, EqualityComparer<T> comparer, Func<int, TCmd> del, Func<int, T, TCmd> ins)
    {
        var list = new List<TCmd>();
        var path = new Path();

        Algorithm.LcsSes(a, b, comparer, path);

        var trace = new Trace(path, Trace.Filter.Del | Trace.Filter.Ins);

        foreach (var item in trace.Enumerate(a.Length, b.Length))
        {
            switch (item.Op)
            {
                case Trace.Op.Del:
                    list.Add(del(item.X));
                    break;
                case Trace.Op.Ins:
                    list.Add(ins(item.X, b[item.Y - 1]));
                    break;
                case Trace.Op.Eq:
                    break;
                default:
                    throw new InvalidOperationException("Unexpected operation.");
            }
        }

        return list.ToArray();
    }

    private static Cmd CmdDel(int x) => new Cmd.Del(x);

    private static Cmd CmdIns(int x, T i) => new Cmd.Ins(x, i);

    public abstract record Cmd
    {
        public sealed record Del(int Pos) : Cmd;

        public sealed record Ins(int Pos, T Item) : Cmd;
    }
}