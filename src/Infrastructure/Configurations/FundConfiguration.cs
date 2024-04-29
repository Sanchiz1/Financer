using Domain.Currencies;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.FundAggregate;

namespace Infrastructure.Configurations;

internal sealed class FundConfiguration : IEntityTypeConfiguration<Fund>
{
    public void Configure(EntityTypeBuilder<Fund> builder)
    {
        builder.ToTable("Funds");

        builder.HasIndex(fund => fund.Id);

        builder.Property(fund => fund.Name)
            .HasMaxLength(200)
            .HasConversion(name => name.Value, value => new Name(value));

        builder.Property(fund => fund.Description)
            .HasMaxLength(200)
            .HasConversion(description => description.Value, value => new Description(value));

        builder.Property(fund => fund.Currency)
            .HasConversion(currency => currency.Code, code => Currency.FromCode(code));

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(fund => fund.UserId);
    }
}