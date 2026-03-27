namespace MyersDiff;

/// <summary>
///  Shortest Edit Script
/// </summary>
/// <param name="b">Target sequence</param>
/// <typeparam name="T">Type of elements</typeparam>
public sealed class Ses<T>(IReadOnlyList<T> b, List<Vector> path) : Trace(path, new Configuration { ReturnDelete = true, ReturnInsert = true })
{
    public (int X, Cmd Cmd, T Ins)[] Build(int n, int m)
    {
        return Enumerate(n, m).Select(item => (item.X, item.Cmd, item.Cmd is Cmd.Ins ? b[item.Y - 1] : default!)).ToArray();
    }
}