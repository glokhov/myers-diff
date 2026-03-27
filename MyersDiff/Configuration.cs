namespace MyersDiff;

public sealed class Configuration
{
    public bool ReturnDelete { get; init; }

    public bool ReturnInsert { get; init; }

    public bool ReturnEqual { get; init; }
}