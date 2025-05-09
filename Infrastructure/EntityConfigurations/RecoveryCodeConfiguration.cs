using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

public class RecoveryCodeConfiguration : IEntityTypeConfiguration<RecoveryCode>
{
    public void Configure(EntityTypeBuilder<RecoveryCode> builder)
    {
        builder.HasKey(r => r.Code);
        builder.Property(r => r.IsActive).IsRequired();
        builder.Property(r => r.CreatedAt).ValueGeneratedOnAdd().IsRequired();
        
        builder.HasOne(r => r.User).WithMany(u => u.ActiveRecoveryCodes).HasForeignKey(r => r.UserId).IsRequired();
    }
}