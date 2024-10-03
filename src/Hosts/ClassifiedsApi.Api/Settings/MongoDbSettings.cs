namespace ClassifiedsApi.Api.Settings;

/// <summary>
/// Параметры подключения к MongoDB.
/// </summary>
public class MongoDbSettings
{
    /// <summary>
    /// Строка подключения.
    /// </summary>
    public string ConnectionString { get; set; } = "";
    
    /// <summary>
    /// Название базы данных.
    /// </summary>
    public string DatabaseName { get; set; } = "";
}