using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Shared.Infrastructure.Extensions;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder EnableSerilogLogging(this DbContextOptionsBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder.LogTo(Log.Information);
    }
}
