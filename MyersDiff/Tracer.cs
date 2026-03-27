namespace MyersDiff;

public abstract class Tracer
{
    public static Tracer Default => new InternalTracer();

    public List<(int X, int Y)[]> Paths { get; } = [];

    public int N { get; set; }

    public int M { get; set; }

    protected abstract bool ReturnDelete { get; }

    protected abstract bool ReturnInsert { get; }

    protected abstract bool ReturnEqual { get; }

    public void AddPath(Vector v, int d)
    {
        Paths.Add(v.Points(d));
    }

    public (int X, int Y)[] Trace()
    {
        return Enumerate(N, M).Select(item => (item.X, item.Y)).ToArray();
    }

    protected IEnumerable<(int X, int Y, Cmd Cmd)> Enumerate(int x, int y)
    {
        return EnumerateBackwords(x, y).Reverse();
    }

    private IEnumerable<(int X, int Y, Cmd Cmd)> EnumerateBackwords(int x, int y)
    {
        for (var i = Paths.Count - 1; i >= 0; i--)
        {
            switch (FindNearest(i, x, y))
            {
                case (true, false):
                    if (ReturnDelete) yield return (x, y, Cmd.Del);
                    x--;
                    break;
                case (false, true):
                    if (ReturnInsert) yield return (x, y, Cmd.Ins);
                    y--;
                    break;
                case (false, false):
                    i++;
                    if (ReturnEqual) yield return (x, y, Cmd.Eq);
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
        for (var j = Paths[i].Length - 1; j >= 0; j--)
        {
            var point = Paths[i][j];

            if (point == Left())
            {
                return (true, false);
            }

            if (point == Above())
            {
                return (false, true);
            }
        }

        return (false, false);

        (int X, int Y) Left() => (x - 1, y);

        (int X, int Y) Above() => (x, y - 1);
    }
}

internal sealed class InternalTracer : Tracer
{
    protected override bool ReturnDelete => false;

    protected override bool ReturnInsert => false;

    protected override bool ReturnEqual => true;
}