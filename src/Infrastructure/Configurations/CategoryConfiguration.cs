using Domain.Shared;
using Domain.Categories;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
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
                .HasForeignKey(fund => fund.UserId);
        }
    }
}
