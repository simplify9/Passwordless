using SW.Auth.Sdk.Models;
using SW.Auth.Web.Domain;

namespace SW.Auth.Web.Extensions;

public static class Mappers
{
    public static AccountModel MapToModel(this Account account)
    {
        return new AccountModel
        {
            Id = account.Id,
            Phone = account.Phone,
            AccountInfo = account.AccountInfo,
            ModifiedOn = account.ModifiedOn,
            CreatedOn = account.CreatedOn,
            LastLogin = account.LastLogin,
            FirstName = account.FirstName,
            LastName = account.LastName,
            DateOfBirth = account.DateOfBirth,
            CompletionPercentage = ComputeCompletionPercentage(account)
        };
    }

    private static decimal ComputeCompletionPercentage(Account account)
    {
        var percentage = 20;

        if (!string.IsNullOrWhiteSpace(account.FirstName))
        {
            percentage += 15;
        }

        if (!string.IsNullOrWhiteSpace(account.LastName))
        {
            percentage += 15;
        }

        if (account.DateOfBirth is not null)
        {
            percentage += 25;
        }

        if (account.AccountInfo?.Keys?.Count > 0)
        {
            percentage += 25;
        }

        return percentage;
    }
}