namespace ClassifiedsApi.Contracts.Contexts.Accounts;

/// <summary>
/// Модель информации об аккаунте.
/// </summary>
public class AccountInfo
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Логин.
    /// </summary>
    public string Login { get; set; } = "";
    
    /// <summary>
    /// Массив названий ролей.
    /// </summary>
    public string[] RoleNames { get; set; } = null!;
}