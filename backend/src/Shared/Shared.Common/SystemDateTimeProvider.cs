using Shared.Common.Utils;

namespace Shared.Common;

internal sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTimeUtil.Now;
}
