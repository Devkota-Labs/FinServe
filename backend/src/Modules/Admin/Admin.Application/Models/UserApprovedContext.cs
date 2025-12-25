using Shared.Security.Contracts;

namespace Admin.Application.Models;

public sealed class UserApprovedContext : UserEvent, IUserApprovedEvent
{
    public Uri LoginUrl { get; init; } = default!;
}
