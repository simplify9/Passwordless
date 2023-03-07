using Microsoft.EntityFrameworkCore;
using SW.Auth.Web.Domain.Account;
using SW.Auth.Web.Extensions;
using SW.PrimitiveTypes;

namespace SW.Auth.Web.Resources.Accounts;

[HandlerName("me")]
public class Me : IQueryHandler
{
    private readonly SwDbContext _db;
    private readonly RequestContext _requestContext;

    public Me(SwDbContext db, RequestContext requestContext)
    {
        _db = db;
        _requestContext = requestContext;
    }

    public async Task<object> Handle()
    {
        var accountId = _requestContext.User.GetAccountId();
        var account = await _db.Set<Account>()
            .FirstOrDefaultAsync(i => i.Id == accountId);

        return account.MapToModel();
    }
}