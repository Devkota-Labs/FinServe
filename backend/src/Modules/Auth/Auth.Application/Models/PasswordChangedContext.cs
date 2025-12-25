using Shared.Security.Contracts;

namespace Auth.Application.Models;

public sealed class PasswordChangedContext : UserEvent, IPasswordChangedEvent
{
    public string Timestamp { get; init; } = default!;
}
