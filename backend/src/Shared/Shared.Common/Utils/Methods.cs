using System.Text;

namespace Shared.Common.Utils;

public static class Methods
{
    public static string GetToString(params object?[] items)
    {
        return string.Join("|", items.Select(i => i?.ToString() ?? "null"));
    }

    public static string GetToString(char seperator, params object?[] items)
    {
        return string.Join(seperator, items.Select(i => i?.ToString() ?? "null"));
    }

    public static string GetStringFromBytes(byte[] byteArray, int offset, int length)
    {
        return Encoding.ASCII.GetString(byteArray, offset, length).Trim('\0', ' ');
    }
    public static string ListToString<T>(IEnumerable<T>? list, string separator = ", ")
    {
        return list == null ? string.Empty : string.Join(separator, list);
    }
}
