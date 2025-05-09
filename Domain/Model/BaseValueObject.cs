namespace Domain.Model;

public class BaseValueObject
{
    public DateTime CreatedAt { get; }

    public BaseValueObject()
    {
        CreatedAt = DateTime.UtcNow;
    }
}