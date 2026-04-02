namespace MyersDiff;

/// <summary>
///  Represents an edit command in the shortest edit script.
/// </summary>
public abstract record Command<T>
{
    /// <summary>
    ///  A command to delete an element at the specified position.
    /// </summary>
    /// <param name="Position">The 1-based position in the original sequence.</param>
    public sealed record Delete(int Position) : Command<T>;

    /// <summary>
    ///  A command to insert an element at the specified position.
    /// </summary>
    /// <param name="Position">The 1-based position in the original sequence after which to insert.</param>
    /// <param name="Element">The element to insert.</param>
    public sealed record Insert(int Position, T Element) : Command<T>;
}