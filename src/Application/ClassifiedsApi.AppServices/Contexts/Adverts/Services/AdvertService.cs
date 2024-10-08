using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.Contracts.Common.Requests;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Services;

/// <inheritdoc />
public class AdvertService : IAdvertService
{
    private readonly IAdvertRepository _repository;
    
    private readonly IValidator<AdvertCreate> _advertCreateValidator;
    private readonly IValidator<UserAdvertRequest<AdvertUpdate>> _advertUpdateRequestValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий объвлений <see cref="IAdvertRepository"/>.</param>
    /// <param name="advertCreateValidator">Валидатор модели создания объявления.</param>
    /// <param name="advertUpdateRequestValidator">Валидатор модели пользовательского запроса на обновление объявления.</param>
    public AdvertService(
        IAdvertRepository repository,
        IValidator<AdvertCreate> advertCreateValidator,
        IValidator<UserAdvertRequest<AdvertUpdate>> advertUpdateRequestValidator)
    {
        _repository = repository;
        _advertCreateValidator = advertCreateValidator;
        _advertUpdateRequestValidator = advertUpdateRequestValidator;
    }
    
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(UserRequest<AdvertCreate> advertCreateRequest, CancellationToken token)
    {
        await _advertCreateValidator.ValidateAndThrowAsync(advertCreateRequest.Model, token);
        return await _repository.CreateAsync(advertCreateRequest, token);
    }
    
    /// <inheritdoc />
    public Task<AdvertInfo> GetByIdAsync(Guid id, CancellationToken token)
    {
        return _repository.GetByIdAsync(id, token);
    }
    
    /// <inheritdoc />
    public async Task<AdvertInfo> UpdateAsync(UserAdvertRequest<AdvertUpdate> advertUpdateRequest, CancellationToken token)
    {
        await _advertUpdateRequestValidator.ValidateAndThrowAsync(advertUpdateRequest, token);
        return await _repository.UpdateAsync(advertUpdateRequest, token);
    }
    
    /// <inheritdoc />
    public Task DeleteAsync(Guid advertId, Guid userId, CancellationToken token)
    {
        return _repository.DeleteAsync(advertId, userId, token);
    }
}