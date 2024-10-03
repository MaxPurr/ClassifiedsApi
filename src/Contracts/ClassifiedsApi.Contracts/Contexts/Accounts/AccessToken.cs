namespace ClassifiedsApi.Contracts.Contexts.Accounts;

/// <summary>
/// Модель токена доступа.
/// </summary>
public class AccessToken
{
    /// <summary>
    /// Токен.
    /// </summary>
    public string Token { get; set; } = "";
    
    /// <summary>
    /// Тип токена.
    /// </summary>
    public string Type { get; set; } = "";
}