using Shared.Security.Contracts;

namespace Auth.Application.Models;

public sealed class EmailVerifiedContext : UserEvent, IEmailVerifiedEvent
{
    public string Email { get; init; } = default!;
}