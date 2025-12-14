using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.EfConfig;

public class EmailVerificationTokenEfConfig : IEntityTypeConfiguration<EmailVerificationToken>
{
    public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("EmailVerificationTokens");
        builder.HasKey(x => x.Id);
    }
}
