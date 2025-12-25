using Shared.Security.Contracts;

namespace Admin.Application.Models;

public sealed class UserUnlockedContext : UserEvent, IUserUnlockedEvent
{
    public Uri LoginUrl { get; init; } = default!;
}