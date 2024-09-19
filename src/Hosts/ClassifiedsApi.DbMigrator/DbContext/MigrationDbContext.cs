using ClassifiedsApi.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.DbMigrator.DbContext;

public class MigrationDbContext : DataBaseContext
{
    public MigrationDbContext(DbContextOptions options) : base(options)
    {
        
    }
}