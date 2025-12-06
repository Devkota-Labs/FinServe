using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Infrastructure.Database.Auditing;
using Shared.Infrastructure.Database.Conventions;

namespace Shared.Infrastructure.Database;

public abstract class BaseDbContext : DbContext
{
    private readonly IHttpContextAccessor? _accessor;
    internal string Name { get; }
    // This will be set by DI during runtime (not migrations)
    internal string CurrentUser => _accessor?.HttpContext?.User?.Identity?.Name ?? "System";

    protected BaseDbContext(DbContextOptions options, IHttpContextAccessor accessor)
        : this(options)
    {
        _accessor = accessor;
        //CurrentUser = accessor.HttpContext?.User?.Identity?.Name;
    }

    // Design-time constructor (for migrations)
    protected BaseDbContext(DbContextOptions options)
        : base(options)
    {
        Name = GetType().Name;

        Log.Information("{0} initialized.", Name);
    }

    /// <summary>
    /// Apply common table name prefixing such as tbl_<module>_<entity>.
    /// Each module passes its own prefix.
    /// </summary>
    protected static void ApplyModuleTablePrefix(ModelBuilder modelBuilder, string modulePrefix)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var currentTable = entity.GetTableName();

            if (string.IsNullOrWhiteSpace(currentTable))
                continue;

            // Rename to tbl_<module>_<table>
            entity.SetTableName($"tbl_{modulePrefix}_{currentTable!}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        Log.Information("{0} OnModelCreating started.", Name);

        base.OnModelCreating(modelBuilder);

        // Apply shared conventions
        modelBuilder.UseSnakeCaseNames();    // optional
    }

    /// <summary>
    /// Apply auditing changes (CreatedBy, UpdatedBy)
    /// </summary>
    public override int SaveChanges()
    {
        this.ApplyAuditInformation();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.ApplyAuditInformation();

        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
