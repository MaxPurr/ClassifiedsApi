using Microsoft.EntityFrameworkCore;
using ClassifiedsApi.DbMigrator.DbContexts;

namespace ClassifiedsApi.DbMigrator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.ConfigureDbConnection(configuration);
    }

    private static IServiceCollection ConfigureDbConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ApplicationDbConnectionString");
        services.AddDbContext<MigrationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        return services;
    }
}