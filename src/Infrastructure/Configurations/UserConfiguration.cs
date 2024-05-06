using Domain.ValueObjects;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(user => user.FirstName)
            .HasMaxLength(200)
            .HasConversion(firstName => firstName.Value, value => new Name(value));

        builder.Property(user => user.LastName)
            .HasMaxLength(200)
            .HasConversion(lastName => lastName.Value, value => new Name(value));

        builder.Property(user => user.PrefferedCurrency)
            .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
    }
}
