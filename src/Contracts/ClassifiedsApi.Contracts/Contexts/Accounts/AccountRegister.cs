namespace ClassifiedsApi.Contracts.Contexts.Accounts;

/// <summary>
/// Модель регистрации нового аккаунта.
/// </summary>
public class AccountRegister
{
    /// <summary>
    /// Логин.
    /// </summary>
    public string Login { get; set; } = "";
    
    /// <summary>
    /// Пароль.
    /// </summary>
    public string Password { get; set; } = "";
    
    /// <summary>
    /// Имя.
    /// </summary>
    public string FirstName { get; set; } = "";
    
    /// <summary>
    /// Фамилия.
    /// </summary>
    public string LastName { get; set; } = "";
    
    /// <summary>
    /// Адрес электронной почты.
    /// </summary>
    public string Email { get; set; } = "";
    
    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// Дата рождения.
    /// </summary>
    public DateTime BirthDate { get; set; }
}