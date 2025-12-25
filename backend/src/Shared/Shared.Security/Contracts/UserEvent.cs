namespace Shared.Security.Contracts;

public abstract class UserEvent : IUserEvent
{
    public int UserId { get; init; }
    public string UserName { get; init; } = default!;
}

