using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;
using ClassifiedsApi.AppServices.Contexts.Users.Services;
using ClassifiedsApi.Contracts.Contexts.Characteristics;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Services;

/// <inheritdoc />
public class CharacteristicService : ICharacteristicService
{
    private readonly ICharacteristicRepository _repository;
    private readonly IUserAccessVerifier _userAccessVerifier;
    private readonly ICharacteristicVerifier _characteristicVerifier;
    
    private readonly IValidator<CharacteristicAdd> _addValidator;
    private readonly IValidator<CharacteristicUpdate> _updateValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий характеристик объявлений <see cref="ICharacteristicRepository"/>.</param>
    /// <param name="userAccessVerifier">Верификатор прав пользователя <see cref="IUserAccessVerifier"/>.</param>
    /// <param name="characteristicVerifier">Верификатор характеристик объявлений <see cref="ICharacteristicVerifier"/>.</param>
    /// <param name="updateValidator">Валидатор модели обновления характеристики объявления <see cref="CharacteristicUpdate"/>.</param>
    /// <param name="addValidator">Валидатор модели добавления характеристики объявления <see cref="CharacteristicAdd"/>..</param>
    public CharacteristicService(
        ICharacteristicRepository repository,
        IUserAccessVerifier userAccessVerifier,
        ICharacteristicVerifier characteristicVerifier,
        IValidator<CharacteristicUpdate> updateValidator, 
        IValidator<CharacteristicAdd> addValidator)
    {
        _repository = repository;
        _userAccessVerifier = userAccessVerifier;
        _updateValidator = updateValidator;
        _addValidator = addValidator;
        _characteristicVerifier = characteristicVerifier;
    }
    
    /// <inheritdoc />
    public async Task<Guid> AddAsync(Guid userId, Guid advertId, CharacteristicAdd characteristicAdd, CancellationToken token)
    {
        await _userAccessVerifier.VerifyAdvertAccessAndThrowAsync(userId, advertId, token);
        await _addValidator.ValidateAndThrowAsync(characteristicAdd, token);
        await _characteristicVerifier.VerifyNameAvailabilityAndThrowAsync(advertId, characteristicAdd.Name!, token);
        var addRequest = new CharacteristicAddRequest
        {
            UserId = userId,
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
        await _updateValidator.ValidateAndThrowAsync(characteristicUpdate, token);
        var characteristic = await _repository.UpdateAsync(advertId, characteristicId, characteristicUpdate, token);
        return characteristic;
    }
    
    /// <inheritdoc />
    public async Task DeleteAsync(Guid userId, Guid advertId, Guid characteristicId, CancellationToken token)
    {
        await _userAccessVerifier.VerifyAdvertAccessAndThrowAsync(userId, advertId, token);
        await _repository.DeleteAsync(advertId, characteristicId, token);
    }
}