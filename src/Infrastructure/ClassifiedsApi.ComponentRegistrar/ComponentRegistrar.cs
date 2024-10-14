using System;
using System.Reflection;
using AutoMapper;
using ClassifiedsApi.AppServices.Common.Validators;
using ClassifiedsApi.AppServices.Contexts.Accounts.Repositories;
using ClassifiedsApi.AppServices.Contexts.Accounts.Services;
using ClassifiedsApi.AppServices.Contexts.AdvertImages.Repositories;
using ClassifiedsApi.AppServices.Contexts.AdvertImages.Services;
using ClassifiedsApi.AppServices.Contexts.Adverts.Builders;
using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.AppServices.Contexts.Adverts.Services;
using ClassifiedsApi.AppServices.Contexts.Categories.Builders;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.AppServices.Contexts.Categories.Services;
using ClassifiedsApi.AppServices.Contexts.Categories.Validators;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Services;
using ClassifiedsApi.AppServices.Contexts.Comments.Builders;
using ClassifiedsApi.AppServices.Contexts.Comments.Repositories;
using ClassifiedsApi.AppServices.Contexts.Comments.Services;
using ClassifiedsApi.AppServices.Contexts.Files.Repositories;
using ClassifiedsApi.AppServices.Contexts.Files.Services;
using ClassifiedsApi.AppServices.Contexts.Users.Repositories;
using ClassifiedsApi.AppServices.Contexts.Users.Services;
using ClassifiedsApi.ComponentRegistrar.MapProfiles;
using ClassifiedsApi.DataAccess.Repositories;
using ClassifiedsApi.Infrastructure.Repository.Sql;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ClassifiedsApi.ComponentRegistrar;

public static class ComponentRegistrar
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(ISqlRepository<,>), typeof(SqlRepository<,>));
        services.AddSingleton(TimeProvider.System);
        
        services.AddSingleton<IMapper>(provider => new Mapper(GetMapperConfiguration(provider)));

        services.AddScoped<ICategorySpecificationBuilder, CategorySpecificationBuilder>();
        services.AddScoped<IAdvertSpecificationBuilder, AdvertSpecificationBuilder>();
        services.AddScoped<ICommentSpecificationBuilder, CommentSpecificationBuilder>();
        services.AddValidatorsFromAssemblyContaining<CategoryCreateValidator>(filter: filter =>
            filter.ValidatorType.GetCustomAttribute<IgnoreAutomaticRegistrationAttribute>() == null
        );
        
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IAdvertRepository, AdvertRepository>();
        services.AddScoped<ICharacteristicRepository, CharacteristicRepository>();
        services.AddScoped<IAdvertImageRepository, AdvertImageRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAdvertService, AdvertService>();
        services.AddScoped<ICharacteristicService, CharacteristicService>();
        services.AddScoped<IAdvertImageService, AdvertImageService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IUserService, UserService>();
        
        services.AddScoped<IUserAccessVerifier, UserAccessVerifier>();
        services.AddScoped<ICharacteristicVerifier, CharacteristicVerifier>();
        services.AddScoped<IAdvertVerifier, AdvertVerifier>();
        services.AddScoped<ICommentVerifier, CommentVerifier>();
        services.AddScoped<IFileVerifier, FileVerifier>();
        services.AddScoped<IUserVerifier, UserVerifier>();
        services.AddScoped<IAccountVerifier, AccountVerifier>();
        
        return services;
    }

    private static MapperConfiguration GetMapperConfiguration(IServiceProvider serviceProvider)
    {
        var timeProvider = serviceProvider.GetRequiredService<TimeProvider>();
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AccountProfile(timeProvider));
            cfg.AddProfile(new CategoryProfile(timeProvider));
            cfg.AddProfile(new AdvertProfile(timeProvider));
            cfg.AddProfile(new CharacteristicProfile(timeProvider));
            cfg.AddProfile(new CommentProfile(timeProvider));
            cfg.AddProfile(new FileProfile(timeProvider));
            cfg.AddProfile<UserProfile>();
        });
        configuration.AssertConfigurationIsValid();
        return configuration;
    }
}