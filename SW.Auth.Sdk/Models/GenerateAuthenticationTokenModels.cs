namespace SW.Auth.Sdk.Models;

public class GenerateAuthenticationTokenModel
{
    public string Phone { get; set; }
}

public class GenerateAuthenticationTokenResponseModel
{
    public string Token { get; set; }
    public string ExpiersOn { get; set; }

}