using Domain.Entities.TransactionAggregate;
using Domain.ValueObjects;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasIndex(transaction => transaction.Id);

        builder.Property(transaction => transaction.Description)
            .HasMaxLength(200)
            .HasConversion(description => description.Value, value => new Description(value));

        builder.OwnsOne(transaction => transaction.Amount, amountBuilder =>
        {
            amountBuilder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
        });

        builder.HasOne<TransactionCategory>()
            .WithMany()
            .HasForeignKey(transaction => transaction.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}