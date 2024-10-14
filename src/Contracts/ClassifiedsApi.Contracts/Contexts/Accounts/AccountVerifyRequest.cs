namespace ClassifiedsApi.Contracts.Contexts.Accounts;

/// <summary>
/// Запрос на проверку учетных данных аккаунта.
/// </summary>
public class AccountVerifyRequest
{
    /// <summary>
    /// Логин.
    /// </summary>
    public required string Login { get; set; }
    
    /// <summary>
    /// Хэш пароля.
    /// </summary>
    public required string PasswordHash { get; set; }
}