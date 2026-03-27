namespace MyersDiff;

public abstract class Trace(List<Vector> path, Configuration config)
{
    protected IEnumerable<(int X, int Y, Cmd Cmd)> Enumerate(int n, int m)
    {
        return EnumerateBackwords(n, m).Reverse();
    }

    private IEnumerable<(int X, int Y, Cmd Cmd)> EnumerateBackwords(int x, int y)
    {
        for (var i = path.Count - 1; i >= 0; i--)
        {
            switch (FindNearest(i, x, y))
            {
                case (true, false):
                    if (config.ReturnDelete) yield return (x, y, Cmd.Del);
                    x--;
                    break;
                case (false, true):
                    if (config.ReturnInsert) yield return (x, y, Cmd.Ins);
                    y--;
                    break;
                case (false, false):
                    i++;
                    if (config.ReturnEqual) yield return (x, y, Cmd.Eq);
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
        var v = path[i];

        foreach (var point in v.Points.Reverse())
        {
            if (point == (x - 1, y))
            {
                return (true, false);
            }

            if (point == (x, y - 1))
            {
                return (false, true);
            }
        }

        return (false, false);
    }
}