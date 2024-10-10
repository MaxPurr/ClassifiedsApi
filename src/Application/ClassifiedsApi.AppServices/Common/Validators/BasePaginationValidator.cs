using ClassifiedsApi.Contracts.Common;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Common.Validators;

/// <summary>
/// Базовый валидатор модели пагинации.
/// </summary>
/// <typeparam name="TPagination">Тип модели пагинации</typeparam>
public abstract class BasePaginationValidator<TPagination> : AbstractValidator<TPagination> 
    where TPagination : BasePagination
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="BasePaginationValidator{TPagination}"/>.
    /// </summary>
    protected BasePaginationValidator()
    {
        RuleFor(pagination => pagination.Skip)
            .GreaterThanOrEqualTo(0);
        RuleFor(pagination => pagination.Take)
            .NotNull()
            .GreaterThan(0)
            .LessThanOrEqualTo(100);
    }
}