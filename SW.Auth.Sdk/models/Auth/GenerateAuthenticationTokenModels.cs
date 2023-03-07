namespace SW.Auth.Sdk.models.Auth;

public class GenerateAuthenticationTokenModel
{
    public string Email { get; set; }
}

public class GenerateAuthenticationTokenResponseModel
{
    public string Token { get; set; }
    public string ExpiersOn { get; set; }

}