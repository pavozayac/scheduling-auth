using Domain.Model;
using Domain.UseCases;

namespace Domain.UseCases.RegisterUser;

public interface IRegisterUserUseCase
{
    Task RegisterUser(string email);
    
}