using System;
using System.Linq.Expressions;

namespace ClassifiedsApi.AppServices.Specifications.Extensions;

/// <summary>
/// Расширения для лямбда-выражений.
/// </summary>
public static class PredicateExpressionExtensions
{
    /// <summary>
    /// Выполняет логическую операцию "И" над лямбда-выражениями.
    /// </summary>
    /// <param name="left">Левое выражение.</param>
    /// <param name="right">Правое выражение.</param>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <returns>Лямбда-выражение.</returns>
    public static Expression<Func<TEntity, bool>> And<TEntity>(
        this Expression<Func<TEntity, bool>> left,
        Expression<Func<TEntity, bool>> right)
    {
        return left.Compose(right, Expression.AndAlso);
    }
    
    /// <summary>
    /// Выполняет логическую операцию "ИЛИ" над лямбда-выражениями.
    /// </summary>
    /// <param name="left">Левое выражение.</param>
    /// <param name="right">Правое выражение.</param>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <returns>Лямбда-выражение.</returns>
    public static Expression<Func<TEntity, bool>> Or<TEntity>(
        this Expression<Func<TEntity, bool>> left,
        Expression<Func<TEntity, bool>> right)
    {
        return left.Compose(right, Expression.OrElse);
    }
    
    /// <summary>
    /// Выполняет отрицание лямбда-выражения.
    /// </summary>
    /// <param name="expression">Лямбда-выражение.</param>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <returns>Лямбда-выражение.</returns>
    public static Expression<Func<TEntity, bool>> Not<TEntity>(this Expression<Func<TEntity, bool>> expression)
    {
        return Expression.Lambda<Func<TEntity, bool>>(
            Expression.Not(expression.Body), 
            expression.Parameters
        );
    }
}