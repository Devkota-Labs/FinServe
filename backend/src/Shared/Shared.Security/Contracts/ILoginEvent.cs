namespace Shared.Security.Contracts;

public interface ILoginEvent
{
    int UserId { get; }
    string UserName { get; }
    string IpAddress { get; }
    string UserAgent { get; }
    string Device { get; }
    DateTime LoginTime { get; }
    bool IsSuspicious { get; }
}
