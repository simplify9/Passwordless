using SW.PrimitiveTypes;

namespace SW.Auth.Web.Domain;

public class RefreshToken : BaseEntity<Guid>
{
    private RefreshToken()
    {
    }

    public RefreshToken(Guid accountId)
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        CreatedOn = DateTime.UtcNow;
    }

    public Guid AccountId { get; set; }
    public Account Account { get; set; }
    public DateTime CreatedOn { get; set; }
}