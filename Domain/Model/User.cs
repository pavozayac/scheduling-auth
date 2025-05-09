namespace Domain.Model;
 public class User : BaseEntity
{
    private const int RecoveryCodePoolSize = 4;

    public string Email { get; private set; }
    public bool IsRegistered { get; private set; }
    public ICollection<RecoveryCode> ActiveRecoveryCodes { get; }
    public OtpCode? ActiveOtpCode { get; private set; }

    private User()
    {
    }

    public User(string email)
    {
        Email = email;
        IsRegistered = false;
        ActiveRecoveryCodes = GenerateNewRecoveryCodes(Id);
        ActiveOtpCode = null;
    }

    private static ICollection<RecoveryCode> GenerateNewRecoveryCodes(Guid userId)
    {
        return Enumerable.Range(0, RecoveryCodePoolSize).Select(_ => new RecoveryCode(userId)).ToList();
    }

    public void GenerateNewOtpCode()
    {
        ActiveOtpCode = new OtpCode(this.Id);
    }

    public void UseOtpCode(string otpCode)
    {
        if (ActiveOtpCode is null)
        {
            throw new InvalidDataException("User has no associated OTP code");
        }

        if (ActiveOtpCode.Code != otpCode)
        {
            throw new InvalidDataException("Invalid OTP code");
        }

        if (ActiveOtpCode.IsExpired)
        {
            throw new InvalidDataException("OTP code has expired");
        }

        ActiveOtpCode = null;
    }

    public void CompleteRegistration(string otpCode)
    {
        if (IsRegistered)
        {
            throw new InvalidDataException("User is already registered");
        }
        
        UseOtpCode(otpCode);
        IsRegistered = true;
    }

    public void UseRecoveryCode(string code)
    {
        var rCode = ActiveRecoveryCodes.FirstOrDefault(x => x.Code == code) ??
                    throw new InvalidDataException(
                        "Could not find recovery code associated with this user.");

        ActiveRecoveryCodes.Remove(rCode);

        var newCode = new RecoveryCode(Id);

        ActiveRecoveryCodes.Add(newCode);
    }

    public void ResetEmail(string newEmail, string recoveryCode)
    {
        UseRecoveryCode(recoveryCode);

        Email = newEmail;

        // User needs to complete registration once more after email reset
        IsRegistered = false;
        GenerateNewOtpCode();
    }
}