namespace HRSystem.Application.Helpers;

public static class AddressHelper
{
    public static string? CombineAddress(params string?[] lines)
    {
        var filtered = lines.Where(l => !string.IsNullOrWhiteSpace(l));
        return string.Join("\n", filtered);
    }

    public static ReadOnlySpan<string> SplitAddress(string? address)
    {
        if (string.IsNullOrEmpty(address)) return [];
        var split = address.Split('\n');
        return split.AsSpan(0, Math.Min(4, split.Length));
    }
}