namespace SW.Auth.Web;

public class InstanceSettings
{
    public int RefreshTokenExpirySpanInDays { get; set; } = 21;
    public int LoginTokenExpirySpanInSeconds { get; set; } = 120;
    public int AccessTokenExpirySpanInSeconds { get; set; } = 600;
}