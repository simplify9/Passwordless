using SW.Auth.Web.Resources.Accounts;
using SW.PrimitiveTypes;

namespace SW.Auth.Web.Domain;

public class Account : BaseEntity<Guid>, IAudited
{
    private Account()
    {
    }


    public Account(string phone)
    {
        Id = Guid.NewGuid();
        AccountInfo = new Dictionary<string, string>();
        Phone = phone;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public Dictionary<string, string> AccountInfo { get; set; }
    public DateTime LastLogin { get; set; }

    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string ModifiedBy { get; set; }

    public void LoggedIn()
    {
        LastLogin = DateTime.UtcNow;
    }

    public void Update(string firstName, string lastName, DateTime? dateOfBirth, Dictionary<string, string> info)
    {
        FirstName = firstName;
        LastName = lastName;
        AccountInfo = info;
        if (dateOfBirth is not null)
        {
            DateOfBirth = dateOfBirth.Value.Date;
        }
    }
}