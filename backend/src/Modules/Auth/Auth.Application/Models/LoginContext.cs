using Shared.Security.Contracts;

namespace Auth.Application.Models;

public sealed class LoginContext : UserEvent, ILoginEvent
{
    public string IpAddress { get; init; } = default!;
    public string UserAgent { get; init; } = default!;
    public string Device { get; init; } = default!;
    public DateTime LoginTime { get; init; }
    public bool IsSuspicious { get; init; }
}
