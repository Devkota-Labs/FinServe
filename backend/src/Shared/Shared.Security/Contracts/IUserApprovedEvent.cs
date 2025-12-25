namespace Shared.Security.Contracts;

public interface IUserApprovedEvent : IUserEvent
{
    Uri LoginUrl { get; }
}