using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SW.Auth.Web.Domain;

namespace SW.Auth.Web.DataConfig;

public class RefreshTokenDataConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(b => b.Id);
        builder.HasIndex(i => i.AccountId);
        builder.HasIndex(i => i.CreatedOn);
        builder.HasOne(i => i.Account).WithMany().HasForeignKey(i => i.AccountId);
        builder.ToTable("refresh_tokens");

    }
}