using System.Security.Claims;
using SW.Auth.Web.Domain.Account;
using SW.HttpExtensions;

namespace SW.Auth.Web.Extensions;

public static class UserExtensions
{
    public static string CreateJwt(this Account account, JwtTokenParameters jwtTokenParameters,
        TimeSpan jwtExpiry = default)
    {
        return jwtExpiry == default
            ? jwtTokenParameters.WriteJwt(CreateClaimsIdentity(account))
            : jwtTokenParameters.WriteJwt(CreateClaimsIdentity(account), jwtExpiry);
    }


    private static ClaimsIdentity CreateClaimsIdentity(this Account account)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim("phone", account.Phone),
        };

        return new ClaimsIdentity(claims, "Auth");
    }
}