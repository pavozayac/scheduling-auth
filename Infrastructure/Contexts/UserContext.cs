using System.Reflection;
using Domain.Model;
using Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<OtpCode> OtpCodes { get; set; }
    public DbSet<RecoveryCode> RecoveryCodes { get; set; }
    
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new OtpCodeConfiguration());
        modelBuilder.ApplyConfiguration(new RecoveryCodeConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}