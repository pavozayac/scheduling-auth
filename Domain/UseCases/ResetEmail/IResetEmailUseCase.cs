namespace Domain.UseCases.ResetEmail;

public interface IResetEmailUseCase
{
    Task ResetEmail(string oldEmail, string newEmail, string recoveryCode);
}