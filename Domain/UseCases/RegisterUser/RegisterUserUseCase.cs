using System.Data;
using Domain.Model;
using Domain.Repositories;
using Domain.Services.OtpService;
using Microsoft.Extensions.Logging;

namespace Domain.UseCases.RegisterUser;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    public record RegistrationRequest(string Email);
    
    private readonly IUserRepository _userRepository;
    private readonly ILogger<RegisterUserUseCase> _logger;
    private readonly IOtpService _otpService;

    public RegisterUserUseCase(IUserRepository userRepository, ILogger<RegisterUserUseCase> logger, IOtpService otpService)
    {
        _userRepository = userRepository;
        _logger = logger;
        _otpService = otpService;
    }

    public async Task RegisterUser(string email)
    {
        var existingUser = await _userRepository.FindUserByEmailAsync(email);
        
        if (existingUser is not null)
        {
            _logger.LogDebug("Found existing user: {User}", existingUser);
            throw new ConstraintException("Email already exists");
        }
        
        _logger.LogDebug("Could not find user for email: {Email}", email);

        var createdUser = new User(email);
        
        _logger.LogDebug("Created user with email: {Email}", createdUser.Email);

        await _userRepository.AddUserAsync(createdUser);
        await _userRepository.SaveChangesAsync();

        await _otpService.SendOtpCodeToUser(createdUser.Email);
    }
}