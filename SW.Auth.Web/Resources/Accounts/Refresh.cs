using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SW.Auth.Sdk.Models;
using SW.Auth.Web.Domain;
using SW.Auth.Web.Extensions;
using SW.HttpExtensions;
using SW.PrimitiveTypes;

namespace SW.Auth.Web.Resources;

[Unprotect]
[HandlerName(nameof(Refresh))]
public class Refresh : ICommandHandler<RefreshRequestModel>
{
    private readonly SwDbContext _db;
    private readonly InstanceSettings _instanceSettings;
    private readonly JwtTokenParameters _jwtTokenParameters;

    public Refresh(SwDbContext db, InstanceSettings instanceSettings, JwtTokenParameters jwtTokenParameters)
    {
        _db = db;
        _instanceSettings = instanceSettings;
        _jwtTokenParameters = jwtTokenParameters;
    }

    public async Task<object> Handle(RefreshRequestModel request)
    {
        var token = await _db.Set<RefreshToken>()
            .Include(i => i.Account)
            .FirstOrDefaultAsync(i => i.Id.Equals(request.RefreshToken));


        if (token is null)
            throw new SWUnauthorizedException("Invalid token");

        if ((DateTime.UtcNow - token.CreatedOn).TotalDays > _instanceSettings.RefreshTokenExpirySpanInDays)
            return (false, "Your authentication token has expired");


        var refreshToken = new RefreshToken(token.AccountId);
        _db.Add(refreshToken);
        _db.Remove(token);
        await _db.SaveChangesAsync();
        return new LoginResponseModel
        {
            Type = "Bearer",
            AccessToken = token.Account.CreateJwt(_jwtTokenParameters,
                TimeSpan.FromSeconds(_instanceSettings.AccessTokenExpirySpanInSeconds)),
            RefreshToken = refreshToken.Id.ToString()
        };
    }

    private class Validate : AbstractValidator<RefreshRequestModel>
    {
        public Validate()
        {
            RuleFor(i => i.RefreshToken).NotEmpty();
        }
    }
}