using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SW.Auth.Web.Domain.Auth;

namespace SW.Auth.Web.DataConfig;

public class AuthenticationTokenDataConfig : IEntityTypeConfiguration<AuthenticationToken>
{
    public void Configure(EntityTypeBuilder<AuthenticationToken> builder)
    {
        builder.HasKey(b => b.Id);
        builder.HasIndex(i => i.AccountId);
        builder.HasIndex(i => i.CreatedOn);
        builder.HasOne(i => i.Account).WithMany().HasForeignKey(i => i.AccountId);
        builder.ToTable("authentication_tokens");

    }
}