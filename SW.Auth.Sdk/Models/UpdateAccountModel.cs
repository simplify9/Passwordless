namespace SW.Auth.Sdk.Models;

public class UpdateAccountModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Dictionary<string, string> AccountInfo { get; set; }
}