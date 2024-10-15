using ClassifiedsApi.AppServices.Exceptions.Common;

namespace ClassifiedsApi.AppServices.Exceptions.Categories;

/// <summary>
/// Исключение, возникающее когда искомая категория не была найдена.
/// </summary>
public class CategoryNotFoundException : EntityNotFoundException
{
    /// <summary>
    /// Инициализирует экземпляр <see cref="CategoryNotFoundException"/>.
    /// </summary>
    public CategoryNotFoundException() : this("Категория не была найдена.")
    {
        
    }
    
    /// <summary>
    /// Инициализирует экземпляр <see cref="CategoryNotFoundException"/>.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public CategoryNotFoundException(string message) : base(message)
    {
        
    }
}