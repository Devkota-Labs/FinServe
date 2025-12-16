using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.EfConfig;

public class PasswordHistoryEfConfig : IEntityTypeConfiguration<PasswordHistory>
{
    public void Configure(EntityTypeBuilder<PasswordHistory> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("PasswordHistories");
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.UserId);
    }
}
