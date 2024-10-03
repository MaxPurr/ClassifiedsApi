namespace ClassifiedsApi.Api.Settings;

/// <summary>
/// Параметры документации API.
/// </summary>
public class ApiDocumentationSettings
{
    /// <summary>
    /// Название.
    /// </summary>
    public string Title { get; set; } = "";

    /// <summary>
    /// Версия.
    /// </summary>
    public string Version { get; set; } = "";
}