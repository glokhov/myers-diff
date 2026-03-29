# MyersDiff

A C# implementation of [Eugene Myers' O(ND) difference algorithm](https://publications.mpi-cbg.de/Myers_1986_6330.pdf) for computing the **Longest Common Subsequence (LCS)** and **Shortest Edit Script (SES)** between two sequences.

## Features

- Generic — works with any element type via `ReadOnlySpan<T>`
- String convenience overloads for the common case
- Zero external dependencies

## Longest Common Subsequence

### String

```csharp
string lcs = Lcs.Build("abcabba", "cbabac");

// "caba"
```

With a custom comparer:

```csharp
string lcs = Lcs.Build("abcabba", "cbabac", EqualityComparer<char>.Default);

// "caba"
```

### Generic

```csharp
char[] lcs = Lcs<char>.Build("abcabba", "cbabac");

// ['c', 'a', 'b', 'a']
```

With a custom comparer:

```csharp
char[] lcs = Lcs<char>.Build("abcabba", "cbabac", EqualityComparer<char>.Default);

// ['c', 'a', 'b', 'a']
```

## Shortest Edit Script

### String

```csharp
Ses.Cmd[] ses = Ses.Build("abcabba", "cbabac");

// [Del(1), Del(2), Ins(3, 'b'), Del(6), Ins(7, 'c')]

foreach (var cmd in ses)
{
    switch (cmd)
    {
        case Ses.Cmd.Del del:
            Console.WriteLine($"Delete at position {del.Pos}");
            break;
        case Ses.Cmd.Ins ins:
            Console.WriteLine($"Insert '{ins.Char}' at position {ins.Pos}");
            break;
    }
}
```

With a custom comparer:

```csharp
Ses.Cmd[] ses = Ses.Build("abcabba", "cbabac", EqualityComparer<char>.Default);

// [Del(1), Del(2), Ins(3, 'b'), Del(6), Ins(7, 'c')]
```

### Generic

```csharp
Ses<char>.Cmd[] ses = Ses<char>.Build("abcabba", "cbabac");

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

With a custom comparer:

```csharp
Ses<char>.Cmd[] ses = Ses<char>.Build("abcabba", "cbabac", EqualityComparer<char>.Default);

// [Del(1), Del(2), Ins(3, 'b'), Del(6), Ins(7, 'c')]
```


## References

- Myers, E.W. *An O(ND) difference algorithm and its variations.* Algorithmica 1, 251–266 (1986). [PDF](https://publications.mpi-cbg.de/Myers_1986_6330.pdf)
