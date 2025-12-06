namespace Shared.Common.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string? s) => string.IsNullOrEmpty(s);
    public static bool IsNullOrWhiteSpace(this string? s) => string.IsNullOrWhiteSpace(s);

    public static string Truncate(this string s, int maxLength)
    {
        if (string.IsNullOrEmpty(s)) return s;
        if (s.Length <= maxLength) return s;
        return s.Substring(0, maxLength);
    }
}
