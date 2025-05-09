using Domain.Services.OtpService;

namespace Domain.UseCases.CompleteUserRegistration;

public interface ICompleteUserRegistrationUseCase
{
    Task<RegistrationCompletionResult> CompleteRegistration(string userEmail, string otpCode);
}