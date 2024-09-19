using ClassifiedsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassifiedsApi.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users").HasKey(user => user.Id);
        builder.Property(user => user.CreatedAt).IsRequired();
        builder.Property(user => user.FirstName).HasMaxLength(255).IsRequired();
        builder.Property(user => user.LastName).HasMaxLength(255).IsRequired();
        builder.Property(user => user.Login).HasMaxLength(255).IsRequired();
        builder.Property(user => user.Email).HasMaxLength(320).IsRequired();
        builder.Property(user => user.IsEmailVerified).IsRequired();
        builder.Property(user => user.BirthDate).IsRequired();
        builder.HasMany(user => user.Adverts)
            .WithOne(advert => advert.User)
            .HasForeignKey(advert => advert.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(user => user.LikedAdverts)
            .WithMany(advert => advert.LikedUsers)
            .UsingEntity<UserFavoriteAdvert>(
                ConfigureAdvertRelationshipToUserFavoriteAdvert,
                ConfigureUserRelationshipToUserFavoriteAdvert,
                ConfigureUserFavoriteAdvert);
    }
    
    private static ReferenceCollectionBuilder<Advert, UserFavoriteAdvert> ConfigureAdvertRelationshipToUserFavoriteAdvert(EntityTypeBuilder<UserFavoriteAdvert> builder)
    {
        return builder
            .HasOne(userFavoriteAdvert => userFavoriteAdvert.Advert)
            .WithMany(user => user.UserFavoriteAdverts)
            .HasForeignKey(userFavoriteAdvert => userFavoriteAdvert.AdvertId)
            .IsRequired();
    }
    
    private static ReferenceCollectionBuilder<User, UserFavoriteAdvert> ConfigureUserRelationshipToUserFavoriteAdvert(EntityTypeBuilder<UserFavoriteAdvert> builder)
    {
        return builder
            .HasOne(userFavoriteAdvert => userFavoriteAdvert.User)
            .WithMany(user => user.UserFavoriteAdverts)
            .HasForeignKey(userFavoriteAdvert => userFavoriteAdvert.UserId)
            .IsRequired();
    }

    private static void ConfigureUserFavoriteAdvert(EntityTypeBuilder<UserFavoriteAdvert> builder)
    {
        builder.ToTable("UserFavoriteAdverts").HasKey(userFavoriteAdvert => userFavoriteAdvert.Id);
        builder.Property(user => user.CreatedAt).IsRequired();
    }
}
