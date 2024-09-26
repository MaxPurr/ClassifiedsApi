using ClassifiedsApi.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.DbMigrator.DbContexts;

public class MigrationDbContext : ApplicationDbContext
{
    public MigrationDbContext(DbContextOptions options) : base(options)
    {
        
    }
}