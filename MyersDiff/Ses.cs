namespace MyersDiff;

/// <summary>
///  Shortest Edit Script
/// </summary>
/// <param name="b">Target sequence</param>
/// <typeparam name="T">Type of elements</typeparam>
public sealed class Ses<T>(IReadOnlyList<T> b, Path path) : Trace(path, Config.Ses)
{
    public Cmd[] Build(int n, int m)
    {
        return Enumerate(n, m).Select(Convert).ToArray();
    }

    private Cmd Convert((int X, int Y, Op Op) item)
    {
        return item.Op switch
        {
            Op.Del => new Cmd.Del(item.X),
            Op.Ins => new Cmd.Ins(item.X, b[item.Y - 1]),
            Op.Eq => throw new InvalidOperationException("Unexpected command."),
            _ => throw new InvalidOperationException("Unexpected command.")
        };
    }

    public abstract record Cmd
    {
        public sealed record Del(int Pos) : Cmd;

        public sealed record Ins(int Pos, T Item) : Cmd;
    }
}