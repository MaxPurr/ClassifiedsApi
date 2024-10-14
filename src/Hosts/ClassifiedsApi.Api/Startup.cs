using ClassifiedsApi.Api.Extensions;
using ClassifiedsApi.Api.Middlewares;
using ClassifiedsApi.AppServices.Settings;
using ClassifiedsApi.ComponentRegistrar;
using ClassifiedsApi.DataAccess.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ClassifiedsApi.Api;

public class Startup {
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddConfiguredAuthentication(_configuration);
        services.AddAuthorization();
        services.AddHttpContextAccessor();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddConfiguredSwaggerGen(_configuration);
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = _configuration.GetConnectionString("ApplicationDbConnectionString");
            options.UseNpgsql(connectionString);
        });
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = _configuration.GetConnectionString("RedisConnectionString");
            options.InstanceName = _configuration["RedisInstanceName"];
        });
        services.Configure<JwtSettings>(_configuration.GetSection(nameof(JwtSettings)));
        services.AddApplicationServices();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        { 
            endpoints.MapControllers();
        });
    }
}