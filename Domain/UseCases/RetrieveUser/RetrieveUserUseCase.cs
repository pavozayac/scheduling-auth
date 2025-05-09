using System.Data;
using System.Diagnostics;
using Domain.Repositories;
using Domain.UseCases.RegisterUser;
using Microsoft.Extensions.Logging;

namespace Domain.UseCases.RetrieveUser;

public class RetrieveUserUseCase : IRetrieveUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<RetrieveUserUseCase> _logger;

    public RetrieveUserUseCase(IUserRepository userRepository, ILogger<RetrieveUserUseCase> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<RetrieveUserResultDto> RetrieveUser(string email)
    {
        var user = await _userRepository.FindUserByEmailAsync(email);

        if (user is null)
        {
            _logger.LogDebug("User could not be found {@UserNotFoundLog}", new
            {
                email,
            });
            throw new InvalidOperationException("User not found");
        }

        var mappedRecoveryCodes = user.ActiveRecoveryCodes.Select(r => new RecoveryCodeDto(r.Code)).ToList();
        var mappedOtpCode = user.ActiveOtpCode is null ? null : new OtpCodeDto(user.ActiveOtpCode.Code);
        
        return new RetrieveUserResultDto(user.Id, user.Email, user.IsRegistered, user.CreatedAt, mappedRecoveryCodes, mappedOtpCode);
    }
}