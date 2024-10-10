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
        builder.Property(user => user.EmailVerified).IsRequired();
        builder.Property(user => user.BirthDate).IsRequired();
        builder.HasMany(user => user.Roles)
            .WithMany(role => role.Users)
            .UsingEntity<UserRole>(
                ConfigureRoleRelationshipToUserRole,
                ConfigureUserRelationshipToUserRole,
                ConfigureUserRole
            );
        builder.HasMany(user => user.Adverts)
            .WithOne(advert => advert.User)
            .HasForeignKey(advert => advert.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(user => user.LikedAdverts)
            .WithMany(advert => advert.LikedUsers)
            .UsingEntity<UserFavoriteAdvert>(
                ConfigureAdvertRelationshipToUserFavoriteAdvert,
                ConfigureUserRelationshipToUserFavoriteAdvert,
                ConfigureUserFavoriteAdvert
            );
    }

    private static ReferenceCollectionBuilder<Role, UserRole> ConfigureRoleRelationshipToUserRole(
        EntityTypeBuilder<UserRole> builder)
    {
        return builder.HasOne(userRole => userRole.Role)
            .WithMany(role => role.UserRoles)
            .HasForeignKey(userRole => userRole.RoleId)
            .IsRequired();
    }
    
    private static ReferenceCollectionBuilder<User, UserRole> ConfigureUserRelationshipToUserRole(
        EntityTypeBuilder<UserRole> builder)
    {
        return builder.HasOne(userRole => userRole.User)
            .WithMany(user => user.UserRoles)
            .HasForeignKey(userRole => userRole.UserId)
            .IsRequired();
    }
    
    private static void ConfigureUserRole(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles").HasKey(userRole => userRole.Id);
        builder.Property(userRole => userRole.CreatedAt).IsRequired();
    }
    
    private static ReferenceCollectionBuilder<Advert, UserFavoriteAdvert> ConfigureAdvertRelationshipToUserFavoriteAdvert(
        EntityTypeBuilder<UserFavoriteAdvert> builder)
    {
        return builder.HasOne(userFavoriteAdvert => userFavoriteAdvert.Advert)
            .WithMany(advert => advert.UserFavoriteAdverts)
            .HasForeignKey(userFavoriteAdvert => userFavoriteAdvert.AdvertId)
            .IsRequired();
    }
    
    private static ReferenceCollectionBuilder<User, UserFavoriteAdvert> ConfigureUserRelationshipToUserFavoriteAdvert(
        EntityTypeBuilder<UserFavoriteAdvert> builder)
    {
        return builder.HasOne(userFavoriteAdvert => userFavoriteAdvert.User)
            .WithMany(user => user.UserFavoriteAdverts)
            .HasForeignKey(userFavoriteAdvert => userFavoriteAdvert.UserId)
            .IsRequired();
    }

    private static void ConfigureUserFavoriteAdvert(EntityTypeBuilder<UserFavoriteAdvert> builder)
    {
        builder.ToTable("UserFavoriteAdverts").HasKey(userFavoriteAdvert => userFavoriteAdvert.Id);
        builder.Property(userFavoriteAdvert => userFavoriteAdvert.CreatedAt).IsRequired();
    }
}
