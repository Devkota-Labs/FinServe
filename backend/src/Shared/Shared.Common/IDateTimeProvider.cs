namespace Shared.Common;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
