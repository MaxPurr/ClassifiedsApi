using ClassifiedsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassifiedsApi.DataAccess.Configurations;

public class AdvertImageConfiguration : IEntityTypeConfiguration<AdvertImage>
{
    public void Configure(EntityTypeBuilder<AdvertImage> builder)
    {
        builder.ToTable("AdvertImages").HasKey(advertImage => new {advertImage.ImageId, advertImage.AdvertId});
    }
}