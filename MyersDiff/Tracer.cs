namespace MyersDiff;

public class Tracer
{
    protected readonly List<(int X, int Y)[]> Paths = [];

    public int Length => Paths.Count;

    public int N { get; set; }

    public int M { get; set; }

    public void AddPath(Vector v, int d)
    {
        Paths.Add(v.Points(d));
    }

    public (int X, int Y)[] Trace()
    {
        return Enumerate(N, M).Select(item => (item.X, item.Y)).ToArray();
    }

    protected IEnumerable<(int X, int Y, Cmd C)> Enumerate(int x, int y)
    {
        return EnumerateBackwords(x, y).Reverse();
    }

    protected virtual IEnumerable<(int X, int Y, Cmd C)> EnumerateBackwords(int x, int y)
    {
        for (var i = Length - 1; i >= 0; i--)
        {
            switch (FindNearest(i, x, y))
            {
                case (true, false):
                    x--;
                    break;
                case (false, true):
                    y--;
                    break;
                case (false, false):
                    i++;
                    yield return (x, y, Cmd.Eq);
                    x--;
                    y--;
                    break;
                default:
                    throw new InvalidOperationException("Unexpected state.");
            }
        }
    }

    protected (bool Left, bool Above) FindNearest(int i, int x, int y)
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