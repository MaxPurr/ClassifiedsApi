using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.AdvertImages.Services;
using ClassifiedsApi.AppServices.Contexts.Adverts.Builders;
using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.AppServices.Contexts.Users.Services;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Services;

/// <inheritdoc />
public class AdvertService : IAdvertService
{
    private readonly IAdvertRepository _repository;
    private readonly IAdvertImageService _advertImageService;
    private readonly IAdvertSpecificationBuilder _specificationBuilder;
    private readonly IUserAccessVerifier _userAccessVerifier;
    
    private readonly IValidator<AdvertCreate> _advertCreateValidator;
    private readonly IValidator<AdvertUpdate> _advertUpdateValidator;
    private readonly IValidator<AdvertsSearch> _advertsSearchValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий объвлений <see cref="IAdvertRepository"/>.</param>
    /// <param name="advertImageService">Сервис фотографий объявлений <see cref="IAdvertImageService"/>.</param>
    /// <param name="specificationBuilder">Строитель спецификаций для объявлений <see cref="IAdvertSpecificationBuilder"/>.</param>
    /// <param name="userAccessVerifier">Верификатор прав пользователя <see cref="IUserAccessVerifier"/>.</param>
    /// <param name="advertCreateValidator">Валидатор модели создания объявления.</param>
    /// <param name="advertUpdateValidator">Валидатор модели обновления объявления.</param>
    /// <param name="advertsSearchValidator">Валидатор модели поиска объявлений.</param>
    public AdvertService(
        IAdvertRepository repository,
        IAdvertImageService advertImageService,
        IAdvertSpecificationBuilder specificationBuilder,
        IUserAccessVerifier userAccessVerifier,
        IValidator<AdvertCreate> advertCreateValidator,
        IValidator<AdvertUpdate> advertUpdateValidator,
        IValidator<AdvertsSearch> advertsSearchValidator)
    {
        _repository = repository;
        _specificationBuilder = specificationBuilder;
        _advertCreateValidator = advertCreateValidator;
        _advertUpdateValidator = advertUpdateValidator;
        _advertsSearchValidator = advertsSearchValidator;
        _userAccessVerifier = userAccessVerifier;
        _advertImageService = advertImageService;
    }
    
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(Guid userId, AdvertCreate advertCreate, CancellationToken token)
    {
        await _advertCreateValidator.ValidateAndThrowAsync(advertCreate, token);
        var createRequest = new AdvertCreateRequest
        {
            UserId = userId,
            AdvertCreate = advertCreate,
        };
        return await _repository.CreateAsync(createRequest, token);
    }
    
    /// <inheritdoc />
    public Task<AdvertInfo> GetByIdAsync(Guid id, CancellationToken token)
    {
        return _repository.GetByIdAsync(id, token);
    }
    
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<ShortAdvertInfo>> SearchAsync(AdvertsSearch search, CancellationToken token)
    { 
        await _advertsSearchValidator.ValidateAndThrowAsync(search, token);
        var specification = _specificationBuilder.Build(search);
        var adverts = await _repository.GetBySpecificationWithPaginationAsync(
            specification: specification, 
            skip: search.Skip, 
            take: search.Take.GetValueOrDefault(0), 
            order: search.Order!, 
            token: token);
        return adverts;
    }

    /// <inheritdoc />
    public async Task<AdvertInfo> UpdateAsync(Guid userId, Guid advertId, AdvertUpdate advertUpdate, CancellationToken token)
    {
        await _userAccessVerifier.VerifyAdvertAccessAndThrowAsync(userId, advertId, token);
        await _advertUpdateValidator.ValidateAndThrowAsync(advertUpdate, token);
        return await _repository.UpdateAsync(advertId, advertUpdate, token);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid userId, Guid advertId, CancellationToken token)
    {
        await _userAccessVerifier.VerifyAdvertAccessAndThrowAsync(userId, advertId, token);
        await _advertImageService.DeleteAllAsync(advertId, token);
        await _repository.DeleteAsync(advertId, token);
    }
}