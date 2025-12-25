namespace Shared.Security.Contracts;

public interface IEmailChangedEvent
{
    int UserId { get; }
    string UserName { get; }
    string OldEmail { get; }
    string NewEmail { get; }
}