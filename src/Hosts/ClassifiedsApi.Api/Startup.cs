using ClassifiedsApi.Api.Extensions;
using ClassifiedsApi.Api.Middlewares;
using ClassifiedsApi.Api.Settings;
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
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGenWithComments();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = _configuration.GetConnectionString("ApplicationDbConnectionString");
            options.UseNpgsql(connectionString);
        });
        services.AddGridFsBucket(_configuration);
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
        app.UseEndpoints(endpoints =>
        { 
            endpoints.MapControllers();
        });
    }
}