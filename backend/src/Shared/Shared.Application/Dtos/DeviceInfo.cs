namespace Shared.Application.Dtos;

public sealed class DeviceInfo
{
    public string UserAgent { get; init; } = default!;
    public string Device { get; init; } = default!;
    public string IpAddress { get; init; } = default!;
}