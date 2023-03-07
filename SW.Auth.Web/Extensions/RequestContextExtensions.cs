using System.Security.Claims;
using SW.PrimitiveTypes;

namespace SW.Auth.Web.Extensions;

public static class RequestContextExtensions
{
    public static Guid GetAccountId(this ClaimsPrincipal claimsPrincipal)
    {
        var accountClaim = claimsPrincipal.FindFirst(i => i.Type.Contains(ClaimTypes.NameIdentifier))?.Value;
        return Guid.Parse(accountClaim);
    }
}