using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.Contracts.Common.Requests;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Validators;

/// <summary>
/// Типизированный валидатор модели пользовательского запроса объявления <see cref="UserAdvertRequest{TModel}"/>.
/// </summary>
/// <typeparam name="TUserAdvertRequest">Тип модели пользовательского запроса объявления.</typeparam>
public abstract class UserAdvertRequestValidator<TUserAdvertRequest> : AbstractValidator<TUserAdvertRequest> 
    where TUserAdvertRequest : UserAdvertRequest
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="UserAdvertRequestValidator{TUserAdvertAction}"/>.
    /// </summary>
    /// <param name="advertRepository">Репозиторий объявлений <see cref="IAdvertRepository"/>.</param>
    protected UserAdvertRequestValidator(IAdvertRepository advertRepository)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(action => new { action.AdvertId, action.UserId })
            .MustAsync((action, token) => advertRepository.IsExistsAsync(action.AdvertId ,action.UserId, token))
            .WithMessage("Объявление не было найдено среди списка объявлений пользователя.");
    }
}

/// <summary>
/// Базовый валидатор модели пользовательского запроса объявления <see cref="UserAdvertRequest{TModel}"/>.
/// </summary>
public class UserAdvertRequestValidator : UserAdvertRequestValidator<UserAdvertRequest> 
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="UserAdvertRequestValidator"/>.
    /// </summary>
    /// <param name="advertRepository">Репозиторий объявлений <see cref="IAdvertRepository"/>.</param>
    public UserAdvertRequestValidator(IAdvertRepository advertRepository) : base(advertRepository)
    {
        
    }
}