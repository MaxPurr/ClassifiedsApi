using System;
using System.Linq.Expressions;
using ClassifiedsApi.AppServices.Extensions;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Common;
using ClassifiedsApi.Contracts.Contexts.Categories;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Specifications;

/// <summary>
/// Спецификация для фильтрации по имени категории.
/// </summary>
public class NameFilterSpecification : Specification<CategoryInfo>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="NameFilterSpecification"/>.
    /// </summary>
    /// <param name="filter">Модель поиска строки в тексте <see cref="TextFilter"/>.</param>
    public NameFilterSpecification(TextFilter filter)
    {
        PredicateExpression = GetPredicateExpression(filter);
    }

    /// <inheritdoc />
    public override Expression<Func<CategoryInfo, bool>> PredicateExpression { get; }
    
    private static Expression<Func<CategoryInfo, bool>> GetPredicateExpression(TextFilter filter)
    {
        var regex = filter.GetRegularExpression();
        if (filter.IgnoreCase)
        {
            return category => EF.Functions.Like(category.Name.ToLower(), regex);
        }
        return category => EF.Functions.Like(category.Name, regex);
    }
}