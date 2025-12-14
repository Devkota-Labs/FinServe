using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.EfConfig;

public class MobileVerificationTokenEfConfig : IEntityTypeConfiguration<MobileVerificationToken>
{
    public void Configure(EntityTypeBuilder<MobileVerificationToken> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("MobileVerificationTokens");
        builder.HasKey(x => x.Id);
    }
}
