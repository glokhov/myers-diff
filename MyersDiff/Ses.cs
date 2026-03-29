namespace MyersDiff;

/// <summary>
///  Shortest Edit Script
/// </summary>
public static class Ses
{
    public static Ses<char>.Cmd[] Build(string a, string b)
    {
        return Ses<char>.Build(a, b);
    }
}

/// <summary>
///  Shortest Edit Script
/// </summary>
public static class Ses<T>
{
    public static Cmd[] Build(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
    {
        var list = new List<Cmd>();
        var path = new Path();

        Algorithm.LcsSes(a, b, EqualityComparer<T>.Default, path);

        var trace = new Trace(path, Trace.Filter.Del | Trace.Filter.Ins);

        foreach (var item in trace.Enumerate(a.Length, b.Length))
        {
            switch (item.Op)
            {
                case Trace.Op.Del:
                    list.Add(new Cmd.Del(item.X));
                    break;
                case Trace.Op.Ins:
                    list.Add(new Cmd.Ins(item.X, b[item.Y - 1]));
                    break;
                case Trace.Op.Eq:
                default:
                    throw new InvalidOperationException("Unexpected command.");
            }
        }

        return list.ToArray();
    }

    public abstract record Cmd
    {
        public sealed record Del(int Pos) : Cmd;

        public sealed record Ins(int Pos, T Item) : Cmd;
    }
}