using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Categories;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Builders;

/// <summary>
/// Строитель спецификаций для категорий.
/// </summary>
public interface ICategorySpecificationBuilder
{
    /// <summary>
    /// Строит спецификацию по модели поиска.
    /// </summary>
    /// <param name="categoriesSearch">Модель поиска категорий <see cref="CategoriesSearch"/>.</param>
    /// <returns>Спецификация.</returns>
    ISpecification<CategoryInfo> Build(CategoriesSearch categoriesSearch);
}