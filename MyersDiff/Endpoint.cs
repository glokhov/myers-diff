namespace MyersDiff;

/// <summary>
///  Represents a position in the edit graph.
/// </summary>
/// <param name="X">The index in the original sequence.</param>
/// <param name="Y">The index in the modified sequence.</param>
public readonly record struct Endpoint(int X, int Y);