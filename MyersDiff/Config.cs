namespace MyersDiff;

public sealed class Config
{
    public bool UseDelete { get; init; }

    public bool UseInsert { get; init; }

    public bool UseEqual { get; init; }

    public static Config Lcs { get; } = new()
    {
        UseEqual = true
    };

    public static Config Ses { get; } = new()
    {
        UseDelete = true,
        UseInsert = true
    };
}