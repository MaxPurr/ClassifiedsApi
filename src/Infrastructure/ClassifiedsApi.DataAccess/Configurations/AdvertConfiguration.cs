using ClassifiedsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassifiedsApi.DataAccess.Configurations;

public class AdvertConfiguration :  IEntityTypeConfiguration<Advert>
{
    public void Configure(EntityTypeBuilder<Advert> builder)
    {
        builder.ToTable("Adverts").HasKey(advert => advert.Id);
        builder.Property(user => user.CreatedAt).IsRequired();
        builder.Property(advert => advert.Title).HasMaxLength(100).IsRequired();
        builder.Property(advert => advert.Description).HasMaxLength(1000).IsRequired();
        builder.HasMany(advert => advert.Comments)
            .WithOne(comment => comment.Advert)
            .HasForeignKey(comment => comment.AdvertId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
