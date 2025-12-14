using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Database;

namespace Auth.Infrastructure.Db;

internal sealed class AuthDbContext(DbContextOptions<AuthDbContext> options) : BaseDbContext(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<PasswordHistory> PasswordHistories { get; set; } = null!;
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; } = null!;
    public DbSet<LoginHistory> LoginHistory { get; set; }
    public DbSet<MobileVerificationToken> MobileVerificationTokens { get; set; }
    public DbSet<OtpVerification> OtpVerifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var module = nameof(Auth);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);

        // ⭐ Auto-prefix all table names
        ApplyModuleTablePrefix(modelBuilder, module);
    }
}
