using System;
using System.Linq.Expressions;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Adverts;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Specifications;

/// <summary>
/// Спецификация для фильтрации по максимальной цене объявления.
/// </summary>
public class MaxPriceSpecification : Specification<ShortAdvertInfo>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="MaxPriceSpecification"/>.
    /// </summary>
    /// <param name="maxPrice">Максимальная цена.</param>
    public MaxPriceSpecification(decimal maxPrice)
    {
        PredicateExpression = advert => advert.Price <= maxPrice;
    }
    
    /// <inheritdoc />
    public override Expression<Func<ShortAdvertInfo, bool>> PredicateExpression { get; }
}