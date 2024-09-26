using ClassifiedsApi.DbMigrator.DbContexts;
using ClassifiedsApi.DbMigrator.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.DbMigrator;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddServices(hostContext.Configuration);
            });
        var host = builder.Build();
        await MigrateAsync(host.Services);
    }

    private static Task MigrateAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetService<MigrationDbContext>();
        return context!.Database.MigrateAsync();
    }
}