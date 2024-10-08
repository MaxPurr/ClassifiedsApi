using ClassifiedsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassifiedsApi.DataAccess.Configurations;

public class CharacteristicConfiguration : IEntityTypeConfiguration<Characteristic>
{
    public void Configure(EntityTypeBuilder<Characteristic> builder)
    {
        builder.ToTable("Characteristics").HasKey(characteristic => new { characteristic.Id, characteristic.AdvertId });
        builder.Property(characteristic => characteristic.CreatedAt).IsRequired();
        builder.Property(characteristic => characteristic.Name).HasMaxLength(255);
        builder.HasAlternateKey(characteristic => new {characteristic.Name, characteristic.AdvertId});
        builder.Property(characteristic => characteristic.Value).HasMaxLength(1000).IsRequired();
    }
}