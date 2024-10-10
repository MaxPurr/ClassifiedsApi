using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Validators;

/// <summary>
/// Типизированный валидатор модели пользовательского запроса объявления <see cref="AdvertRequest{TModel}"/>.
/// </summary>
/// <typeparam name="TAdvertRequest">Тип модели пользовательского запроса объявления.</typeparam>
public abstract class AdvertRequestValidator<TAdvertRequest> : AbstractValidator<TAdvertRequest> 
    where TAdvertRequest : AdvertRequest
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertRequestValidator{TAdvertRequest}"/>.
    /// </summary>
    /// <param name="advertRepository">Репозиторий объявлений <see cref="IAdvertRepository"/>.</param>
    protected AdvertRequestValidator(IAdvertRepository advertRepository)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(request => new { request.AdvertId, request.UserId })
            .MustAsync((request, token) => advertRepository.IsExistsAsync(request.UserId, request.AdvertId, token))
            .WithMessage("Объявление не было найдено среди объявлений пользователя.");
    }
}

/// <summary>
/// Базовый валидатор модели пользовательского запроса объявления <see cref="AdvertRequest"/>.
/// </summary>
public class AdvertRequestValidator : AdvertRequestValidator<AdvertRequest> 
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertRequestValidator"/>.
    /// </summary>
    /// <param name="advertRepository">Репозиторий объявлений <see cref="IAdvertRepository"/>.</param>
    public AdvertRequestValidator(IAdvertRepository advertRepository) : base(advertRepository)
    {
        
    }
}