namespace MyersDiff;

public class Trace(Path path, Config config)
{
    public IEnumerable<(int X, int Y, Op Op)> Enumerate(int n, int m)
    {
        return EnumerateBackwords(n, m).Reverse();
    }

    private IEnumerable<(int X, int Y, Op Op)> EnumerateBackwords(int x, int y)
    {
        for (var i = path.Paths.Count - 1; i >= 0; i--)
        {
            switch (FindNearest(i, x, y))
            {
                case (true, false):
                    if (config.UseDelete) yield return (x, y, Op.Del);
                    x--;
                    break;
                case (false, true):
                    if (config.UseInsert) yield return (x, y, Op.Ins);
                    y--;
                    break;
                case (false, false):
                    i++;
                    if (config.UseEqual) yield return (x, y, Op.Eq);
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
        var v = path.Paths[i];

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

    public enum Op
    {
        Del,
        Ins,
        Eq
    }
}