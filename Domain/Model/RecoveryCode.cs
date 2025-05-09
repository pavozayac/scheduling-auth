using System.Text;

namespace Domain.Model;

public sealed class RecoveryCode : BaseValueObject
{
    private const int RecoveryCodeLength = 16;
    public string Code { get; }
    public bool IsActive { get; }
    
    public Guid UserId { get; }
    public User User { get; private set; } = null!;
    
    private RecoveryCode() {}

    public RecoveryCode(string code, Guid userId, bool isActive)
    {
        Code = code;
        UserId = userId;
        IsActive = isActive;
    }
    
    public RecoveryCode(Guid userId) : this(generateRecoveryCode(), userId, false) { }

    private static string generateRecoveryCode()
    {
        var random = new Random();
        
        var capitals = Enumerable.Range('A', 26).Select(x => (char)x);
        var lowercase = Enumerable.Range('a', 26).Select(x => (char)x);
        var alphabet = capitals.Concat(lowercase).ToArray();

        var code = random.GetItems(alphabet, RecoveryCodeLength).Aggregate("", (a, b) => a + b);

        return code;
    }
}