using System;
using System.Linq.Expressions;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Categories;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Specifications;

/// <summary>
/// Спецификация для поиска по идентификатору родительской категории.
/// </summary>
public class ByParentIdSpecification : Specification<CategoryInfo>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="ByParentIdSpecification"/>.
    /// </summary>
    /// <param name="parentId">Идентификатор родительской категори <see cref="Guid"/>.</param>
    public ByParentIdSpecification(Guid? parentId)
    {
        PredicateExpression = category => category.ParentId == parentId;
    }
    
    /// <inheritdoc />
    public override Expression<Func<CategoryInfo, bool>> PredicateExpression { get; }
}