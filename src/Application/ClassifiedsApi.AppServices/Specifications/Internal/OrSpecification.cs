using System;
using System.Linq.Expressions;
using ClassifiedsApi.AppServices.Specifications.Extensions;

namespace ClassifiedsApi.AppServices.Specifications.Internal;

/// <summary>
/// Спецификация "ИЛИ".
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public class OrSpecification<TEntity> : Specification<TEntity>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="OrSpecification{TEntity}"/>.
    /// </summary>
    /// <param name="left">Первая спецификация.</param>
    /// <param name="right">Вторая спецификация.</param>
    public OrSpecification(ISpecification<TEntity> left, ISpecification<TEntity> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        PredicateExpression = left.PredicateExpression.Or(right.PredicateExpression);
    }
    
    /// <inheritdoc />
    public override Expression<Func<TEntity, bool>> PredicateExpression { get; }
}