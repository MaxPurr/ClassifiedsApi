using ClassifiedsApi.AppServices.Exceptions.Common;

namespace ClassifiedsApi.AppServices.Exceptions.Users;

/// <summary>
/// Исключение, возникающее когда доступа к запрашиваемому комментарию запрещен.
/// </summary>
public class CommentAccessDeniedException : ResourceAccessDeniedException
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CommentAccessDeniedException"/>.
    /// </summary>
    public CommentAccessDeniedException() : base("Доступ к запрашиваемому комментарию запрещен.")
    {
        
    }
}