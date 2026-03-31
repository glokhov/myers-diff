using BenchmarkDotNet.Running;
using MyersDiff.Benchmarks;

_ = BenchmarkRunner.Run([
    typeof(AlgorithmBenchmarks),
    typeof(LcsBenchmarks),
    typeof(SesBenchmarks)
]);