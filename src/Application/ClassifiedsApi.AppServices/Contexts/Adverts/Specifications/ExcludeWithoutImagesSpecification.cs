using System;
using System.Linq.Expressions;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Adverts;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Specifications;

/// <summary>
/// Спецификация для фильтрации объявлений без фотографий.
/// </summary>
public class ExcludeWithoutImagesSpecification : Specification<ShortAdvertInfo>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="ExcludeWithoutImagesSpecification"/>.
    /// </summary>
    public ExcludeWithoutImagesSpecification()
    {
        PredicateExpression = advert => advert.ImageIds.Count > 0;
    }
    
    /// <inheritdoc />
    public override Expression<Func<ShortAdvertInfo, bool>> PredicateExpression { get; }
}