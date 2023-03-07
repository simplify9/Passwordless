using Microsoft.EntityFrameworkCore;
using SW.Auth.Sdk.models.Accounts;
using SW.Auth.Web.Domain.Account;
using SW.Auth.Web.Extensions;
using SW.PrimitiveTypes;

namespace SW.Auth.Web.Resources.Accounts;

[HandlerName("update")]
public class Update : ICommandHandler<UpdateAccountModel>
{
    private readonly SwDbContext _db;
    private readonly RequestContext _requestContext;

    public Update(SwDbContext db, RequestContext requestContext)
    {
        _db = db;
        _requestContext = requestContext;
    }

    public async Task<object> Handle(UpdateAccountModel request)
    {
        var accountId = _requestContext.User.GetAccountId();
        var account = await _db.Set<Account>()
            .FirstOrDefaultAsync(i=>i.Id==accountId);

        account!.Update(request.FirstName, request.LastName, request.DateOfBirth, request.AccountInfo);

        await _db.SaveChangesAsync();

        return null;
    }
}