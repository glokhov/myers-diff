using System.Diagnostics;

namespace MyersDiff;

/// <summary>
///  Core implementation of the Myers O(ND) difference algorithm.
/// </summary>
public static class Algorithm
{
    /// <summary>
    ///  Finds the middle snake between two sequences using the Myers O(ND) difference algorithm.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <param name="comparer">The equality comparer used to compare elements.</param>
    /// <returns>A <see cref="Snake"/> containing the <see cref="Snake.Start"/> and <see cref="Snake.End"/> endpoints of the middle snake.</returns>
    public static Snake GetMiddleSnake<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer)
    {
        var n = a.Length;
        var m = b.Length;

        var max = (n + m) / 2;
        var delta = n - m;

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

                if (delta % 2 != 0 && k >= delta - (d - 1) && k <= delta + (d - 1) && v[k] >= w[k])
                {
                    return new Snake(new Endpoint(s, s - k), new Endpoint(x, y));
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

                if (delta % 2 == 0 && k >= -d && k <= d && v[k] >= w[k])
                {
                    return new Snake(new Endpoint(x, y), new Endpoint(s, s - k));
                }
            }
        }

        throw new UnreachableException();
    }

    /// <summary>
    ///  Computes the longest common subsequence and shortest edit script between two sequences
    ///  using the Myers O(ND) difference algorithm.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <param name="comparer">The equality comparer used to compare elements.</param>
    /// <returns>A <see cref="Path"/> containing vector snapshots from the forward pass.</returns>
    [Obsolete("This method uses excessive memory due to storing full vector snapshots. This method will be removed in the final release.")]
    public static Path LcsSes<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer)
    {
        ArgumentNullException.ThrowIfNull(comparer);

        var n = a.Length;
        var m = b.Length;

        var p = new Path(n, m);

        if (n == 0 && m == 0)
        {
            return p;
        }

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
                    // The final d-step is not snapshotted. During backtracking, any position
                    // not found in a snapshot is interpreted as a diagonal (equal) move.

                    return p;
                }
            }

            p.MakeSnapshot(v, d);
        }

        throw new UnreachableException();
    }
}