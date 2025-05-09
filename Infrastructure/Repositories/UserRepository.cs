using System.ComponentModel;
using Domain.Model;
using Domain.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserContext _context;

    public UserRepository(UserContext context)
    {
        _context = context;
    }

    public async Task<User?> FindUserAsync(Guid userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.ActiveOtpCode)
            .Include(u => u.ActiveRecoveryCodes)
            .SingleOrDefaultAsync();
        
        return user;
    }

    public async Task<User?> FindUserByEmailAsync(string email)
    {
        var user = await _context.Users
            .Where(u => string.Equals(u.Email, email))
            .Include(u => u.ActiveOtpCode)
            .Include(u => u.ActiveRecoveryCodes)
            .SingleOrDefaultAsync();

        return user;
    }

    public async Task<User?> FindUserByRecoveryCodeAsync(string code)
    {
        var user = await _context.Users
            .Where(u => u.ActiveRecoveryCodes.Any(r => r.Code == code))
            .Include(u => u.ActiveOtpCode)
            .Include(u => u.ActiveRecoveryCodes)
            .SingleOrDefaultAsync();

        return user;
    }

    public async Task<User> AddUserAsync(User user)
    {
        var addedUser = await _context.Users.AddAsync(user);
        return addedUser.Entity;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}