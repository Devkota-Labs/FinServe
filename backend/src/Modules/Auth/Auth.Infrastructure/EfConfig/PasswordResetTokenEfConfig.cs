using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.EfConfig;

public class PasswordResetTokenEfConfig : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("PasswordResetTokens");
        builder.HasKey(x => x.Id);
        //builder.HasOne(x => x.User)
        //    .WithMany()
        //    .HasForeignKey(x => x.UserId)
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}
