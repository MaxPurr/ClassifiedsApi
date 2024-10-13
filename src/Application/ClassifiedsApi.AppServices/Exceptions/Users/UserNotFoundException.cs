using ClassifiedsApi.AppServices.Exceptions.Common;

namespace ClassifiedsApi.AppServices.Exceptions.Users;

/// <summary>
/// Исключение, возникающее когда искомый пользователь не был найден.
/// </summary>
public class UserNotFoundException : EntityNotFoundException
{
    /// <summary>
    /// Инициализирует экземпляр <see cref="UserNotFoundException"/>.
    /// </summary>
    public UserNotFoundException() : base("Пользователь не был найден.")
    {
        
    }
}