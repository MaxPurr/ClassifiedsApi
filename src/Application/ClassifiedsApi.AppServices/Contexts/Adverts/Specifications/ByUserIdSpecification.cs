using System;
using System.Linq.Expressions;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Adverts;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Specifications;

/// <summary>
/// Спецификация для фильтрации по идентификатору пользователя объявления.
/// </summary>
public class ByUserIdSpecification : Specification<ShortAdvertInfo>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="ByCategoryIdSpecification"/>.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    public ByUserIdSpecification(Guid userId)
    {
        PredicateExpression = advert => advert.UserId == userId;
    }
    
    /// <inheritdoc />
    public override Expression<Func<ShortAdvertInfo, bool>> PredicateExpression { get; }
}