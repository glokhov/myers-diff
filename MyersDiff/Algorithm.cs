using System.Buffers;
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

        var maxD = (n + m + 1) / 2;
        var delta = n - m;

        var deltaEven = delta % 2 == 0;
        var deltaOdd = !deltaEven;

        var offset = maxD + Math.Abs(delta) + 1;
        var len = offset * 2 + 1;

        var fwd = ArrayPool<int>.Shared.Rent(len);
        var bwd = ArrayPool<int>.Shared.Rent(len);

        Array.Clear(fwd, 0, len);
        Array.Clear(bwd, 0, len);

        bwd[delta + 1 + offset] = n + 1;

        try
        {
            for (var d = 0; d <= maxD; d++)
            {
                for (var k = -d; k <= d; k += 2)
                {
                    int x;

                    if (k == -d || (k != d && fwd[k - 1 + offset] < fwd[k + 1 + offset]))
                    {
                        x = fwd[k + 1 + offset];
                    }
                    else
                    {
                        x = fwd[k - 1 + offset] + 1;
                    }

                    var y = x - k;
                    var s = x;

                    while (x < n && y < m && comparer.Equals(a[x], b[y]))
                    {
                        x++;
                        y++;
                    }

                    fwd[k + offset] = x;

                    if (deltaOdd && k >= delta - (d - 1) && k <= delta + (d - 1) && fwd[k + offset] >= bwd[k + offset])
                    {
                        return (d * 2 - 1, s, s - k, x, y);
                    }
                }

                for (var k = -d + delta; k <= d + delta; k += 2)
                {
                    int x;

                    if (k == -d + delta || k != d + delta && bwd[k - 1 + offset] >= bwd[k + 1 + offset])
                    {
                        x = bwd[k + 1 + offset] - 1;
                    }
                    else
                    {
                        x = bwd[k - 1 + offset];
                    }

                    var y = x - k;
                    var s = x;

                    while (x > 0 && y > 0 && comparer.Equals(a[x - 1], b[y - 1]))
                    {
                        x--;
                        y--;
                    }

                    bwd[k + offset] = x;

                    if (deltaEven && k >= -d && k <= d && fwd[k + offset] >= bwd[k + offset])
                    {
                        return (d * 2, x, y, s, s - k);
                    }
                }
            }
        }
        finally
        {
            ArrayPool<int>.Shared.Return(fwd);
            ArrayPool<int>.Shared.Return(bwd);
        }

        throw new UnreachableException();
    }

    private static int CommonPrefix<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer)
    {
        var i = 0;
        var minLen = Math.Min(a.Length, b.Length);

        while (i < minLen && comparer.Equals(a[i], b[i]))
        {
            i++;
        }

        return i;
    }

    private static int CommonSuffix<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer)
    {
        var i = 0;
        var minLen = Math.Min(a.Length, b.Length);

        while (i < minLen && comparer.Equals(a[a.Length - 1 - i], b[b.Length - 1 - i]))
        {
            i++;
        }

        return i;
    }
}