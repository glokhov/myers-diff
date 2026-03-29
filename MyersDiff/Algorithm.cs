using System.Diagnostics;

namespace MyersDiff;

/// <summary>
///  Core implementation of the Myers O(ND) difference algorithm.
/// </summary>
public static class Algorithm
{
    /// <summary>
    ///  Computes the longest common subsequence and shortest edit script between two sequences
    ///  using the Myers O(ND) difference algorithm.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <param name="comparer">The equality comparer used to compare elements.</param>
    /// <returns>A <see cref="Path"/> containing vector snapshots from the forward pass.</returns>
    public static Path LcsSes<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer) where T : IEquatable<T>
    {
        var p = new Path();

        var n = a.Length;
        var m = b.Length;

        if (n is 0 && m is 0)
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

        Debug.Fail("Unexpected end of algorithm.");

        return p;
    }
}