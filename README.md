# Myers' difference algorithm [![Nuget Version](https://img.shields.io/nuget/v/MyersDiff)](https://www.nuget.org/packages/MyersDiff)

A C# implementation of [Eugene Myers' O(ND) difference algorithm](https://publications.mpi-cbg.de/Myers_1986_6330.pdf) for computing the **longest common subsequence (LCS)**, **shortest edit script (SES)**, and **full diff** between two sequences.

## Features

- Generic — works with any element type via `ReadOnlySpan<T>`
- Custom equality comparers via `IEqualityComparer<T>`
- Linear-space divide-and-conquer implementation
- Zero external dependencies

## Longest Common Subsequence

```csharp
List<char> subsequence = Algorithm.ComputeLcs<char>("abcabba", "cbabac");

// ['b', 'a', 'b', 'a']
```

With an explicit comparer:

```csharp
List<char> subsequence = Algorithm.ComputeLcs("abcabba", "cbabac", EqualityComparer<char>.Default);

// ['b', 'a', 'b', 'a']
```

## Shortest Edit Script

```csharp
List<Edit<char>> script = Algorithm.ComputeSes<char>("abcabba", "cbabac");

// [Insert(0, c), Delete(1), Delete(3), Delete(6), Insert(7, c)]

foreach (var command in script)
{
    switch (command)
    {
        case Edit<char>.Delete delete:
            Console.WriteLine($"Delete at position {delete.Position}");
            break;
        case Edit<char>.Insert insert:
            Console.WriteLine($"Insert '{insert.Element}' at position {insert.Position}");
            break;
    }
}
```

With an explicit comparer:

```csharp
List<Edit<char>> script = Algorithm.ComputeSes("abcabba", "cbabac", EqualityComparer<char>.Default);

// [Insert(0, c), Delete(1), Delete(3), Delete(6), Insert(7, c)]
```

## Diff

```csharp
string a = "abcabba";
string b = "cbabac";

List<Diff> diffs = Algorithm.ComputeDiff<char>(a, b);

Algorithm.ReorderDeletesBeforeInserts(diffs);

foreach (var diff in diffs)
{
    switch (diff)
    {
        case Diff.Delete del:
            Console.WriteLine($"- {a[del.X - 1]}");
            break;
        case Diff.Insert ins:
            Console.WriteLine($"+ {b[ins.Y - 1]}");
            break;
        case Diff.Equal eq:
            Console.WriteLine($"  {a[eq.X - 1]}");
            break;
    }
}

// - a
// + c
//   b
// - c
//   a
//   b
// - b
//   a
// + c
```

With an explicit comparer:

```csharp
List<Diff> diffs = Algorithm.ComputeDiff("abcabba", "cbabac", EqualityComparer<char>.Default);
```

### Reordering deletions before insertions

By default, within each non-equal block the algorithm emits insertions before deletions. Call `Algorithm.ReorderDeletesBeforeInserts` to flip the order so that all deletions appear first — useful when rendering traditional unified diffs where `-` lines precede `+` lines.

```csharp
List<Diff> diffs = Algorithm.ComputeDiff<char>(a, b);

Algorithm.ReorderDeletesBeforeInserts(diffs);
```

## References

- Myers, E.W. *An O(ND) difference algorithm and its variations.* Algorithmica 1, 251–266 (1986). [PDF](https://publications.mpi-cbg.de/Myers_1986_6330.pdf)
