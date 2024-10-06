using System;
using System.Linq.Expressions;
using ClassifiedsApi.AppServices.Specifications.Extensions;

namespace ClassifiedsApi.AppServices.Specifications.Internal;

/// <summary>
/// Спецификация "НЕ".
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public class NotSpecification<TEntity> : Specification<TEntity>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="NotSpecification{TEntity}"/>.
    /// </summary>
    /// <param name="specification">Спецификация.</param>
    public NotSpecification(ISpecification<TEntity> specification)
    {
        ArgumentNullException.ThrowIfNull(specification);
        PredicateExpression = specification.PredicateExpression.Not();
    }
    
    /// <inheritdoc />
    public override Expression<Func<TEntity, bool>> PredicateExpression { get; }
}