namespace MyersDiff;

public static partial class Algorithm
{
    /// <summary>
    ///  Computes the longest common subsequence between two sequences
    ///  using a divide-and-conquer approach with the middle snake.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <returns>A list containing the longest common subsequence.</returns>
    public static List<T> ComputeLcs<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
    {
        return ComputeLcs(a, b, EqualityComparer<T>.Default);
    }

    /// <summary>
    ///  Computes the longest common subsequence between two sequences
    ///  using a divide-and-conquer approach with the middle snake.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <param name="comparer">The equality comparer used to compare elements.</param>
    /// <returns>A list containing the longest common subsequence.</returns>
    public static List<T> ComputeLcs<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer)
    {
        ArgumentNullException.ThrowIfNull(comparer);

        var lcs = new List<T>();

        ComputeLcsCore(a, b, comparer, lcs);

        return lcs;
    }

    private static void ComputeLcsCore<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer, List<T> lcs)
    {
        if (a.Length == 0 || b.Length == 0)
        {
            return;
        }

        // Trim common prefix.
        var prefix = CommonPrefix(a, b, comparer);

        for (var i = 0; i < prefix; i++)
        {
            lcs.Add(a[i]);
        }

        a = a[prefix..];
        b = b[prefix..];

        // Trim common suffix.
        var suffix = CommonSuffix(a, b, comparer);
        var suffixSpan = a.Slice(a.Length - suffix, suffix);

        a = a[..^suffix];
        b = b[..^suffix];

        // Process the trimmed middle.
        if (a.Length > 0 && b.Length > 0)
        {
            var (d, x, y, u, v) = FindMiddleSnake(a, b, comparer);

            if (d == 0)
            {
                // Sequences are identical — every element is part of the LCS.
                foreach (var e in a)
                {
                    lcs.Add(e);
                }
            }
            else if (d == 1)
            {
                // Exactly one edit (insert or delete). The shorter sequence
                // is entirely contained as a subsequence of the longer one.
                var shorter = a.Length < b.Length ? a : b;

                foreach (var e in shorter)
                {
                    lcs.Add(e);
                }
            }
            else
            {
                // Recurse on the portion before the middle snake.
                ComputeLcsCore(a[..x], b[..y], comparer, lcs);

                // The middle snake itself is a diagonal — all equal elements.
                for (var i = x; i < u; i++)
                {
                    lcs.Add(a[i]);
                }

                // Recurse on the portion after the middle snake.
                ComputeLcsCore(a[u..], b[v..], comparer, lcs);
            }
        }

        // Append common suffix to LCS.
        for (var i = 0; i < suffix; i++)
        {
            lcs.Add(suffixSpan[i]);
        }
    }
}