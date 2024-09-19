using ClassifiedsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassifiedsApi.DataAccess.Configurations;

public class AdvertPhotoConfiguration : IEntityTypeConfiguration<AdvertPhoto>
{
    public void Configure(EntityTypeBuilder<AdvertPhoto> builder)
    {
        builder.ToTable("AdvertPhoto").HasKey(advertPhoto => advertPhoto.Id);
        builder.Property(advertPhoto => advertPhoto.CreatedAt).IsRequired();
        builder.Property(advertPhoto => advertPhoto.PhotoId).IsRequired();
    }
}