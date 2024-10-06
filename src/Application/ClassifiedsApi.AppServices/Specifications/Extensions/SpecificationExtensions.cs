using System;
using System.Linq.Expressions;
using ClassifiedsApi.AppServices.Specifications.Internal;

namespace ClassifiedsApi.AppServices.Specifications.Extensions;

/// <summary>
/// Расширения для спецификаций.
/// </summary>
public static class SpecificationExtensions
{
    /// <summary>
    /// Компонует две спецификации при помощи логического оператора "И".
    /// </summary>
    /// <param name="left">Первая спецификация.</param>
    /// <param name="right">Вторая спецификация.</param>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <returns>Спецификация.</returns>
    public static Specification<TEntity> And<TEntity>(
        this ISpecification<TEntity> left, 
        ISpecification<TEntity> right)
    {
        return new AndSpecification<TEntity>(left, right);
    }
    
    /// <summary>
    /// Компонует спецификацию с деревом выражений предиката при помощи логического оператора "И".
    /// </summary>
    /// <param name="specification">Спецификация.</param>
    /// <param name="expression">Дерево выражений предиката.</param>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <returns>Спецификация.</returns>
    public static ISpecification<TEntity> AndPredicate<TEntity>(
        this ISpecification<TEntity> specification, 
        Expression<Func<TEntity, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        return specification.And(Specification<TEntity>.FromPredicate(expression));
    }
    
    /// <summary>
    /// Компонует две спецификации при помощи логического оператора "ИЛИ".
    /// </summary>
    /// <param name="left">Первая спецификация.</param>
    /// <param name="right">Вторая спецификация.</param>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <returns>Спецификация.</returns>
    public static Specification<TEntity> Or<TEntity>(
        this ISpecification<TEntity> left, 
        ISpecification<TEntity> right)
    {
        return new OrSpecification<TEntity>(left, right);
    }
    
    /// <summary>
    /// Компонует спецификацию с деревом выражений предиката при помощи логического оператора "ИЛИ".
    /// </summary>
    /// <param name="specification">Спецификация.</param>
    /// <param name="expression">Дерево выражений предиката.</param>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <returns>Спецификация.</returns>
    public static Specification<TEntity> OrPredicate<TEntity>(
        this ISpecification<TEntity> specification, 
        Expression<Func<TEntity, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        return specification.Or(Specification<TEntity>.FromPredicate(expression));
    }
    
    /// <summary>
    /// Выполняет отрицание спецификации.
    /// </summary>
    /// <param name="specification">Спецификация.</param>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <returns>Спецификация.</returns>
    public static Specification<TEntity> Not<TEntity>(this ISpecification<TEntity> specification)
    {
        return new NotSpecification<TEntity>(specification);
    }
}