using System;
using System.IO;
using ClassifiedsApi.Api.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace ClassifiedsApi.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerGenWithComments(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Classifieds API",
                Version = "v1"
            });
            var docTypeMarkers = new Type[]
            {
                
            };
            foreach (Type marker in docTypeMarkers)
            {
                string xmlFileName = $"{marker.Assembly.GetName().Name}.xml";
                string xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                if (File.Exists(xmlFilePath))
                {
                    options.IncludeXmlComments(xmlFilePath);
                }
            }
        });
        return services;
    }

    public static IServiceCollection AddGridFsBucket(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbSettings = configuration.GetRequiredSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
        string connectionString = mongoDbSettings!.ConnectionString;
        string databaseName = mongoDbSettings!.DatabaseName;
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase(databaseName);
        services.AddSingleton<IGridFSBucket>(_ => new GridFSBucket(database));
        return services;
    }
}