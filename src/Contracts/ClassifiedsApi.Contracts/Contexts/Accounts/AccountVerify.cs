namespace ClassifiedsApi.Contracts.Contexts.Accounts;

/// <summary>
/// Модель для проверки учетных данных аккаунта.
/// </summary>
public class AccountVerify
{
    /// <summary>
    /// Логин.
    /// </summary>
    public string? Login { get; set; }
    
    /// <summary>
    /// Пароль.
    /// </summary>
    public string? Password { get; set; }
}