namespace ClassifiedsApi.AppServices.Settings;

/// <summary>
/// Параметры генерации JWT.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Издатель токена.
    /// </summary>
    public string Issuer { get; set; } = "";
    
    /// <summary>
    /// Потребитель токена.
    /// </summary>
    public string Audience { get; set; } = "";
    
    /// <summary>
    /// Ключ для шифрации.
    /// </summary>
    public string SigningKey { get; set; } = "";
    
    /// <summary>
    /// Время действия токена в минутах.
    /// </summary>
    public int ValidityPeriodInMinutes { get; set; }
}