using Domain.UseCases.RegisterUser;

namespace Domain.UseCases.RetrieveUser;

public interface IRetrieveUserUseCase
{
   Task<RetrieveUserResultDto> RetrieveUser(string email);
}