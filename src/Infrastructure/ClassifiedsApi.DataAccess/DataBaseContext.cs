using ClassifiedsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.DataAccess;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Advert> Adverts { get; set; } = null!;
    public DbSet<UserFavoriteAdvert> UserFavoriteAdverts { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<File> Files { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataBaseContext).Assembly);
    }
}
