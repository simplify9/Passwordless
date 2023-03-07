using SW.Auth.Web.Services;
using SW.PrimitiveTypes;

namespace SW.Auth.Web.Domain.Auth;

public class AuthenticationToken : BaseEntity<Guid>
{
    private AuthenticationToken()
    {
    }

    public AuthenticationToken(Guid accountId)
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        CreatedOn = DateTime.UtcNow;
        SetRandomPassword();
    }

    public string Password { get; private set; }
    public Guid AccountId { get; set; }
    public Account.Account Account { get; set; }

    public DateTime CreatedOn { get; set; }

    public (bool, string) Validate(string password, int loginTokenExpirySpanInSeconds)
    {
        if ((DateTime.UtcNow - CreatedOn).Seconds > loginTokenExpirySpanInSeconds)
            return (false, "Your authentication token has expired");

        if (!CryptoUtil.Verify(password, Password))
            return (false, "Invalid token password");

        return (true, "Success");
    }

    private void SetRandomPassword()
    {
        // this is a mock for testing, should be a random 8 chars string; 
        Password = CryptoUtil.Hash("92N13");
    }
}