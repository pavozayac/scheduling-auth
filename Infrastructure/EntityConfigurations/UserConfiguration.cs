using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Email).IsRequired();
        builder.Property(u => u.CreatedAt).ValueGeneratedOnAdd().IsRequired();

        builder.HasMany(u => u.ActiveRecoveryCodes)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .HasPrincipalKey(r => r.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.ActiveOtpCode)
            .WithOne(o => o.User)
            .HasForeignKey<OtpCode>(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
    }
}