namespace Shared.Security.Contracts;

public interface IEmailVerifiedEvent : IUserEvent
{
    string Email { get; }
}
