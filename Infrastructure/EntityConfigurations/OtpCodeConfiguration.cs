using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

public class OtpCodeConfiguration : IEntityTypeConfiguration<OtpCode>
{
    public void Configure(EntityTypeBuilder<OtpCode> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Code).IsRequired();
        builder.Property(u => u.ExpiresAt).ValueGeneratedOnAdd().IsRequired();
        builder.Property(u => u.CreatedAt).ValueGeneratedOnAdd().IsRequired();

        builder.HasOne(o => o.User).WithOne(u => u.ActiveOtpCode).HasForeignKey<OtpCode>(o => o.UserId).IsRequired();
    }
}