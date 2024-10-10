using System;
using System.Linq.Expressions;
using ClassifiedsApi.AppServices.Extensions;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Common;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Specifications;

/// <summary>
/// Спецификация для фильтрации по тексту объявления.
/// </summary>
public class TextFilterSpecification : Specification<ShortAdvertInfo>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="TextFilterSpecification"/>.
    /// </summary>
    /// <param name="filter">Модель поиска строки в тексте <see cref="TextFilter"/>.</param>
    public TextFilterSpecification(TextFilter filter)
    {
        PredicateExpression = GetPredicateExpression(filter);
    }
    
    /// <inheritdoc />
    public override Expression<Func<ShortAdvertInfo, bool>> PredicateExpression { get; }
    
    private static Expression<Func<ShortAdvertInfo, bool>> GetPredicateExpression(TextFilter filter)
    {
        var regex = filter.GetRegularExpression();
        if (filter.IgnoreCase)
        {
            return advert => EF.Functions.Like(advert.Title.ToLower(), regex) ||
                             EF.Functions.Like(advert.Description.ToLower(), regex);
        }
        return advert => EF.Functions.Like(advert.Title, regex) ||
                         EF.Functions.Like(advert.Description, regex);
    }
}