using ClassifiedsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassifiedsApi.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(user => user.CreatedAt).IsRequired();
        builder.Property(user => user.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(user => user.LastName).HasMaxLength(50).IsRequired();
        builder.Property(user => user.Login).HasMaxLength(50).IsRequired();
        builder.Property(user => user.Email).HasMaxLength(320).IsRequired();
        builder.Property(user => user.IsEmailVerified).IsRequired();
        builder.Property(user => user.BirthDate).IsRequired();
        builder.HasMany(user => user.Adverts)
            .WithOne(advert => advert.User)
            .HasForeignKey(advert => advert.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
