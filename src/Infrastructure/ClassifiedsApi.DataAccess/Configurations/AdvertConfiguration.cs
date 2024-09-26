using ClassifiedsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassifiedsApi.DataAccess.Configurations;

public class AdvertConfiguration :  IEntityTypeConfiguration<Advert>
{
    public void Configure(EntityTypeBuilder<Advert> builder)
    {
        builder.ToTable("Adverts").HasKey(advert => advert.Id);
        builder.Property(advert => advert.CreatedAt).IsRequired();
        builder.Property(advert => advert.Title).HasMaxLength(255).IsRequired();
        builder.Property(advert => advert.Description).HasMaxLength(1000).IsRequired();
        builder.Property(advert => advert.Price).HasColumnType("decimal(18,2)").IsRequired();
        builder.HasMany(advert => advert.Images)
            .WithOne(advertPhoto => advertPhoto.Advert)
            .HasForeignKey(advertPhoto => advertPhoto.AdvertId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(advert => advert.Comments)
            .WithOne(comment => comment.Advert)
            .HasForeignKey(comment => comment.AdvertId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
