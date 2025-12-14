using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.EfConfig;

public class LoginHistoryEfConfig : IEntityTypeConfiguration<LoginHistory>
{
    public void Configure(EntityTypeBuilder<LoginHistory> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("LoginHistories");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.SessionId).IsUnique();
        builder.HasIndex(x => new { x.UserId, x.LoginTime });
        builder.Property(x => x.LogoutTime).IsRequired(false);
    }
}
