using ClassifiedsApi.Api.Extensions;
using ClassifiedsApi.Api.Middlewares;
using ClassifiedsApi.AppServices.Settings;
using ClassifiedsApi.ComponentRegistrar;
using ClassifiedsApi.DataAccess.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClassifiedsApi.Api;

public static class Program {
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services, builder.Configuration);

        builder.Host.UseConfiguredSerilog();
        
        var app = builder.Build();
        Configure(app);
        
        app.Run();
    }
    
    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfiguredAuthentication(configuration);
        services.AddAuthorization();
        services.AddHttpContextAccessor();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddConfiguredSwaggerGen(configuration);
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("ApplicationDbConnectionString");
            options.UseNpgsql(connectionString);
        });
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnectionString");
            options.InstanceName = configuration["RedisInstanceName"];
        });
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.AddApplicationServices();
    }
    
    private static void Configure(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}