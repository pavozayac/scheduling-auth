using Domain.Model;

namespace Domain.Repositories;

public interface IUserRepository : IRepository
{
    Task<User> AddUserAsync(User user);
    Task<User?> FindUserAsync(Guid userId);
    Task<User?> FindUserByRecoveryCodeAsync(string code);
    Task<User?> FindUserByEmailAsync(string email);
}