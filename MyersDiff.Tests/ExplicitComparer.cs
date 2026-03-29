namespace MyersDiff.Tests;

internal sealed class ExplicitComparer : IEqualityComparer<char>
{
    public static readonly ExplicitComparer Instance = new();

    public bool Equals(char a, char b) => char.ToUpperInvariant(a) == char.ToUpperInvariant(b);

    public int GetHashCode(char obj) => char.ToUpperInvariant(obj).GetHashCode();
}