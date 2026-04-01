namespace MyersDiff;

/// <summary>
///  Represents the middle snake found by the Myers O(ND) difference algorithm.
/// </summary>
/// <param name="Start">The start of the snake.</param>
/// <param name="End">The end of the snake.</param>
public readonly record struct Snake(Endpoint Start, Endpoint End);