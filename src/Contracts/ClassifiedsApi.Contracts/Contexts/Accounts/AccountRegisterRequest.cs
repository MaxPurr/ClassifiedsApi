namespace ClassifiedsApi.Contracts.Contexts.Accounts;

/// <summary>
/// Запрос на регистрацию аккаунта.
/// </summary>
public class AccountRegisterRequest
{
    /// <summary>
    /// Логин.
    /// </summary>
    public required string Login { get; set; }
    
    /// <summary>
    /// Хэш пароля.
    /// </summary>
    public required string PasswordHash { get; set; }
    
    /// <summary>
    /// Имя.
    /// </summary>
    public required string FirstName { get; set; }
    
    /// <summary>
    /// Фамилия.
    /// </summary>
    public required string LastName { get; set; }
    
    /// <summary>
    /// Адрес электронной почты.
    /// </summary>
    public required string Email { get; set; }
    
    /// <summary>
    /// Номер телефона.
    /// </summary>
    public required string? Phone { get; set; }
    
    /// <summary>
    /// Дата рождения.
    /// </summary>
    public required DateTime BirthDate { get; set; }
}