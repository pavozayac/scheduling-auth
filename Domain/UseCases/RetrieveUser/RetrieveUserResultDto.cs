using Domain.Model;

namespace Domain.UseCases.RegisterUser;

public record RetrieveUserResultDto(
    Guid Id,
    string Email,
    bool IsRegistered,
    DateTime CreatedAt,
    ICollection<RecoveryCodeDto> RecoveryCodes,
    OtpCodeDto? OtpCode);

public record RecoveryCodeDto(string Code);

public record OtpCodeDto(string Code);