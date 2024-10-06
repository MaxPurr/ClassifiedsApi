using System;
using System.Linq.Expressions;

namespace ClassifiedsApi.AppServices.Specifications.Internal;

/// <summary>
/// Обобщенная спецификация на основе дерева выражений.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class ExpressionSpecification<TEntity> : Specification<TEntity>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="ExpressionSpecification{TEntity}"/>.
    /// </summary>
    /// <param name="expression">Дерево выражений.</param>
    /// <exception cref="ArgumentNullException">Возникает если переданное дерево выражений равно null.</exception>
    public ExpressionSpecification(Expression<Func<TEntity, bool>> expression)
    {
        PredicateExpression = expression ?? throw new ArgumentNullException(nameof(expression));
    }
    
    /// <inheritdoc />
    public override Expression<Func<TEntity, bool>> PredicateExpression { get; }
}