using System;
using System.Linq.Expressions;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Adverts;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Specifications;

/// <summary>
/// Спецификация для фильтрации по минимальной цене объявления.
/// </summary>
public class MinPriceSpecification : Specification<ShortAdvertInfo>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="MinPriceSpecification"/>.
    /// </summary>
    /// <param name="minPrice">Минимальная цена.</param>
    public MinPriceSpecification(decimal minPrice)
    {
        PredicateExpression = advert => advert.Price >= minPrice;
    }
    
    /// <inheritdoc />
    public override Expression<Func<ShortAdvertInfo, bool>> PredicateExpression { get; }
}