namespace ClassifiedsApi.Contracts.Contexts.Users;

/// <summary>
/// Модель информации о контактах пользователя.
/// </summary>
public class UserContactsInfo
{
    /// <summary>
    /// Адрес электронной почты.
    /// </summary>
    public string Email { get; set; } = "";

    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string? Phone { get; set; }
}