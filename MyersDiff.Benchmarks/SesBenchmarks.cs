using BenchmarkDotNet.Attributes;

namespace MyersDiff.Benchmarks;

[MemoryDiagnoser]
public class SesBenchmarks
{
    private char[] _original = [];
    private char[] _modified = [];

    [Params(100, 1_000, 10_000)]
    public int Length { get; set; }

    [Params("disjoint", "similar", "identical")]
    public string Scenario { get; set; } = "";

    [GlobalSetup]
    public void Setup()
    {
        var rng = new Random(42);

        _original = Enumerable.Range(0, Length).Select(_ => (char)('a' + rng.Next(26))).ToArray();

        _modified = Scenario switch
        {
            "disjoint" => Enumerable.Range(0, Length).Select(_ => (char)('A' + rng.Next(26))).ToArray(),
            "similar" => _original.Select(c => rng.NextDouble() < 0.1 ? (char)('a' + rng.Next(26)) : c).ToArray(),
            "identical" => (char[])_original.Clone(),
            _ => throw new ArgumentException("Unknown scenario.")
        };
    }

    [Benchmark]
    public List<Ses<char>.Cmd> Build()
    {
        return Ses<char>.Build(_original, _modified, EqualityComparer<char>.Default);
    }
}
