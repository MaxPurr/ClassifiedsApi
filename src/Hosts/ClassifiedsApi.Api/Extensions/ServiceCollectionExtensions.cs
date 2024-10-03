using System;
using System.IO;
using ClassifiedsApi.Api.Controllers;
using ClassifiedsApi.Api.Settings;
using ClassifiedsApi.AppServices.Helpers;
using ClassifiedsApi.AppServices.Settings;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ClassifiedsApi.Api.Extensions;

/// <summary>
/// Расширения коллекции служб.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет сконфигурированную аутентификацию.
    /// </summary>
    /// <param name="services">Коллекция служб <see cref="IServiceCollection"/>.</param>
    /// <param name="configuration">Конфигурация <see cref="IConfiguration"/>.</param>
    /// <returns>Коллекция служб <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddConfiguredAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()!;
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = CryptoHelper.GetSymmetricSecurityKey(jwtSettings.SigningKey)
                };
            });
        return services;
    }

    /// <summary>
    /// Добавляет генерацию Swagger с комментариями.
    /// </summary>
    /// <param name="services">Коллекция служб <see cref="IServiceCollection"/>.</param>
    /// <param name="configuration">Конфигурация <see cref="IConfiguration"/>.</param>
    /// <returns>Коллекция служб <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddConfiguredSwaggerGen(this IServiceCollection services, IConfiguration configuration)
    {
        var docSettings = configuration.GetRequiredSection(nameof(ApiDocumentationSettings)).Get<ApiDocumentationSettings>();
        return services.AddSwaggerGen(options =>
        {
            ConfigureXmlComments(options, docSettings!);
            ConfigureSecurity(options);
        });
    }

    private static void ConfigureXmlComments(SwaggerGenOptions options, ApiDocumentationSettings docSettings)
    {
        options.SwaggerDoc(docSettings.Version, new OpenApiInfo
        {
            Title = docSettings.Title,
            Version = docSettings.Version
        });
        var docTypeMarkers = new Type[]
        {
            typeof(AdvertInfo),
            typeof(FileController)
        };
        foreach (var marker in docTypeMarkers)
        {
            var xmlFileName = $"{marker.Assembly.GetName().Name}.xml";
            var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
            if (File.Exists(xmlFilePath))
            {
                options.IncludeXmlComments(xmlFilePath);
            }
        }
    }

    private static void ConfigureSecurity(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Авторизация",
            Description = "Пожалуйста введите токен.",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer",
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[]{}
            }
        });
    }
    
    /// <summary>
    /// Добавляет GridFS в колекцию служб.
    /// </summary>
    /// <param name="services">Коллекция служб <see cref="IServiceCollection"/>.</param>
    /// <returns>Коллекция служб <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddGridFsBucket(this IServiceCollection services)
    {
        services.AddSingleton<IGridFSBucket>(provider =>
        {
            var mongoDbSettings = provider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            var database = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
            return new GridFSBucket(database);
        });
        return services;
    }
}