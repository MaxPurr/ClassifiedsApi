using ClassifiedsApi.Api.Extensions;
using ClassifiedsApi.Api.Middlewares;
using ClassifiedsApi.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.Api;

public class Startup {
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSwaggerGenWithComments();
        services.AddDbContext<DataBaseContext>(options =>
        {
            var connectionString = _configuration.GetConnectionString("ConnectionString");
            options.UseNpgsql(connectionString);
        });
        services.AddControllers();
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