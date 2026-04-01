# Myers' difference algorithm [![Nuget Version](https://img.shields.io/nuget/v/MyersDiff)](https://www.nuget.org/packages/MyersDiff)

A C# implementation of [Eugene Myers' O(ND) difference algorithm](https://publications.mpi-cbg.de/Myers_1986_6330.pdf) for computing the **Longest Common Subsequence (LCS)** and **Shortest Edit Script (SES)** between two sequences.

## Features

- Generic — works with any element type via `ReadOnlySpan<T>`
- Custom equality comparers via `IEqualityComparer<T>`
- Zero external dependencies

## Longest Common Subsequence

```csharp
List<char> subsequence = Lcs<char>.Build("abcabba", "cbabac");

// ['c', 'a', 'b', 'a']
```

With an explicit comparer:

```csharp
List<char> subsequence = Lcs<char>.Build("abcabba", "cbabac", EqualityComparer<char>.Default);

// ['c', 'a', 'b', 'a']
```

## Shortest Edit Script

```csharp
List<Ses<char>.Command> script = Ses<char>.Build("abcabba", "cbabac");

// [Delete(1), Delete(2), Insert(3, 'b'), Delete(6), Insert(7, 'c')]

foreach (var command in script)
{
    switch (command)
    {
        case Ses<char>.Command.Delete delete:
            Console.WriteLine($"Delete at position {delete.Position}");
            break;
        case Ses<char>.Command.Insert insert:
            Console.WriteLine($"Insert '{insert.Element}' at position {insert.Position}");
            break;
    }
}
```

With an explicit comparer:

```csharp
List<Ses<char>.Command> script = Ses<char>.Build("abcabba", "cbabac", EqualityComparer<char>.Default);

// [Delete(1), Delete(2), Insert(3, 'b'), Delete(6), Insert(7, 'c')]
```

## Custom Trace Logic

The `Path`, `Vector`, and `Trace` types are public, so you can call `Algorithm.LcsSes` directly and implement your own trace logic on top of the recorded snapshots.

For example, building a unified diff-style output:

```csharp
string a = "abcabba";
string b = "cbabac";

var path = Algorithm.LcsSes(a, b, EqualityComparer<char>.Default);

var operation = Trace.Operation.Delete | Trace.Operation.Insert | Trace.Operation.Equal;

foreach (var edit in Trace.GetEdits(path, operation))
{
    switch (edit.Operation)
    {
        case Trace.Operation.Delete:
            Console.WriteLine($"- {a[edit.X - 1]}");
            break;
        case Trace.Operation.Insert:
            Console.WriteLine($"+ {b[edit.Y - 1]}");
            break;
        case Trace.Operation.Equal:
            Console.WriteLine($"  {a[edit.X - 1]}");
            break;
    }
}

// - a
// - b
//   c
// + b
//   a
//   b
// - b
//   a
// + c
```

## Limitations

The linear-space refinement (divide-and-conquer) described in the original paper is not yet implemented. The current implementation stores the full snapshot history during the forward pass. This is planned for a future release.

## References

- Myers, E.W. *An O(ND) difference algorithm and its variations.* Algorithmica 1, 251–266 (1986). [PDF](https://publications.mpi-cbg.de/Myers_1986_6330.pdf)
