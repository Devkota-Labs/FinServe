namespace Shared.Common.Extensions;

public static class DateTimeExtensions
{
    public static long ToUnixTimeSeconds(this DateTime dt) => new DateTimeOffset(dt).ToUnixTimeSeconds();
    public static DateTime FromUnixTimeSeconds(long seconds) => DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;
}
