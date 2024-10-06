using System;
using AutoMapper;
using ClassifiedsApi.AppServices.Contexts.Accounts.Repositories;
using ClassifiedsApi.AppServices.Contexts.Accounts.Services;
using ClassifiedsApi.AppServices.Contexts.Categories.Builders;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.AppServices.Contexts.Categories.Services;
using ClassifiedsApi.AppServices.Contexts.Categories.Validators;
using ClassifiedsApi.AppServices.Contexts.Files.Repositories;
using ClassifiedsApi.AppServices.Contexts.Files.Services;
using ClassifiedsApi.ComponentRegistrar.MapProfiles;
using ClassifiedsApi.DataAccess.Repositories;
using ClassifiedsApi.Infrastructure.Repository.GridFs;
using ClassifiedsApi.Infrastructure.Repository.Sql;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ClassifiedsApi.ComponentRegistrar;

public static class ComponentRegistrar
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(ISqlRepository<,>), typeof(SqlRepository<,>));
        services.AddScoped<IGridFsRepository, GridFsRepository>();
        services.AddSingleton(TimeProvider.System);
        
        services.AddSingleton<IMapper>(provider => new Mapper(GetMapperConfiguration(provider)));

        services.AddScoped<ICategorySpecificationBuilder, CategorySpecificationBuilder>();
        services.AddValidatorsFromAssemblyContaining<CategoryCreateValidator>();
        
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ICategoryService, CategoryService>();
        
        return services;
    }

    private static MapperConfiguration GetMapperConfiguration(IServiceProvider serviceProvider)
    {
        var timeProvider = serviceProvider.GetRequiredService<TimeProvider>();
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<FileProfile>();
            cfg.AddProfile(new AccountProfile(timeProvider));
            cfg.AddProfile(new CategoryProfile(timeProvider));
        });
        configuration.AssertConfigurationIsValid();
        return configuration;
    }
}