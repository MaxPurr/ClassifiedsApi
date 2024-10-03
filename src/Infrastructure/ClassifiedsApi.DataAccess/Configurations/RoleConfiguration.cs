using ClassifiedsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassifiedsApi.DataAccess.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles").HasKey(role => role.Id);
        builder.Property(role => role.CreatedAt).IsRequired();
        builder.Property(role => role.Name).HasMaxLength(255).IsRequired();
    }
}