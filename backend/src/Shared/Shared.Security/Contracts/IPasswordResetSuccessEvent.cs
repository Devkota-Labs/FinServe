namespace Shared.Security.Contracts;

public interface IPasswordResetSuccessEvent : IUserEvent
{
    string Timestamp { get; }
}
