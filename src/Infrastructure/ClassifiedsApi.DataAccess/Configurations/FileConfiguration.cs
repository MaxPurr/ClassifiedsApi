using ClassifiedsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassifiedsApi.DataAccess.Configurations;

public class FileConfiguration : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.ToTable("Files").HasKey(file => file.Id);
        builder.Property(file => file.CreatedAt).IsRequired();
        builder.Property(file => file.Name).HasMaxLength(256).IsRequired();
        builder.Property(file => file.ContentType).HasMaxLength(256).IsRequired();
        builder.Property(file => file.Content).IsRequired();
        builder.Property(file => file.Length).IsRequired();
    }
}