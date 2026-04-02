namespace MyersDiff;

public static partial class Algorithm
{
    /// <summary>
    ///  Computes the full diff between two sequences, returning every edit
    ///  operation including deletions, insertions, and equal elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <returns>A list of <see cref="Diff"/> records representing all edit operations.</returns>
    public static List<Diff> ComputeDiff<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
    {
        return ComputeDiff(a, b, EqualityComparer<T>.Default);
    }

    /// <summary>
    ///  Computes the full diff between two sequences, returning every edit
    ///  operation including deletions, insertions, and equal elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <param name="comparer">The equality comparer used to compare elements.</param>
    /// <returns>A list of <see cref="Diff"/> records representing all edit operations.</returns>
    public static List<Diff> ComputeDiff<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer)
    {
        ArgumentNullException.ThrowIfNull(comparer);

        var edits = new List<Diff>();

        ComputeDiffCore(a, b, comparer, edits, 0, 0);

        return edits;
    }

    /// <summary>
    ///  Reorders the diffs in-place so that all deletions come before any insertions
    ///  within each contiguous block of non-equal edits.
    /// </summary>
    /// <param name="diffs">The list of diffs to reorder in-place.</param>
    public static void ReorderDeletesBeforeInserts(List<Diff> diffs)
    {
        var i = 0;

        while (i < diffs.Count)
        {
            if (diffs[i] is Diff.Equal)
            {
                i++;
                continue;
            }

            // Find the extent of this non-Equal block.
            var blockStart = i;
            var deletes = 0;
            var inserts = 0;

            while (i < diffs.Count && diffs[i] is not Diff.Equal)
            {
                if (diffs[i] is Diff.Delete)
                {
                    deletes++;
                }
                else
                {
                    inserts++;
                }

                i++;
            }

            // Nothing to reorder if the block is purely one type.
            if (deletes == 0 || inserts == 0)
            {
                continue;
            }

            // Determine the start point of this block in the edit graph.
            int x0, y0;

            if (blockStart == 0)
            {
                x0 = 0;
                y0 = 0;
            }
            else
            {
                var prev = (Diff.Equal)diffs[blockStart - 1];
                x0 = prev.X;
                y0 = prev.Y;
            }

            // Rewrite the block: all deletes first, then all inserts.
            var idx = blockStart;

            for (var d = 1; d <= deletes; d++)
            {
                diffs[idx++] = new Diff.Delete(x0 + d);
            }

            for (var ins = 1; ins <= inserts; ins++)
            {
                diffs[idx++] = new Diff.Insert(x0 + deletes, y0 + ins);
            }
        }
    }

    private static void ComputeDiffCore<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer, List<Diff> edits, int aOffset, int bOffset)
    {
        if (a.Length > 0 && b.Length == 0)
        {
            for (var i = 0; i < a.Length; i++)
            {
                edits.Add(new Diff.Delete(aOffset + i + 1));
            }

            return;
        }

        if (a.Length == 0 && b.Length > 0)
        {
            for (var j = 0; j < b.Length; j++)
            {
                edits.Add(new Diff.Insert(aOffset, bOffset + j + 1));
            }

            return;
        }

        if (a.Length == 0)
        {
            return;
        }

        var (d, x, y, u, v) = FindMiddleSnake(a, b, comparer);

        if (d == 0)
        {
            // All elements are equal.
            for (var i = 0; i < a.Length; i++)
            {
                edits.Add(new Diff.Equal(aOffset + i + 1, bOffset + i + 1));
            }

            return;
        }

        if (d == 1)
        {
            var p = 0;
            var minLen = Math.Min(a.Length, b.Length);

            while (p < minLen && comparer.Equals(a[p], b[p]))
            {
                p++;
            }

            // Equal prefix.
            for (var i = 0; i < p; i++)
            {
                edits.Add(new Diff.Equal(aOffset + i + 1, bOffset + i + 1));
            }

            if (a.Length > b.Length)
            {
                edits.Add(new Diff.Delete(aOffset + p + 1));

                // Equal suffix: remaining b[p..] matches a[p+1..].
                for (var i = 0; i < b.Length - p; i++)
                {
                    edits.Add(new Diff.Equal(aOffset + p + 2 + i, bOffset + p + 1 + i));
                }
            }
            else
            {
                edits.Add(new Diff.Insert(aOffset + p, bOffset + p + 1));

                // Equal suffix: remaining a[p..] matches b[p+1..].
                for (var i = 0; i < a.Length - p; i++)
                {
                    edits.Add(new Diff.Equal(aOffset + p + 1 + i, bOffset + p + 2 + i));
                }
            }

            return;
        }

        // Recurse on the portion before the middle snake.
        ComputeDiffCore(a[..x], b[..y], comparer, edits, aOffset, bOffset);

        // Equal elements in the middle snake (diagonal).
        for (var i = 0; i < u - x; i++)
        {
            edits.Add(new Diff.Equal(aOffset + x + i + 1, bOffset + y + i + 1));
        }

        // Recurse on the portion after the middle snake.
        ComputeDiffCore(a[u..], b[v..], comparer, edits, aOffset + u, bOffset + v);
    }
}