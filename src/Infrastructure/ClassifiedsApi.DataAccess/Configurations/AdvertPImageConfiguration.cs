using ClassifiedsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassifiedsApi.DataAccess.Configurations;

public class AdvertPImageConfiguration : IEntityTypeConfiguration<AdvertImage>
{
    public void Configure(EntityTypeBuilder<AdvertImage> builder)
    {
        builder.ToTable("AdvertPhoto").HasKey(advertPhoto => advertPhoto.Id);
        builder.Property(advertPhoto => advertPhoto.CreatedAt).IsRequired();
        builder.Property(advertPhoto => advertPhoto.ImageId).IsRequired();
    }
}