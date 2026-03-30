# Myers' difference algorithm [![Nuget Version](https://img.shields.io/nuget/v/MyersDiff)](https://www.nuget.org/packages/MyersDiff)

A C# implementation of [Eugene Myers' O(ND) difference algorithm](https://publications.mpi-cbg.de/Myers_1986_6330.pdf) for computing the **Longest Common Subsequence (LCS)** and **Shortest Edit Script (SES)** between two sequences.

## Features

- Generic — works with any element type via `ReadOnlySpan<T>`
- Custom equality comparers via `IEqualityComparer<T>`
- String convenience overloads for the common case
- Zero external dependencies

## Longest Common Subsequence

### String

```csharp
string lcs = Lcs.Build("abcabba", "cbabac");

// "caba"
```

With an explicit comparer:

```csharp
string lcs = Lcs.Build("abcabba", "cbabac", EqualityComparer<char>.Default);

// "caba"
```

### Generic

```csharp
ReadOnlySpan<char> lcs = Lcs<char>.Build("abcabba", "cbabac");

// ['c', 'a', 'b', 'a']
```

With an explicit comparer:

```csharp
ReadOnlySpan<char> lcs = Lcs<char>.Build("abcabba", "cbabac", EqualityComparer<char>.Default);

// ['c', 'a', 'b', 'a']
```

## Shortest Edit Script

### String

```csharp
ReadOnlySpan<Ses.Cmd> ses = Ses.Build("abcabba", "cbabac");

// [Del(1), Del(2), Ins(3, 'b'), Del(6), Ins(7, 'c')]

foreach (var cmd in ses)
{
    switch (cmd)
    {
        case Ses.Cmd.Del del:
            Console.WriteLine($"Delete at position {del.Pos}");
            break;
        case Ses.Cmd.Ins ins:
            Console.WriteLine($"Insert '{ins.Item}' at position {ins.Pos}");
            break;
    }
}
```

With an explicit comparer:

```csharp
ReadOnlySpan<Ses.Cmd> ses = Ses.Build("abcabba", "cbabac", EqualityComparer<char>.Default);

// [Del(1), Del(2), Ins(3, 'b'), Del(6), Ins(7, 'c')]
```

### Generic

```csharp
ReadOnlySpan<Ses<char>.Cmd> ses = Ses<char>.Build("abcabba", "cbabac");

// [Del(1), Del(2), Ins(3, 'b'), Del(6), Ins(7, 'c')]

foreach (var cmd in ses)
{
    switch (cmd)
    {
        case Ses<char>.Cmd.Del del:
            Console.WriteLine($"Delete at position {del.Pos}");
            break;
        case Ses<char>.Cmd.Ins ins:
            Console.WriteLine($"Insert '{ins.Item}' at position {ins.Pos}");
            break;
    }
}
```

With an explicit comparer:

```csharp
ReadOnlySpan<Ses<char>.Cmd> ses = Ses<char>.Build("abcabba", "cbabac", EqualityComparer<char>.Default);

// [Del(1), Del(2), Ins(3, 'b'), Del(6), Ins(7, 'c')]
```

## Custom Trace Logic

The `Path`, `Vector`, and `Trace` types are public, so you can call `Algorithm.LcsSes` directly and implement your own trace logic on top of the recorded snapshots.

For example, building a unified diff-style output:

```csharp
string a = "abcabba";
string b = "cbabac";

var path = Algorithm.LcsSes(a, b, EqualityComparer<char>.Default);

var filter = Trace.Filter.Del | Trace.Filter.Ins | Trace.Filter.Eq;

foreach (var edit in Trace.EnumerateEdits(path, filter))
{
    switch (edit.Op)
    {
        case Trace.Op.Del:
            Console.WriteLine($"- {a[edit.X - 1]}");
            break;
        case Trace.Op.Ins:
            Console.WriteLine($"+ {b[edit.Y - 1]}");
            break;
        case Trace.Op.Eq:
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
