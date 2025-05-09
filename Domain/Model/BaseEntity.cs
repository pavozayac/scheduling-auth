namespace Domain.Model;

public class BaseEntity
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; }

    public BaseEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }
}