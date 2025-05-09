using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Domain.UseCases.CompleteUserRegistration;

public class CompleteUserRegistrationUseCase : ICompleteUserRegistrationUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger _logger;

    public CompleteUserRegistrationUseCase(IUserRepository userRepository,
        ILogger<CompleteUserRegistrationUseCase> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<RegistrationCompletionResult> CompleteRegistration(string userEmail, string otpCode)
    {
        var user = await _userRepository.FindUserByEmailAsync(userEmail);

        if (user is null)
        {
            throw new InvalidDataException("User not found");
        }

        try
        {
            user.CompleteRegistration(otpCode);
            await _userRepository.SaveChangesAsync();

            _logger.LogDebug("User used OTP code {@OtpUsedLog}", new
            {
                userEmail = user.Email,
                otpCode = user.ActiveOtpCode?.Code,
                outcome = "Success",
            });

            return RegistrationCompletionResult.RegistrationSuccess;
        }
        catch (InvalidDataException e)
        {
            _logger.LogDebug(e, "User failed to use OTP code {@OtpUsedLog}", new
            {
                userEmail = user.Email,
                otpCode = user.ActiveOtpCode?.Code,
                outcome = "Failure",
            });

            return RegistrationCompletionResult.RegistrationFailure;
        }
    }
}