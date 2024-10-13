namespace ClassifiedsApi.Contracts.Contexts.Users;

/// <summary>
/// Модель информации о пользователе.
/// </summary>
public class UserInfo
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
    /// Имя.
    /// </summary>
    public string FirstName { get; set; } = "";

    /// <summary>
    /// Фамилия.
    /// </summary>
    public string LastName { get; set; } = "";
    
    /// <summary>
    /// Дата рождения.
    /// </summary>
    public DateTime BirthDate { get; set; }
    
    /// <summary>
    /// Идентификатор фотографии профиля.
    /// </summary>
    public string? PhotoId { get; set; }
}