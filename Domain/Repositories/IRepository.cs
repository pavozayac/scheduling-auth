namespace Domain.Repositories;

public interface IRepository
{
    Task SaveChangesAsync();
}