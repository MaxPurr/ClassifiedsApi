using ClassifiedsApi.Contracts.Contexts.Accounts;
using ClassifiedsApi.Contracts.Contexts.Files;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace ClassifiedsApi.Api.Extensions;

/// <summary>
/// Расширения класса <see cref="ConfigureHostBuilder"/>.
/// </summary>
public static class ConfigureHostBuilderExtensions
{
    /// <summary>
    /// Добавляет сконфигурированный Serilog.
    /// </summary>
    /// <param name="hostBuilder"></param>
    public static void UseConfiguredSerilog(this ConfigureHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, _, config) =>
        {
            config.ReadFrom.Configuration(context.Configuration)
                .Enrich.WithEnvironmentName();
            config.Destructure.ByTransforming<FileUpload>(
                file => new { file.Name, file.ContentType, file.Length });
            config.Destructure.ByTransforming<FileDownload>(
                file => new { file.Name, file.ContentType});
            config.Destructure.ByTransforming<AccountRegister>(
                account => new
                {
                    account.Login, account.FirstName, account.LastName, account.Email, account.Phone, account.BirthDate
                });
            config.Destructure.ByTransforming<AccountVerify>(
                account => new { account.Login });
        });
    }
}