using Microsoft.AspNetCore.Http;
using Shared.Application.Dtos;
using Shared.Application.Interfaces.Services;

namespace Shared.Infrastructure.Services;

internal sealed class DeviceResolver : IDeviceResolver
{
    private static string GetDeviceFromUserAgent(string? userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
            return "Unknown Device";

        if (userAgent.Contains("Android", StringComparison.OrdinalIgnoreCase))
            return "Android Device";

        if (userAgent.Contains("iPhone", StringComparison.OrdinalIgnoreCase))
            return "iPhone";

        if (userAgent.Contains("iPad", StringComparison.OrdinalIgnoreCase))
            return "iPad";

        if (userAgent.Contains("Windows", StringComparison.OrdinalIgnoreCase))
            return "Windows PC";

        if (userAgent.Contains("Macintosh", StringComparison.OrdinalIgnoreCase))
            return "Mac";

        return "Unknown Device";
    }

    public DeviceInfo Resolve(HttpContext? context)
    {
        var userAgent =
            context?.Request.Headers["User-Agent"].ToString();

        var ipAddress =
            context?.Request.Headers["X-Forwarded-For"].FirstOrDefault()
            ?? context?.Connection.RemoteIpAddress?.ToString()
            ?? "Unknown";

        var device = GetDeviceFromUserAgent(userAgent);

        return new DeviceInfo
        {
            UserAgent = userAgent ?? "unknown",
            Device = device,
            IpAddress = ipAddress
        };
    }
}