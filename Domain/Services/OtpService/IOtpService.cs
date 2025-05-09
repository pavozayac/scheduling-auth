namespace Domain.Services.OtpService;

public interface IOtpService
{
    Task SendOtpCodeToUser(string userEmail);
}