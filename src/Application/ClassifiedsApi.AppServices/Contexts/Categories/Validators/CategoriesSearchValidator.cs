using System;
using ClassifiedsApi.AppServices.Common.Validators;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.Contracts.Contexts.Categories;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Validators;

/// <summary>
/// Валидатор модели поиска категорий <see cref="CategoriesSearch"/>.
/// </summary>
public class CategoriesSearchValidator : BasePaginationValidator<CategoriesSearch>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CategoriesSearchValidator"/>.
    /// </summary>
    /// <param name="categoryRepository">Репозиторий категорий <see cref="ICategoryRepository"/>.</param>
    public CategoriesSearchValidator(ICategoryRepository categoryRepository)
    {
        When(search => search.FilterByParentId != null, () =>
        {
            RuleFor(search => search.FilterByParentId!.ParentId)
                .NotEqual(Guid.Empty)
                .SetValidator(new CategoryExistsValidator(categoryRepository))
                .WithName("Parent Id");
        });
        
        When(search => search.NameFilter != null, () =>
        {
            RuleFor(search => search.NameFilter!)
                .SetValidator(new TextFilterValidator());
        });
        
        RuleFor(search => search.Order)
            .NotNull();
        
        When(search => search.Order != null, () =>
        {
            RuleFor(search => search.Order!.By)
                .NotEqual(CategoriesOrderBy.None);
        });
    }
}