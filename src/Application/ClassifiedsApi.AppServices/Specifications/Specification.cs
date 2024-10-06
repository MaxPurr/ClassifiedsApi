using System;
using System.Linq.Expressions;
using ClassifiedsApi.AppServices.Specifications.Extensions;
using ClassifiedsApi.AppServices.Specifications.Internal;

namespace ClassifiedsApi.AppServices.Specifications;

/// <summary>
/// Базовый класс спецификации.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public abstract class Specification<TEntity> : ISpecification<TEntity>
{
    /// <summary>
    /// Спецификация со значением истина для любой входной сущности.
    /// </summary>
    public static readonly Specification<TEntity> True = new ExpressionSpecification<TEntity>(_ => true);
    
    /// <summary>
    /// Спецификация со значением ложь для любой входной сущности.
    /// </summary>
    public static readonly Specification<TEntity> False = new ExpressionSpecification<TEntity>(_ => true);
    
    /// <summary>
    /// Провайдер скомбилированного выражения предиката.
    /// </summary>
    protected Lazy<Func<TEntity, bool>> CompiledPredicateProvider { get; }

    /// <inheritdoc />
    public abstract Expression<Func<TEntity, bool>> PredicateExpression { get; }
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="Specification{TEntity}"/>.
    /// </summary>
    protected Specification()
    {
        CompiledPredicateProvider = new Lazy<Func<TEntity, bool>>(() => PredicateExpression.Compile());
    }
    
    /// <summary>
    /// Операция "И".
    /// </summary>
    /// <param name="left">Первая спецификация.</param>
    /// <param name="right">Вторая спецификация.</param>
    /// <returns>Спецификация.</returns>
    public static Specification<TEntity> operator & (Specification<TEntity> left, Specification<TEntity> right)
    {
        return left.And(right);
    }
    
    /// <summary>
    /// Операция "ИЛИ".
    /// </summary>
    /// <param name="left">Первая спецификация.</param>
    /// <param name="right">Вторая спецификация.</param>
    /// <returns>Спецификация.</returns>
    public static Specification<TEntity> operator | (Specification<TEntity> left, Specification<TEntity> right)
    {
        return left.Or(right);
    }
    
    /// <summary>
    /// Операция "НЕ".
    /// </summary>
    /// <param name="specification">Спецификация.</param>
    /// <returns>Спецификация.</returns>
    public static Specification<TEntity> operator ! (Specification<TEntity> specification)
    {
        return specification.Not();
    }
    
    /// <summary>
    /// Оператор приведения типа к дереву выражений.
    /// </summary>
    /// <param name="specification">Спецификация.</param>
    /// <returns>Дерево выражений.</returns>
    public static implicit operator Expression<Func<TEntity, bool>>(Specification<TEntity> specification)
    {
        return specification.PredicateExpression;
    }
    
    /// <summary>
    /// Метод для создания спецификации из предиката.
    /// </summary>
    /// <param name="predicate">Предикат.</param>
    /// <returns>Обобщенная спецификация на основе дерева выражений <see cref="ExpressionSpecification{TEntity}"/>.</returns>
    public static Specification<TEntity> FromPredicate(Expression<Func<TEntity, bool>> predicate)
    {
        return new ExpressionSpecification<TEntity>(predicate);
    }
    
    /// <inheritdoc />
    public bool IsSatisfiedBy(TEntity entity)
    {
        var predicate = CompiledPredicateProvider.Value;
        return predicate(entity);
    }
}