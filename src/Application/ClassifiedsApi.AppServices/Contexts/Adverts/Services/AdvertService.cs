using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Common.Services;
using ClassifiedsApi.AppServices.Contexts.AdvertImages.Services;
using ClassifiedsApi.AppServices.Contexts.Adverts.Builders;
using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.AppServices.Contexts.Categories.Validators;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Services;
using ClassifiedsApi.AppServices.Contexts.Users.Validators;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Services;

/// <inheritdoc cref="IAdvertService"/>
public class AdvertService : ServiceBase, IAdvertService
{
    private readonly IAdvertRepository _repository;
    private readonly IAdvertImageService _advertImageService;
    private readonly ICharacteristicService _characteristicService;
    private readonly IAdvertSpecificationBuilder _specificationBuilder;
    private readonly IUserAccessValidator _userAccessValidator;
    private readonly IUserValidator _userValidator;
    private readonly ICategoryValidator _categoryValidator;
    
    private readonly ILogger<AdvertService> _logger;
    private readonly IStructuralLoggingService _logService;
    
    private readonly IValidator<AdvertCreate> _advertCreateValidator;
    private readonly IValidator<AdvertUpdate> _advertUpdateValidator;
    private readonly IValidator<AdvertsSearch> _advertsSearchValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий объвлений <see cref="IAdvertRepository"/>.</param>
    /// <param name="advertImageService">Сервис фотографий объявлений <see cref="IAdvertImageService"/>.</param>
    /// <param name="characteristicService">Сервис характеристик объявлений <see cref="ICharacteristicService"/>.</param>
    /// <param name="specificationBuilder">Строитель спецификаций для объявлений <see cref="IAdvertSpecificationBuilder"/>.</param>
    /// <param name="userAccessValidator">Валидатор прав пользователей <see cref="IUserAccessValidator"/>.</param>
    /// <param name="userValidator">Валидатор пользователей <see cref="IUserValidator"/>.</param>
    /// <param name="categoryValidator">Валидатор категорий <see cref="ICategoryValidator"/>.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="logService">Сервис структурного логирования <see cref="IStructuralLoggingService"/>.</param>
    /// <param name="advertCreateValidator">Валидатор модели создания объявления.</param>
    /// <param name="advertUpdateValidator">Валидатор модели обновления объявления.</param>
    /// <param name="advertsSearchValidator">Валидатор модели поиска объявлений.</param>
    public AdvertService(
        IAdvertRepository repository,
        IAdvertImageService advertImageService,
        ICharacteristicService characteristicService,
        IAdvertSpecificationBuilder specificationBuilder,
        IUserAccessValidator userAccessValidator,
        IUserValidator userValidator,
        ICategoryValidator categoryValidator,
        ILogger<AdvertService> logger, 
        IStructuralLoggingService logService,
        IValidator<AdvertCreate> advertCreateValidator,
        IValidator<AdvertUpdate> advertUpdateValidator,
        IValidator<AdvertsSearch> advertsSearchValidator)
    {
        _repository = repository;
        _specificationBuilder = specificationBuilder;
        _advertCreateValidator = advertCreateValidator;
        _advertUpdateValidator = advertUpdateValidator;
        _advertsSearchValidator = advertsSearchValidator;
        _logger = logger;
        _logService = logService;
        _categoryValidator = categoryValidator;
        _characteristicService = characteristicService;
        _userValidator = userValidator;
        _userAccessValidator = userAccessValidator;
        _advertImageService = advertImageService;
    }
    
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(Guid userId, AdvertCreate advertCreate, CancellationToken token)
    {
        using (_logService.PushProperty("UserId", userId))
        using (_logService.PushProperty("AdvertCreate", advertCreate, true))
        {
            _logger.LogInformation("Запрос на создание объявления.");
            
            _advertCreateValidator.ValidateAndThrow(advertCreate);
            await _categoryValidator.ValidateExistsAndThrowAsync(advertCreate.CategoryId.GetValueOrDefault(), token);
        
            var createRequest = new AdvertCreateRequest
            {
                UserId = userId,
                AdvertCreate = advertCreate,
            };
            var id = await _repository.CreateAsync(createRequest, token);
            _logger.LogInformation("Объявление успешно созданно. Идентификатор объявления: {AdvertId}", id);

            return id;
        }
    }
    
    /// <inheritdoc />
    public async Task<AdvertInfo> GetByIdAsync(Guid id, CancellationToken token)
    {
        using var _ = _logService.PushProperty("AdvertId", id);
        _logger.LogInformation("Получение информации об объявлении по идентификатору.");
        
        var info = await _repository.GetByIdAsync(id, token);
        info.ImageIds = await _advertImageService.GetByAdvertIdAsync(id, token);
        info.Characteristics = await _characteristicService.GetByAdvertIdAsync(id, token);
        _logger.LogInformation("Информация успешно получена: {@AdvertInfo}", info);
        
        return info;
    }

    private async Task ValidateSearchAsync(AdvertsSearch search, CancellationToken token)
    {
        _advertsSearchValidator.ValidateAndThrow(search);
        if (search.FilterByCategoryId.HasValue)
        {
            await _categoryValidator.ValidateExistsAndThrowAsync(search.FilterByCategoryId.Value, token);
        }
    }

    private async Task<IReadOnlyCollection<ShortAdvertInfo>> GetBySpecificationWithPaginationAsync(
        ISpecification<ShortAdvertInfo> specification, 
        int? skip, 
        int take,
        AdvertsOrder order,
        CancellationToken token)
    {
        var adverts = await _repository.GetBySpecificationWithPaginationAsync(
            specification: specification, 
            skip: skip, 
            take: take, 
            order: order, 
            token: token);
        foreach (var advert in adverts)
        {
            advert.ImageIds = await _advertImageService.GetByAdvertIdAsync(advert.Id, token);
        }
        return adverts;
    }
    
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<ShortAdvertInfo>> SearchAsync(AdvertsSearch search, CancellationToken token)
    { 
        using var _ = _logService.PushProperty("AdvertsSearch", search, true);
        _logger.LogInformation("Поиск объявлений по запросу.");
        
        await ValidateSearchAsync(search, token);
        
        var specification = _specificationBuilder.Build(search);
        _logger.LogInformation("Построена спецификация поиска объявлений.");
        
        var adverts = await GetBySpecificationWithPaginationAsync(
            specification: specification, 
            skip: search.Skip, 
            take: search.Take.GetValueOrDefault(), 
            order: search.Order!, 
            token: token);
        _logger.LogInformation("Объявления успешно получены.");
        
        return adverts;
    }
    
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<ShortAdvertInfo>> GetByUserIdAsync(
        Guid userId, 
        AdvertsSearch search, 
        CancellationToken token)
    {
        using (_logService.PushProperty("UserId", userId))
        using (_logService.PushProperty("AdvertsSearch", search, true))
        {
            _logger.LogInformation("Поиск объявлений пользователя по запросу.");
            
            await ValidateSearchAsync(search, token);
            await _userValidator.ValidateExistsAndThrowAsync(userId, token);
        
            var specification = _specificationBuilder.Build(userId, search);
            _logger.LogInformation("Построена спецификация поиска объявлений пользователя.");
            
            var adverts = await GetBySpecificationWithPaginationAsync(
                specification: specification, 
                skip: search.Skip, 
                take: search.Take.GetValueOrDefault(), 
                order: search.Order!, 
                token: token);
            _logger.LogInformation("Объявления пользователя успешно получены.");
            
            return adverts;
        }
    }

    /// <inheritdoc />
    public async Task<UpdatedAdvertInfo> UpdateAsync(Guid userId, Guid advertId, AdvertUpdate advertUpdate, CancellationToken token)
    {
        using (_logService.PushProperty("UserId", userId))
        using (_logService.PushProperty("AdvertId", advertId))
        using (_logService.PushProperty("AdvertUpdate", advertUpdate, true))
        {
            _logger.LogInformation("Запрос на обновление объявления.");
            
            _advertUpdateValidator.ValidateAndThrow(advertUpdate);
            await _userAccessValidator.ValidateAdvertAccessAndThrowAsync(userId, advertId, token);
            if (advertUpdate.CategoryId.HasValue)
            {
                await _categoryValidator.ValidateExistsAndThrowAsync(advertUpdate.CategoryId.Value, token);
            }
        
            var updatedInfo = await _repository.UpdateAsync(advertId, advertUpdate, token);
            _logger.LogInformation("Объявление успешно обновлено.");
            
            return updatedInfo;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid userId, Guid advertId, CancellationToken token)
    {
        using (_logService.PushProperty("UserId", userId))
        using (_logService.PushProperty("AdvertId", advertId))
        {
            _logger.LogInformation("Запрос на удаление объявления.");
            
            await _userAccessValidator.ValidateAdvertAccessAndThrowAsync(userId, advertId, token);
            
            using var scope = CreateTransactionScope();
            await _advertImageService.DeleteByAdvertIdAsync(advertId, token);
            await _characteristicService.DeleteByAdvertIdAsync(advertId, token);
            await _repository.DeleteAsync(advertId, token);
            scope.Complete();
            
            _logger.LogInformation("Объявление успешно удалено.");
        }
    }
}