using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Domain.UseCases.ResetEmail;

public class ResetEmailUseCase : IResetEmailUseCase
{
    private readonly ILogger<ResetEmailUseCase> _logger;
    private readonly IUserRepository _userRepository;

    public ResetEmailUseCase(ILogger<ResetEmailUseCase> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task ResetEmail(string oldEmail, string newEmail, string recoveryCode)
    {
        var user = await _userRepository.FindUserByEmailAsync(oldEmail);

        if (user is null)
        {
            _logger.LogDebug("Cannot reset email: user with such email does not exist {@ResetLog}", new
            {
                OldEmail = oldEmail,
                NewEmail = newEmail,
                RecoveryCode = recoveryCode
            });
            return;
        }

        user.ResetEmail(newEmail, recoveryCode);

        await _userRepository.SaveChangesAsync();
    }
}