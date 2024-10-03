using AutoMapper;
using ClassifiedsApi.AppServices.Contexts.Accounts.Repositories;
using ClassifiedsApi.AppServices.Contexts.Accounts.Services;
using ClassifiedsApi.AppServices.Contexts.Files.Repositories;
using ClassifiedsApi.AppServices.Contexts.Files.Services;
using ClassifiedsApi.ComponentRegistrar.MapProfiles;
using ClassifiedsApi.Contracts.Contexts.Accounts;
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
        
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IJwtService, JwtService>();
        
        return services;
    }

    private static MapperConfiguration GetMapperConfiguration()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<FileProfile>();
            cfg.AddProfile<AccountProfile>();
        });
        configuration.AssertConfigurationIsValid();
        return configuration;
    }
}