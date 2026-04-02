using System.Diagnostics;

namespace MyersDiff;

/// <summary>
///  Core implementation of the Myers O(ND) difference algorithm.
/// </summary>
public static partial class Algorithm
{
    internal static (int D, int X, int Y, int U, int V) FindMiddleSnake<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer)
    {
        var n = a.Length;
        var m = b.Length;

        var max = (n + m + 1) / 2;
        var delta = n - m;

        var even = delta % 2 == 0;
        var odd = !even;

        var v = new Vector(n + m);
        var w = new Vector(n + m);

        v[1] = 0;
        w[delta + 1] = n + 1;

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

                var s = x;

                while (x < n && y < m && comparer.Equals(a[x], b[y]))
                {
                    x++;
                    y++;
                }

                v[k] = x;

                if (odd && k >= delta - (d - 1) && k <= delta + (d - 1) && v[k] >= w[k])
                {
                    return (d * 2 - 1, s, s - k, x, y);
                }
            }

            for (var k = -d + delta; k <= d + delta; k += 2)
            {
                int x;

                if (k == -d + delta || k != d + delta && w[k - 1] >= w[k + 1])
                {
                    x = w[k + 1] - 1;
                }
                else
                {
                    x = w[k - 1];
                }

                var y = x - k;

                var s = x;

                while (x > 0 && y > 0 && comparer.Equals(a[x - 1], b[y - 1]))
                {
                    x--;
                    y--;
                }

                w[k] = x;

                if (even && k >= -d && k <= d && v[k] >= w[k])
                {
                    return (d * 2, x, y, s, s - k);
                }
            }
        }

        throw new UnreachableException();
    }
}