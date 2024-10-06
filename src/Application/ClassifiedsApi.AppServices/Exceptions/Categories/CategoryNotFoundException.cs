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
    public CategoryNotFoundException() : base("Категория не была найдена.")
    {
        
    }
}