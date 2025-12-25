using Shared.Security.Contracts;

namespace Auth.Application.Models;

public sealed class PasswordResetSuccessContext : UserEvent, IPasswordResetSuccessEvent
{
    public string Timestamp { get; init; } = default!;
}