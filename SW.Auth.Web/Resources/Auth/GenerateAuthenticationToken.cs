using System.Globalization;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SW.Auth.Sdk.models.Auth;
using SW.Auth.Web.Domain.Account;
using SW.Auth.Web.Domain.Auth;
using SW.PrimitiveTypes;

namespace SW.Auth.Web.Resources.Auth;

[Unprotect]
[HandlerName("Generate")]
public class GenerateAuthenticationToken : ICommandHandler<GenerateAuthenticationTokenModel>
{
    private readonly SwDbContext _db;
    private readonly InstanceSettings _instanceSettings;

    public GenerateAuthenticationToken(SwDbContext db, InstanceSettings instanceSettings)
    {
        _db = db;
        _instanceSettings = instanceSettings;
    }

    public async Task<object> Handle(GenerateAuthenticationTokenModel request)
    {
        var account = await _db.Set<Account>()
            .FirstOrDefaultAsync(i => i.NormalizedEmail.Equals(request.Email.ToUpper()));

        if (account is null)
        {
            account = new Account(request.Email);
            _db.Add(account);
        }

        var token = new AuthenticationToken(account.Id);


        _db.Add(token);
        await _db.SaveChangesAsync();

        return new GenerateAuthenticationTokenResponseModel
        {
            Token = token.Id.ToString(),
            ExpiersOn = token.CreatedOn.AddSeconds(_instanceSettings.LoginTokenExpirySpanInSeconds)
                .ToString(CultureInfo.InvariantCulture)
        };
    }


    private class Validate : AbstractValidator<GenerateAuthenticationTokenModel>
    {
        public Validate()
        {
            RuleFor(i => i.Email).NotEmpty().EmailAddress();
        }
    }
}