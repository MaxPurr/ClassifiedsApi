using Microsoft.OpenApi.Models;

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
}