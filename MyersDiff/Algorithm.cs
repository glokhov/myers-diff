namespace MyersDiff;

public static class Algorithm
{
    public static void LcsSes<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, EqualityComparer<T> comparer, List<Vector> path)
    {
        var n = a.Length;
        var m = b.Length;

        var max = n + m;

        var v = new Vector(max);

        for (var d = 0; d <= max; d++)
        {
            for (var k = -d; k <= d; k += 2)
            {
                int x;

                if (k == -d || (k != d && v[k - 1] < v[k + 1]))
                {
                    x = v[k + 1];
                }
                else
                {
                    x = v[k - 1] + 1;
                }

                var y = x - k;

                while (x < n && y < m && comparer.Equals(a[x], b[y]))
                {
                    x++;
                    y++;
                }

                v[k] = x;

                if (x >= n && y >= m)
                {
                    return;
                }
            }

            path.Add(v.Copy(d));
        }

        throw new InvalidOperationException("Unexpected end of algorithm.");
    }
}