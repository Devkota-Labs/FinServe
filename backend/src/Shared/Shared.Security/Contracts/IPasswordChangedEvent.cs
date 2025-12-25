namespace Shared.Security.Contracts;

public interface IPasswordChangedEvent : IUserEvent
{
    string Timestamp { get; }
}
