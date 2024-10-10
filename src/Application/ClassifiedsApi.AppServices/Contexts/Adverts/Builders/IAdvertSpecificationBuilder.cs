using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Adverts;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Builders;

/// <summary>
/// Строитель спецификаций для объявлений.
/// </summary>
public interface IAdvertSpecificationBuilder
{
    /// <summary>
    /// Строит спецификацию по модели поиска.
    /// </summary>
    /// <param name="search">Модель поиска объявлений <see cref="AdvertsSearch"/>.</param>
    /// <returns>Спецификация.</returns>
    ISpecification<ShortAdvertInfo> Build(AdvertsSearch search);
}