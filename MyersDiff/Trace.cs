namespace MyersDiff;

public sealed class Trace(Path path, Trace.Filter filter)
{
    public IEnumerable<(int X, int Y, Op Op)> Enumerate(int n, int m)
    {
        return EnumerateBackwards(n, m).Reverse();
    }

    private IEnumerable<(int X, int Y, Op Op)> EnumerateBackwards(int x, int y)
    {
        for (var i = path.Snapshots.Count - 1; i >= 0; i--)
        {
            switch (FindNearest(i, x, y))
            {
                case (true, false):
                    if (filter.HasFlag(Filter.Del)) yield return (x, y, Op.Del);
                    x--;
                    break;
                case (false, true):
                    if (filter.HasFlag(Filter.Ins)) yield return (x, y, Op.Ins);
                    y--;
                    break;
                case (false, false):
                    i++;
                    if (filter.HasFlag(Filter.Eq)) yield return (x, y, Op.Eq);
                    x--;
                    y--;
                    break;
                default:
                    throw new InvalidOperationException("Unexpected state.");
            }
        }
    }

    private (bool Left, bool Above) FindNearest(int i, int x, int y)
    {
        var v = path.Snapshots[i];
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

    [Flags]
    public enum Filter
    {
        Del = 1,
        Ins = 2,
        Eq = 4
    }

    public enum Op
    {
        Del,
        Ins,
        Eq
    }
}