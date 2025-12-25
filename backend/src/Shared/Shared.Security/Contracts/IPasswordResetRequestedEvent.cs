namespace Shared.Security.Contracts;

public interface IPasswordResetRequestedEvent : IUserEvent
{
    Uri ResetLink { get; }
    int ExpiryTimeInMinutes { get; }
}
