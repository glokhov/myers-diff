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
    /// <param name="path">The path to record vector snapshots into.</param>
    public static void LcsSes<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, EqualityComparer<T> comparer, Path path) where T : IEquatable<T>
    {
        var n = a.Length;
        var m = b.Length;

        if (n == 0 && m == 0)
        {
            return;
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

                    return;
                }
            }

            path.MakeSnapshot(v, d);
        }

        throw new InvalidOperationException("Unexpected end of algorithm.");
    }
}