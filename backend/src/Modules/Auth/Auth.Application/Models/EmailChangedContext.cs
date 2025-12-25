using Shared.Security.Contracts;

namespace Auth.Application.Models;

public sealed class EmailChangedContext : UserEvent, IEmailChangedEvent
{
    public string OldEmail { get; init; } = default!;
    public string NewEmail { get; init; } = default!;
}