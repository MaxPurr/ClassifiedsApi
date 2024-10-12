using ClassifiedsApi.AppServices.Exceptions.Common;

namespace ClassifiedsApi.AppServices.Exceptions.Comments;

/// <summary>
/// Исключение, возникающее когда искомый комментарий не был найден.
/// </summary>
public class CommentNotFoundException : EntityNotFoundException
{
    /// <summary>
    /// Инициализирует экземпляр <see cref="CommentNotFoundException"/>.
    /// </summary>
    public CommentNotFoundException() : base("Комментарий не был найден.")
    {
        
    }
}