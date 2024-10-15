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
    public CommentNotFoundException() : this("Комментарий не был найден.")
    {
        
    }

    /// <summary>
    /// Инициализирует экземпляр <see cref="CommentNotFoundException"/>.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public CommentNotFoundException(string message) : base(message)
    {
        
    }
}