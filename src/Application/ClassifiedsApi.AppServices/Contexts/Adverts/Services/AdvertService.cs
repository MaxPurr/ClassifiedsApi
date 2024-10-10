using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Adverts.Builders;
using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using ClassifiedsApi.Contracts.Contexts.Users;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Services;

/// <inheritdoc />
public class AdvertService : IAdvertService
{
    private readonly IAdvertRepository _repository;
    private readonly IAdvertSpecificationBuilder _specificationBuilder;
    
    private readonly IValidator<AdvertCreate> _advertCreateValidator;
    private readonly IValidator<AdvertRequest<AdvertUpdate>> _advertUpdateRequestValidator;
    private readonly IValidator<AdvertRequest> _advertRequestValidator;
    private readonly IValidator<AdvertsSearch> _advertsSearchValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий объвлений <see cref="IAdvertRepository"/>.</param>
    /// <param name="specificationBuilder">Строитель спецификаций для объявлений <see cref="IAdvertSpecificationBuilder"/>.</param>
    /// <param name="advertCreateValidator">Валидатор модели создания объявления.</param>
    /// <param name="advertUpdateRequestValidator">Валидатор модели пользовательского запроса на обновление объявления.</param>
    /// <param name="advertRequestValidator">Валидатор модели пользовательского запроса объявления.</param>
    /// <param name="advertsSearchValidator">Валидатор модели поиска объявлений.</param>
    public AdvertService(
        IAdvertRepository repository,
        IAdvertSpecificationBuilder specificationBuilder,
        IValidator<AdvertCreate> advertCreateValidator,
        IValidator<AdvertRequest<AdvertUpdate>> advertUpdateRequestValidator, 
        IValidator<AdvertRequest> advertRequestValidator, 
        IValidator<AdvertsSearch> advertsSearchValidator)
    {
        _repository = repository;
        _specificationBuilder = specificationBuilder;
        _advertCreateValidator = advertCreateValidator;
        _advertUpdateRequestValidator = advertUpdateRequestValidator;
        _advertRequestValidator = advertRequestValidator;
        _advertsSearchValidator = advertsSearchValidator;
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
    public Task<IReadOnlyCollection<ShortAdvertInfo>> SearchAsync(AdvertsSearch search, CancellationToken token)
    { 
        _advertsSearchValidator.ValidateAndThrow(search);
        var specification = _specificationBuilder.Build(search);
        return _repository.GetBySpecificationWithPaginationAsync(specification, search.Skip, search.Take.GetValueOrDefault(0), search.Order!, token);
    }

    /// <inheritdoc />
    public async Task<AdvertInfo> UpdateAsync(AdvertRequest<AdvertUpdate> advertUpdateRequest, CancellationToken token)
    {
        await _advertUpdateRequestValidator.ValidateAndThrowAsync(advertUpdateRequest, token);
        return await _repository.UpdateAsync(advertUpdateRequest.AdvertId, advertUpdateRequest.Model, token);
    }
    
    /// <inheritdoc />
    public async Task DeleteAsync(AdvertDeleteRequest deleteRequest, CancellationToken token)
    {
        await _advertRequestValidator.ValidateAndThrowAsync(deleteRequest, token);
        await _repository.DeleteAsync(deleteRequest.AdvertId, token);
    }
}