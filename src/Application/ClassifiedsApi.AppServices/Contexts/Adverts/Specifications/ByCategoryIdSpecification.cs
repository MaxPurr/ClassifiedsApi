using System;
using System.Linq.Expressions;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Adverts;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Specifications;

/// <summary>
/// Спецификация для фильтрации по идентификатору категории объявления.
/// </summary>
public class ByCategoryIdSpecification : Specification<ShortAdvertInfo>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="ByCategoryIdSpecification"/>.
    /// </summary>
    /// <param name="categoryId">Идентификатор категории.</param>
    public ByCategoryIdSpecification(Guid categoryId)
    {
        PredicateExpression = advert => advert.CategoryId == categoryId;
    }
    
    /// <inheritdoc />
    public override Expression<Func<ShortAdvertInfo, bool>> PredicateExpression { get; }
}