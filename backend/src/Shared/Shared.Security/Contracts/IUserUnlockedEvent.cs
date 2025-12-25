namespace Shared.Security.Contracts;

public interface IUserUnlockedEvent : IUserEvent
{
    Uri LoginUrl { get; }
}