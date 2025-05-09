using System.Runtime.CompilerServices;
using System.Text;

namespace Domain.Model;

public sealed class OtpCode : BaseEntity
{
    public const int OtpExpirationTimeMinutes = 5;
    public string Code { get; }
    public DateTime ExpiresAt { get; private set; }

    public Guid UserId { get; }
    public User User { get; private set; } = null!;
    
    public bool IsExpired => ExpiresAt < DateTime.UtcNow;
    
    private OtpCode() {}

    public OtpCode(Guid userId)
    {
        UserId = userId;
        Code = _generateOtpCodeString();
        ExpiresAt = DateTime.UtcNow.AddMinutes(OtpExpirationTimeMinutes);
    }

    public OtpCode(Guid userId, string code, DateTime expiresAt)
    {
        this.UserId = userId;
        this.Code = code;
        this.ExpiresAt = expiresAt;
    }

    private static string _generateOtpCodeString()
    {
        StringBuilder codeBuilder = new();

        for (int i = 0; i < 6; i++)
        {
            int digit = Random.Shared.Next(10);
            codeBuilder.Append(digit);
        }

        return codeBuilder.ToString();
    }
}