using Shared.Security.Contracts;

namespace Auth.Application.Models;

public sealed class PasswordResetRequestedContext : UserEvent, IPasswordResetRequestedEvent
{
    public Uri ResetLink { get; init; } = default!;
    public int ExpiryTimeInMinutes { get; init; }
}
