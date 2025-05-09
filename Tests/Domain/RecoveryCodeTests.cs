using Domain.Model;

namespace Tests.Domain;

public class RecoveryCodeTests
{
    [Fact]
    public void ShouldHaveLowercaseAndUppercase()
    {
        var userId = Guid.NewGuid();
        var recoveryCode = new RecoveryCode(userId);
        
        Assert.Matches("^[A-Za-z]*$", recoveryCode.Code);
    }

    [Fact]
    public void ShouldHaveLengthSixteen()
    {
        var userId = Guid.NewGuid();
        var recoveryCode = new RecoveryCode(userId);
        
        Assert.Equal(16, recoveryCode.Code.Length);
    }
    
}