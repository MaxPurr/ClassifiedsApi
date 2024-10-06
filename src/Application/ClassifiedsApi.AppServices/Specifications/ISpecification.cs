using System;
using System.Linq.Expressions;

namespace ClassifiedsApi.AppServices.Specifications
{
    /// <summary>
    /// Интерфейс-маркер спецификации.
    /// </summary>
    public interface ISpecification { }
    
    /// <summary>
    /// Спецификация некоторой сущности.
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    public interface ISpecification<TEntity> : ISpecification
    {
        /// <summary>
        /// Дерево выражений предиката спецификации.
        /// </summary>
        Expression<Func<TEntity, bool>> PredicateExpression { get; }
        
        /// <summary>
        /// Проверяет удовлетворяет ли сущность спецификации.
        /// </summary>
        /// <param name="entity">Сущность для проверки.</param>
        /// <returns><code data-dev-comment-type="langword">true</code> если сущность удовлетворяет спецификации, иначе <code data-dev-comment-type="langword">false</code>.</returns>
        bool IsSatisfiedBy(TEntity entity);
    }
}