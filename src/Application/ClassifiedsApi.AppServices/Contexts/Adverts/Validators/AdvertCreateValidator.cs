using System;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.AppServices.Contexts.Categories.Validators;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Validators;

/// <summary>
/// Валидатор модели создания объявления <see cref="AdvertCreate"/>.
/// </summary>
public class AdvertCreateValidator : AbstractValidator<AdvertCreate>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertCreateValidator"/>.
    /// </summary>
    /// <param name="categoryRepository">Репозиторий категорий <see cref="ICategoryRepository"/>.</param>
    public AdvertCreateValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(advertCreate => advertCreate.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255);
        RuleFor(advertCreate => advertCreate.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(1000);
        RuleFor(advertCreate => advertCreate.Price)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .GreaterThanOrEqualTo(0);
        RuleForEach(advertCreate => advertCreate.Characteristics)
            .ChildRules(characteristic =>
            {
                characteristic.RuleFor(pair => pair.Key)
                    .Cascade(CascadeMode.Stop)
                    .MinimumLength(3)
                    .MaximumLength(255)
                    .WithName("Characteristic name");
                characteristic.RuleFor(pair => pair.Value)
                    .Cascade(CascadeMode.Stop)
                    .MinimumLength(3)
                    .MaximumLength(255)
                    .WithName("Characteristic value");
            });
        RuleFor(advertCreate => advertCreate.CategoryId)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEqual(Guid.Empty)
            .SetValidator(new CategoryExistsValidator(categoryRepository));
    }
}