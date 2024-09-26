using System;
using System.IO;
using ClassifiedsApi.Api.Controllers;
using ClassifiedsApi.Api.Settings;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    private const string ApiTitle = "Classifieds API";
    private const string ApiVersion = "v1";
    
    /// <summary>
    /// Добавляет генерацию Swagger с комментариями.
    /// </summary>
    /// <param name="services">Коллекция служб <see cref="IServiceCollection"/>.</param>
    /// <returns>Коллекция служб <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddSwaggerGenWithComments(this IServiceCollection services)
    {
        return services.AddSwaggerGen(ConfigureXmlComments);
    }

    private static void ConfigureXmlComments(SwaggerGenOptions options)
    {
        options.SwaggerDoc(ApiVersion, new OpenApiInfo
        {
            Title = ApiTitle,
            Version = ApiVersion
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
            Console.WriteLine(xmlFilePath);
            if (File.Exists(xmlFilePath))
            {
                options.IncludeXmlComments(xmlFilePath);
            }
        }
    }
    
    /// <summary>
    /// Добавляет GridFS в колекцию служб.
    /// </summary>
    /// <param name="services">Коллекция служб <see cref="IServiceCollection"/>.</param>
    /// <returns>Коллекция служб <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddGridFsBucket(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbSettings = configuration.GetRequiredSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
        var connectionString = mongoDbSettings!.ConnectionString;
        var databaseName = mongoDbSettings!.DatabaseName;
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase(databaseName);
        services.AddSingleton<IGridFSBucket>(provider => new GridFSBucket(database));
        return services;
    }
}