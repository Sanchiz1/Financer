using Domain.Entities.TransactionAggregate;
using Domain.ValueObjects;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.ValueObjects;
using Domain.AggregatesModel.TransactionAggregate;

namespace Infrastructure.Configurations;
internal sealed class CategoryConfiguration : IEntityTypeConfiguration<TransactionCategory>
{
    public void Configure(EntityTypeBuilder<TransactionCategory> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(category => category.Id);

        builder.Property(category => category.Name)
            .HasMaxLength(255)
            .HasConversion(name => name.Value, value => new Name(value));

        builder.Property(category => category.Description)
            .HasMaxLength(255)
            .HasConversion(description => description.Value, value => new Description(value));

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(category => category.UserId);
    }
}