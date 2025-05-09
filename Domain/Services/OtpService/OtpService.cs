using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Domain.Services.OtpService;

public class OtpService : IOtpService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<OtpService> _logger;

    public OtpService(IUserRepository userRepository, ILogger<OtpService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task SendOtpCodeToUser(string userEmail)
    {
        var user = await _userRepository.FindUserByEmailAsync(userEmail);

        if (user is null)
        {
            throw new InvalidDataException("Cannot send OTP: User does not exist");
        }

        user.GenerateNewOtpCode();

        await _userRepository.SaveChangesAsync();

        _logger.LogDebug("OTP code sent to user {@OtpSendLog}", new
        {
            userEmail = user.Email,
            otpCode = user.ActiveOtpCode?.Code,
        });
    }
}