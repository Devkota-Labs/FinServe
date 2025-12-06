namespace Shared.Common;

public static class Constants
{
    public const int DefaultPageSize = 25;
    public const string DateFormat = "yyyy-MM-ddTHH:mm:ssZ";
    public static readonly IFormatProvider IFormatProvider = System.Globalization.CultureInfo.InvariantCulture;
}
