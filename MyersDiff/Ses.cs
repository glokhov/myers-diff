namespace MyersDiff;

/// <summary>
///  Shortest Edit Script
/// </summary>
/// <param name="b">Target sequence</param>
/// <typeparam name="T">Type of elements</typeparam>
public sealed class Ses<T>(IReadOnlyList<T> b) : Tracer
{
    protected override bool ReturnDelete => true;

    protected override bool ReturnInsert => true;

    protected override bool ReturnEqual => false;

    public (int X, Cmd Cmd, T Ins)[] Build()
    {
        return Enumerate(N, M).Select(item => (item.X, item.Cmd, item.Cmd is Cmd.Ins ? b[item.Y - 1] : default!)).ToArray();
    }
}