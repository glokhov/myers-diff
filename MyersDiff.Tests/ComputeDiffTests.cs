namespace MyersDiff.Tests;

public sealed class ComputeDiffTests
{
    private const string A = "abcabba";
    private const string B = "cbabac";

    [Fact]
    public void Test()
    {
        var diffs = Algorithm.ComputeDiff<char>(A, B);

        Assert.Equal(
            [
                new Diff.Insert(0, 1),
                new Diff.Delete(1),
                new Diff.Equal(2, 2),
                new Diff.Delete(3),
                new Diff.Equal(4, 3),
                new Diff.Equal(5, 4),
                new Diff.Delete(6),
                new Diff.Equal(7, 5),
                new Diff.Insert(7, 6)
            ],
            diffs);
    }

    [Fact]
    public void Test_Comparer_Reorder()
    {
        var diffs = Algorithm.ComputeDiff(A, B, EqualityComparer<char>.Default);

        Algorithm.ReorderDeletesBeforeInserts(diffs);

        Assert.Equal(
            [
                new Diff.Delete(1),
                new Diff.Insert(1, 1),
                new Diff.Equal(2, 2),
                new Diff.Delete(3),
                new Diff.Equal(4, 3),
                new Diff.Equal(5, 4),
                new Diff.Delete(6),
                new Diff.Equal(7, 5),
                new Diff.Insert(7, 6)
            ],
            diffs);
    }

    [Fact]
    public void Test_BothEmpty()
    {
        Assert.Empty(Algorithm.ComputeDiff<char>("", ""));
    }

    [Fact]
    public void Test_FirstEmpty()
    {
        var diffs = Algorithm.ComputeDiff<char>("", "abc");

        Assert.Equal(
            [
                new Diff.Insert(0, 1),
                new Diff.Insert(0, 2),
                new Diff.Insert(0, 3)
            ],
            diffs);
    }

    [Fact]
    public void Test_SecondEmpty()
    {
        var diffs = Algorithm.ComputeDiff<char>("abc", "");

        Assert.Equal(
            [
                new Diff.Delete(1),
                new Diff.Delete(2),
                new Diff.Delete(3)
            ],
            diffs);
    }

    [Fact]
    public void Test_Identical()
    {
        var diffs = Algorithm.ComputeDiff<char>("abc", "abc");

        Assert.Equal(
            [
                new Diff.Equal(1, 1),
                new Diff.Equal(2, 2),
                new Diff.Equal(3, 3)
            ],
            diffs);
    }

    [Fact]
    public void Test_Disjoint()
    {
        var diffs = Algorithm.ComputeDiff<char>("abc", "xyz");

        Assert.Equal(
            [
                new Diff.Insert(0, 1),
                new Diff.Insert(0, 2),
                new Diff.Insert(0, 3),
                new Diff.Delete(1),
                new Diff.Delete(2),
                new Diff.Delete(3)
            ],
            diffs);
    }

    [Fact]
    public void Test_Disjoint_Reorder()
    {
        var diffs = Algorithm.ComputeDiff<char>("abc", "xyz");

        Algorithm.ReorderDeletesBeforeInserts(diffs);

        Assert.Equal(
            [
                new Diff.Delete(1),
                new Diff.Delete(2),
                new Diff.Delete(3),
                new Diff.Insert(3, 1),
                new Diff.Insert(3, 2),
                new Diff.Insert(3, 3)
            ],
            diffs);
    }

    [Fact]
    public void Test_SingleCharSame()
    {
        var diffs = Algorithm.ComputeDiff<char>("a", "a");

        Assert.Equal([new Diff.Equal(1, 1)], diffs);
    }

    [Fact]
    public void Test_SingleCharDifferent()
    {
        var diffs = Algorithm.ComputeDiff<char>("a", "b");

        Assert.Equal(
            [
                new Diff.Insert(0, 1),
                new Diff.Delete(1)
            ],
            diffs);
    }

    [Fact]
    public void Test_SingleCharDifferent_Reorder()
    {
        var diffs = Algorithm.ComputeDiff<char>("a", "b");

        Algorithm.ReorderDeletesBeforeInserts(diffs);

        Assert.Equal(
            [
                new Diff.Delete(1),
                new Diff.Insert(1, 1)
            ],
            diffs);
    }

    [Fact]
    public void Test_Prefix()
    {
        var diffs = Algorithm.ComputeDiff<char>("abc", "abcd");

        Assert.Equal(
            [
                new Diff.Equal(1, 1),
                new Diff.Equal(2, 2),
                new Diff.Equal(3, 3),
                new Diff.Insert(3, 4)
            ],
            diffs);
    }

    [Fact]
    public void Test_Suffix()
    {
        var diffs = Algorithm.ComputeDiff<char>("abcd", "bcd");

        Assert.Equal(
            [
                new Diff.Delete(1),
                new Diff.Equal(2, 1),
                new Diff.Equal(3, 2),
                new Diff.Equal(4, 3)
            ],
            diffs);
    }

    [Fact]
    public void Test_ExplicitComparer_Identical()
    {
        var diffs = Algorithm.ComputeDiff("abc", "ABC", ExplicitComparer.Instance);

        Assert.Equal(
            [
                new Diff.Equal(1, 1),
                new Diff.Equal(2, 2),
                new Diff.Equal(3, 3)
            ],
            diffs);
    }

    [Fact]
    public void Test_ExplicitComparer_Mixed()
    {
        var diffs = Algorithm.ComputeDiff("abX", "ABY", ExplicitComparer.Instance);

        Assert.Equal(
            [
                new Diff.Equal(1, 1),
                new Diff.Equal(2, 2),
                new Diff.Insert(2, 3),
                new Diff.Delete(3)
            ],
            diffs);
    }

    [Fact]
    public void Test_ExplicitComparer_Mixed_Reorder()
    {
        var diffs = Algorithm.ComputeDiff("abX", "ABY", ExplicitComparer.Instance);

        Algorithm.ReorderDeletesBeforeInserts(diffs);

        Assert.Equal(
            [
                new Diff.Equal(1, 1),
                new Diff.Equal(2, 2),
                new Diff.Delete(3),
                new Diff.Insert(3, 3)
            ],
            diffs);
    }

    [Fact]
    public void Test_EditCount_Equals_N_Plus_M_Minus_Lcs()
    {
        var diffs = Algorithm.ComputeDiff<char>(A, B);
        var lcs = Algorithm.ComputeLcs<char>(A, B);

        Assert.Equal(A.Length + B.Length - lcs.Count, diffs.Count);
    }

    [Fact]
    public void Test_Equals_Match_Lcs()
    {
        var diffs = Algorithm.ComputeDiff<char>(A, B);
        var lcs = Algorithm.ComputeLcs<char>(A, B);

        var equalsCount = diffs.Count(e => e is Diff.Equal);

        Assert.Equal(lcs.Count, equalsCount);
    }

    [Fact]
    public void Test_Deletes_Plus_Equals_Cover_Original()
    {
        var diffs = Algorithm.ComputeDiff<char>(A, B);

        var deleteAndEqualCount = diffs.Count(e => e is Diff.Delete or Diff.Equal);

        Assert.Equal(A.Length, deleteAndEqualCount);
    }

    [Fact]
    public void Test_Inserts_Plus_Equals_Cover_Modified()
    {
        var diffs = Algorithm.ComputeDiff<char>(A, B);

        var insertAndEqualCount = diffs.Count(e => e is Diff.Insert or Diff.Equal);

        Assert.Equal(B.Length, insertAndEqualCount);
    }
}