using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SW.Auth.Web.Domain;
using SW.Auth.Web.Domain.Account;

namespace SW.Auth.Web.DataConfig;

public class ContactAddressConfig : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasMaxLength(32);
        builder.HasIndex(i => i.CreatedOn);
        builder.HasIndex(i => i.NormalizedEmail).IsUnique();
        builder.Property(i => i.AccountInfo).HasColumnType("jsonb");
        builder.ToTable("accounts");
    }
}