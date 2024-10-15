using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Common.Services;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Validators;
using ClassifiedsApi.AppServices.Contexts.Users.Validators;
using ClassifiedsApi.AppServices.Extensions;
using ClassifiedsApi.Contracts.Contexts.Characteristics;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Services;

/// <inheritdoc />
public class CharacteristicService : ICharacteristicService
{
    private const string CacheKeyFormat = "advert:{0}:characteristics";
    private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(5);
    
    private readonly ICharacteristicRepository _repository;
    private readonly IDistributedCache _cache;
    private readonly IUserAccessValidator _userAccessValidator;
    private readonly ICharacteristicValidator _characteristicValidator;
    
    private readonly ILogger<CharacteristicService> _logger;
    private readonly IStructuralLoggingService _logService;
    
    private readonly IValidator<CharacteristicAdd> _addValidator;
    private readonly IValidator<CharacteristicUpdate> _updateValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий характеристик объявлений <see cref="ICharacteristicRepository"/>.</param>
    /// <param name="cache">Распределенный кэш <see cref="IDistributedCache"/>.</param>
    /// <param name="userAccessValidator">Валидатор прав пользователя <see cref="IUserAccessValidator"/>.</param>
    /// <param name="characteristicValidator">Валидатор характеристик объявлений <see cref="ICharacteristicValidator"/>.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="logService">Сервис структурного логирования <see cref="IStructuralLoggingService"/>.</param>
    /// <param name="updateValidator">Валидатор модели обновления характеристики объявления <see cref="CharacteristicUpdate"/>.</param>
    /// <param name="addValidator">Валидатор модели добавления характеристики объявления <see cref="CharacteristicAdd"/>..</param>
    public CharacteristicService(
        ICharacteristicRepository repository,
        IDistributedCache cache,
        IUserAccessValidator userAccessValidator,
        ICharacteristicValidator characteristicValidator,
        ILogger<CharacteristicService> logger, 
        IStructuralLoggingService logService,
        IValidator<CharacteristicUpdate> updateValidator, 
        IValidator<CharacteristicAdd> addValidator)
    {
        _repository = repository;
        _userAccessValidator = userAccessValidator;
        _updateValidator = updateValidator;
        _addValidator = addValidator;
        _logger = logger;
        _logService = logService;
        _cache = cache;
        _characteristicValidator = characteristicValidator;
    }

    private static string GetCacheKey(Guid advertId)
    {
        return string.Format(CacheKeyFormat, advertId);
    }

    private async Task ClearCacheAsync(Guid advertId, CancellationToken token)
    {
        var cacheKey = GetCacheKey(advertId);
        await _cache.RemoveAsync(cacheKey, token);
        _logger.LogInformation("Кэш характеристик объявления очищен.");
    }
    
    /// <inheritdoc />
    public async Task<Guid> AddAsync(Guid userId, Guid advertId, CharacteristicAdd characteristicAdd, CancellationToken token)
    {
        using (_logService.PushProperty("UserId", userId))
        using (_logService.PushProperty("AdvertId", advertId))
        using (_logService.PushProperty("CharacteristicAdd", characteristicAdd, true))
        {
            _logger.LogInformation("Запрос на добавление характеристики объявления.");
            
            _addValidator.ValidateAndThrow(characteristicAdd);
            await _userAccessValidator.ValidateAdvertAccessAndThrowAsync(userId, advertId, token);
            await _characteristicValidator.ValidateNameAvailabilityAndThrowAsync(advertId, characteristicAdd.Name!, token);
        
            await ClearCacheAsync(advertId, token);
        
            var addRequest = new CharacteristicAddRequest
            {
                AdvertId = advertId,
                CharacteristicAdd = characteristicAdd
            };
            var id = await _repository.AddAsync(addRequest, token);
            _logger.LogInformation("Характеристика объявления успешно добавлена. " +
                                   "Идентификатор характеристики: {CharacteristicId}", id);
            
            return id;
        }
    }
    
    /// <inheritdoc />
    public async Task<CharacteristicInfo> UpdateAsync(
        Guid userId, 
        Guid advertId, 
        Guid characteristicId, 
        CharacteristicUpdate characteristicUpdate,
        CancellationToken token)
    {
        using (_logService.PushProperty("UserId", userId))
        using (_logService.PushProperty("AdvertId", advertId))
        using (_logService.PushProperty("CharacteristicId", characteristicId))
        using (_logService.PushProperty("CharacteristicUpdate", characteristicUpdate, true))
        {
            _logger.LogInformation("Запрос на обновление характеристики объявления.");
            
            _updateValidator.ValidateAndThrow(characteristicUpdate);
            await _userAccessValidator.ValidateAdvertAccessAndThrowAsync(userId, advertId, token);
            if (characteristicUpdate.Name != null)
            {
                await _characteristicValidator.ValidateNameAvailabilityAndThrowAsync(advertId, characteristicUpdate.Name!, token);
            }
        
            await ClearCacheAsync(advertId, token);
        
            var characteristic = await _repository.UpdateAsync(advertId, characteristicId, characteristicUpdate, token);
            _logger.LogInformation("Характеристика объявления успешно обновлена.");
            
            return characteristic;
        }
    }
    
    /// <inheritdoc />
    public async Task DeleteAsync(Guid userId, Guid advertId, Guid characteristicId, CancellationToken token)
    {
        using (_logService.PushProperty("UserId", userId))
        using (_logService.PushProperty("AdvertId", advertId))
        using (_logService.PushProperty("CharacteristicId", characteristicId))
        {
            _logger.LogInformation("Запрос на удаление характеристики объявления.");
            
            await _userAccessValidator.ValidateAdvertAccessAndThrowAsync(userId, advertId, token);
        
            await ClearCacheAsync(advertId, token);
        
            await _repository.DeleteAsync(advertId, characteristicId, token);
            _logger.LogInformation("Характеристика объявления успешно удалена.");
        }
    }
    
    /// <inheritdoc />
    public async Task DeleteByAdvertIdAsync(Guid advertId, CancellationToken token)
    {
        _logger.LogInformation("Запрос на удаление всех характеристик объявления.");
        
        await ClearCacheAsync(advertId, token);
        
        await _repository.DeleteByAdvertIdAsync(advertId, token);
        _logger.LogInformation("Характеристики объявления успешно удалены.");
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<CharacteristicInfo>> GetByAdvertIdAsync(Guid advertId, CancellationToken token)
    {
        _logger.LogInformation("Получение всех характеристик объявления.");
        
        var cacheKey = GetCacheKey(advertId);
        var characteristics = await _cache.GetAsync<IReadOnlyCollection<CharacteristicInfo>>(cacheKey, token);
        if (characteristics != null)
        {
            _logger.LogInformation("Характеристики объявления получены из кэша.");
            return characteristics;
        }
        
        characteristics = await _repository.GetByAdvertIdAsync(advertId, token);
        _logger.LogInformation("Характеристики объявления получены из базы данных.");
        
        await _cache.SetAsync(cacheKey, characteristics, CacheExpirationTime, token);
        _logger.LogInformation("Характеристики объявления добавлены в кэш.");
        
        return characteristics;
    }
}