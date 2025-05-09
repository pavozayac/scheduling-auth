using Domain.UseCases.RegisterUser;

namespace Application.Handlers;

public record UserRetrievedResponse(
    Guid Id, 
    string Email, 
    bool IsRegistered, 
    DateTime CreatedAt,
    ICollection<RecoveryCodeDto> RecoveryCodes,
    OtpCodeDto? OtpCode);