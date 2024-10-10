namespace ClassifiedsApi.Contracts.Contexts.Categories;

/// <summary>
/// Типизированная модель запроса категории.
/// </summary>
/// <typeparam name="TModel">Тип модели запроса.</typeparam>
public class CategoryRequest<TModel> where TModel : class
{
    /// <summary>
    /// Идентификатор категории.
    /// </summary>
    public required Guid CategoryId { get; set; }
    
    /// <summary>
    /// Модель запроса.
    /// </summary>
    public required TModel Model { get; init; }
}