namespace SW.Auth.Sdk.Models;

public class LoginRequestModel
{
    public Guid Token { get; set; }
    public string Password { get; set; }
}

public class LoginResponseModel
{
    public string AccessToken { get; set; }
    public string Type { get; set; }
    public string RefreshToken { get; set; }
}