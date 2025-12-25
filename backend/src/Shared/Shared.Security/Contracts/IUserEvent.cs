namespace Shared.Security.Contracts;

public interface IUserEvent
{
    int UserId { get; }
    string UserName { get; }
}
