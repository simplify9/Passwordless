using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SW.Auth.Sdk.Models;
using SW.Auth.Web.Domain;
using SW.Auth.Web.Extensions;
using SW.HttpExtensions;
using SW.PrimitiveTypes;

namespace SW.Auth.Web.Resources.Accounts;

[HandlerName(nameof(Login))]
[Unprotect]
public class Login : ICommandHandler<LoginRequestModel>
{
    private readonly SwDbContext _db;
    private readonly InstanceSettings _instanceSettings;
    private readonly JwtTokenParameters _jwtTokenParameters;

    public Login(SwDbContext db, InstanceSettings instanceSettings, JwtTokenParameters jwtTokenParameters)
    {
        _db = db;
        _instanceSettings = instanceSettings;
        _jwtTokenParameters = jwtTokenParameters;
    }

    public async Task<object> Handle(LoginRequestModel request)
    {
        var token = await _db.Set<AuthenticationToken>()
            .Include(i => i.Account)
            .FirstOrDefaultAsync(i => i.Id.Equals(request.Token));

        if (token is null)
            throw new SWUnauthorizedException("Invalid token");

        var (isValid, reason) = token.Validate(request.Password, _instanceSettings.AccessTokenExpirySpanInSeconds);
        if (!isValid)
            throw new SWUnauthorizedException(reason);


        token.Account.LoggedIn();
        var refreshToken = new RefreshToken(token.AccountId);
        _db.Add(refreshToken);
        await _db.SaveChangesAsync();


        return new LoginResponseModel
        {
            Type = "Bearer",
            AccessToken = token.Account.CreateJwt(_jwtTokenParameters,
                TimeSpan.FromSeconds(_instanceSettings.AccessTokenExpirySpanInSeconds)),
            RefreshToken = refreshToken.Id.ToString()
        };
    }

    private class Validate : AbstractValidator<LoginRequestModel>
    {
        public Validate()
        {
            RuleFor(i => i.Password).NotEmpty();
            RuleFor(i => i.Token).NotEmpty();
        }
    }
}