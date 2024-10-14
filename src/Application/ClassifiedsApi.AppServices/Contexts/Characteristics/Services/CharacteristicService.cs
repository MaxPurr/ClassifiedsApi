using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;
using ClassifiedsApi.AppServices.Contexts.Users.Services;
using ClassifiedsApi.AppServices.Extensions;
using ClassifiedsApi.Contracts.Contexts.Characteristics;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Services;

/// <inheritdoc />
public class CharacteristicService : ICharacteristicService
{
    private const string CacheKeyFormat = "advert:{0}:characteristics";
    private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(5);
    
    private readonly ICharacteristicRepository _repository;
    private readonly IDistributedCache _cache;
    private readonly IUserAccessVerifier _userAccessVerifier;
    private readonly ICharacteristicVerifier _characteristicVerifier;
    
    private readonly IValidator<CharacteristicAdd> _addValidator;
    private readonly IValidator<CharacteristicUpdate> _updateValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий характеристик объявлений <see cref="ICharacteristicRepository"/>.</param>
    /// <param name="cache">Распределенный кэш <see cref="IDistributedCache"/>.</param>
    /// <param name="userAccessVerifier">Верификатор прав пользователя <see cref="IUserAccessVerifier"/>.</param>
    /// <param name="characteristicVerifier">Верификатор характеристик объявлений <see cref="ICharacteristicVerifier"/>.</param>
    /// <param name="updateValidator">Валидатор модели обновления характеристики объявления <see cref="CharacteristicUpdate"/>.</param>
    /// <param name="addValidator">Валидатор модели добавления характеристики объявления <see cref="CharacteristicAdd"/>..</param>
    public CharacteristicService(
        ICharacteristicRepository repository,
        IDistributedCache cache,
        IUserAccessVerifier userAccessVerifier,
        ICharacteristicVerifier characteristicVerifier,
        IValidator<CharacteristicUpdate> updateValidator, 
        IValidator<CharacteristicAdd> addValidator)
    {
        _repository = repository;
        _userAccessVerifier = userAccessVerifier;
        _updateValidator = updateValidator;
        _addValidator = addValidator;
        _cache = cache;
        _characteristicVerifier = characteristicVerifier;
    }

    private static string GetCacheKey(Guid advertId)
    {
        return string.Format(CacheKeyFormat, advertId);
    }

    private Task ClearCacheAsync(Guid advertId, CancellationToken token)
    {
        var cacheKey = GetCacheKey(advertId);
        return _cache.RemoveAsync(cacheKey, token);
    }
    
    /// <inheritdoc />
    public async Task<Guid> AddAsync(Guid userId, Guid advertId, CharacteristicAdd characteristicAdd, CancellationToken token)
    {
        await _userAccessVerifier.VerifyAdvertAccessAndThrowAsync(userId, advertId, token);
        _addValidator.ValidateAndThrow(characteristicAdd);
        await _characteristicVerifier.VerifyNameAvailabilityAndThrowAsync(advertId, characteristicAdd.Name!, token);
        
        await ClearCacheAsync(advertId, token);
        
        var addRequest = new CharacteristicAddRequest
        {
            AdvertId = advertId,
            CharacteristicAdd = characteristicAdd
        };
        var id = await _repository.AddAsync(addRequest, token);
        return id;
    }
    
    /// <inheritdoc />
    public async Task<CharacteristicInfo> UpdateAsync(
        Guid userId, 
        Guid advertId, 
        Guid characteristicId, 
        CharacteristicUpdate characteristicUpdate,
        CancellationToken token)
    {
        await _userAccessVerifier.VerifyAdvertAccessAndThrowAsync(userId, advertId, token);
        if (characteristicUpdate.Name != null)
        {
            await _characteristicVerifier.VerifyNameAvailabilityAndThrowAsync(advertId, characteristicUpdate.Name!, token);
        }
        _updateValidator.ValidateAndThrow(characteristicUpdate);
        
        await ClearCacheAsync(advertId, token);
        
        var characteristic = await _repository.UpdateAsync(advertId, characteristicId, characteristicUpdate, token);
        return characteristic;
    }
    
    /// <inheritdoc />
    public async Task DeleteAsync(Guid userId, Guid advertId, Guid characteristicId, CancellationToken token)
    {
        await _userAccessVerifier.VerifyAdvertAccessAndThrowAsync(userId, advertId, token);
        
        await ClearCacheAsync(advertId, token);
        await _repository.DeleteAsync(advertId, characteristicId, token);
    }
    
    /// <inheritdoc />
    public async Task DeleteByAdvertIdAsync(Guid advertId, CancellationToken token)
    {
        await ClearCacheAsync(advertId, token);
        await _repository.DeleteByAdvertIdAsync(advertId, token);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<CharacteristicInfo>> GetByAdvertIdAsync(Guid advertId, CancellationToken token)
    {
        var cacheKey = GetCacheKey(advertId);
        var characteristics = await _cache.GetAsync<IReadOnlyCollection<CharacteristicInfo>>(cacheKey, token);
        if (characteristics != null)
        {
            return characteristics;
        }
        characteristics = await _repository.GetByAdvertIdAsync(advertId, token);
        await _cache.SetAsync(cacheKey, characteristics, CacheExpirationTime, token);
        return characteristics;
    }
}