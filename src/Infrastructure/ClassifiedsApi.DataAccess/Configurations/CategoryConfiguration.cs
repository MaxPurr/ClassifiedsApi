using ClassifiedsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassifiedsApi.DataAccess.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories").HasKey(category => category.Id);
        builder.Property(category => category.CreatedAt).IsRequired();
        builder.Property(category => category.Name).HasMaxLength(255).IsRequired();
        builder.HasMany(category => category.ChildCategories)
            .WithOne(category => category.ParentCategory)
            .HasForeignKey(category => category.ParentId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(category => category.Adverts)
            .WithOne(advert => advert.Category)
            .HasForeignKey(advert => advert.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
