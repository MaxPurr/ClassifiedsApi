using AutoMapper;
using ClassifiedsApi.AppServices.Contexts.Files.Repositories;
using ClassifiedsApi.AppServices.Contexts.Files.Services;
using ClassifiedsApi.ComponentRegistrar.MapProfiles;
using ClassifiedsApi.DataAccess.Repositories;
using ClassifiedsApi.Infrastructure.Repository.GridFs;
using ClassifiedsApi.Infrastructure.Repository.Sql;
using Microsoft.Extensions.DependencyInjection;

namespace ClassifiedsApi.ComponentRegistrar;

public static class ComponentRegistrar
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(ISqlRepository<,>), typeof(SqlRepository<,>));
        services.AddScoped<IGridFsRepository, GridFsRepository>();
        
        services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));
        
        services.AddScoped<IFileRepository, FileRepository>();
        
        services.AddScoped<IFileService, FileService>();
        
        return services;
    }

    private static MapperConfiguration GetMapperConfiguration()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<FileProfile>();
        });
        configuration.AssertConfigurationIsValid();
        return configuration;
    }
}