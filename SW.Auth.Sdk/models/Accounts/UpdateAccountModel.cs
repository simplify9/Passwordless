namespace SW.Auth.Sdk.models.Accounts;

public class UpdateAccountModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Dictionary<string, string> AccountInfo { get; set; }
}