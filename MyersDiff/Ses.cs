namespace MyersDiff;

/// <summary>
///  Shortest Edit Script
/// </summary>
/// <param name="b">Target sequence</param>
/// <typeparam name="T">Type of elements</typeparam>
public sealed class Ses<T>(IReadOnlyList<T> b) : Tracer
{
    public (int, Cmd, T)[] Script()
    {
        return Enumerate(N, M).Select(item => (item.X, item.C, item.C is Cmd.Ins ? b[item.Y - 1] : default!)).ToArray();
    }

    protected override IEnumerable<(int X, int Y, Cmd C)> EnumerateBackwords(int x, int y)
    {
        for (var i = Length - 1; i >= 0; i--)
        {
            switch (FindNearest(i, x, y))
            {
                case (true, false):
                    yield return (x, y, Cmd.Del);
                    x--;
                    break;
                case (false, true):
                    yield return (x, y, Cmd.Ins);
                    y--;
                    break;
                case (false, false):
                    i++;
                    x--;
                    y--;
                    break;
                default:
                    throw new InvalidOperationException("Unexpected state.");
            }
        }
    }
}