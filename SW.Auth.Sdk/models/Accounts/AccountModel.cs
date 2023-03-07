using System.Data.Common;

namespace SW.Auth.Sdk.models.Accounts;

public class AccountModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime LastLogin { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public decimal CompletionPercentage { get; set; }

    public Dictionary<string, string> AccountInfo { get; set; }
}