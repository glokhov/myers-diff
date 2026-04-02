namespace MyersDiff;

public static partial class Algorithm
{
    /// <summary>
    ///  Computes the shortest edit script between two sequences
    ///  using a divide-and-conquer approach with the middle snake.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <returns>A list of <see cref="Edit{T}"/> records representing the edit commands.</returns>
    public static List<Edit<T>> ComputeSes<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
    {
        return ComputeSes(a, b, EqualityComparer<T>.Default);
    }

    /// <summary>
    ///  Computes the shortest edit script between two sequences
    ///  using a divide-and-conquer approach with the middle snake.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="a">The original sequence.</param>
    /// <param name="b">The modified sequence.</param>
    /// <param name="comparer">The equality comparer used to compare elements.</param>
    /// <returns>A list of <see cref="Edit{T}"/> records representing the edit commands.</returns>
    public static List<Edit<T>> ComputeSes<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer)
    {
        ArgumentNullException.ThrowIfNull(comparer);

        var ses = new List<Edit<T>>();

        ComputeSesCore(a, b, comparer, ses, 0);

        return ses;
    }

    private static void ComputeSesCore<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, IEqualityComparer<T> comparer, List<Edit<T>> ses, int offset)
    {
        // Trim common prefix.
        var prefix = CommonPrefix(a, b, comparer);

        a = a[prefix..];
        b = b[prefix..];

        offset += prefix;

        // Trim common suffix.
        var suffix = CommonSuffix(a, b, comparer);

        a = a[..^suffix];
        b = b[..^suffix];

        // Process the trimmed middle.
        if (a.Length > 0 && b.Length == 0)
        {
            for (var i = 0; i < a.Length; i++)
            {
                ses.Add(new Edit<T>.Delete(offset + i + 1));
            }

            return;
        }

        if (a.Length == 0 && b.Length > 0)
        {
            foreach (var e in b)
            {
                ses.Add(new Edit<T>.Insert(offset, e));
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

            if (a.Length > b.Length)
            {
                ses.Add(new Edit<T>.Delete(offset + p + 1));
            }
            else
            {
                ses.Add(new Edit<T>.Insert(offset + p, b[p]));
            }

            return;
        }

        // Recurse on the portion before the middle snake.
        ComputeSesCore(a[..x], b[..y], comparer, ses, offset);

        // Recurse on the portion after the middle snake.
        ComputeSesCore(a[u..], b[v..], comparer, ses, offset + u);
    }
}