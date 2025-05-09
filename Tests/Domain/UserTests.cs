using Domain.Model;

namespace Tests.Domain;

public class UserTests
{
    [Fact]
    public void ShouldCreateUserWithActiveRecoveryCodes()
    {
        var user = new User("testuser@niepodam.pl");
        
        Assert.Equal(4, user.ActiveRecoveryCodes.Count);
    }

    [Fact]
    public void ShouldCreateUserWithoutOtpCode()
    {
        var user = new User("testuser@niepodam.pl");
        
        Assert.Null(user.ActiveOtpCode);
    }

    [Fact]
    public void ShouldReplaceUsedRecoveryCodes()
    {
        var user = new User("testuser@niepodam.pl");

        var recoveryCode = user.ActiveRecoveryCodes.First();
        
        user.UseRecoveryCode(recoveryCode.Code);
        
        Assert.Null(user.ActiveRecoveryCodes.FirstOrDefault(r => r.Code == recoveryCode.Code));
        Assert.Equal(4, user.ActiveRecoveryCodes.Count);
    }
}