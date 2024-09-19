using ClassifiedsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassifiedsApi.DataAccess.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments").HasKey(comment => comment.Id);
        builder.Property(comment => comment.CreatedAt).IsRequired();
        builder.Property(comment => comment.Text).HasMaxLength(1000).IsRequired();
        builder.HasMany(comment => comment.ChildComments)
            .WithOne(comment => comment.ParentComment)
            .HasForeignKey(comment => comment.ParentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
