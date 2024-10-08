using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;
using ClassifiedsApi.Contracts.Common.Requests;
using ClassifiedsApi.Contracts.Contexts.Characteristics;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Services;

/// <inheritdoc />
public class CharacteristicService : ICharacteristicService
{
    private readonly ICharacteristicRepository _repository;
    
    private readonly IValidator<UserAdvertRequest> _userAdvertRequestValidator;
    private readonly IValidator<UserAdvertRequest<CharacteristicAdd>> _characteristicAddRequestValidator;
    private readonly IValidator<CharacteristicUpdateRequest> _characteristicUpdateRequestValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий характеристик объявлений <see cref="ICharacteristicRepository"/>.</param>
    /// <param name="userAdvertRequestValidator">Валидатор модели пользовательского запроса объявления.</param>
    /// <param name="characteristicAddRequestValidator">Валидатор модели пользовательского запроса на добавление характеристики объявления.</param>
    /// <param name="characteristicUpdateRequestValidator">Валидатор модели пользовательского запроса на обновление характеристики объявления.</param>
    public CharacteristicService(
        ICharacteristicRepository repository, 
        IValidator<UserAdvertRequest> userAdvertRequestValidator, 
        IValidator<UserAdvertRequest<CharacteristicAdd>> characteristicAddRequestValidator, 
        IValidator<CharacteristicUpdateRequest> characteristicUpdateRequestValidator)
    {
        _repository = repository;
        _userAdvertRequestValidator = userAdvertRequestValidator;
        _characteristicAddRequestValidator = characteristicAddRequestValidator;
        _characteristicUpdateRequestValidator = characteristicUpdateRequestValidator;
    }
    
    /// <inheritdoc />
    public async Task<Guid> AddAsync(UserAdvertRequest<CharacteristicAdd> characteristicAddRequest, CancellationToken token)
    {
        await _characteristicAddRequestValidator.ValidateAndThrowAsync(characteristicAddRequest, token);
        return await _repository.AddAsync(characteristicAddRequest, token);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(CharacteristicDeleteRequest characteristicDeleteRequest, CancellationToken token)
    {
        await _userAdvertRequestValidator.ValidateAndThrowAsync(characteristicDeleteRequest, token);
        await _repository.DeleteAsync(characteristicDeleteRequest, token);
    }
    
    /// <inheritdoc />
    public async Task<CharacteristicInfo> UpdateAsync(CharacteristicUpdateRequest characteristicUpdateRequest, CancellationToken token)
    {
        await _characteristicUpdateRequestValidator.ValidateAndThrowAsync(characteristicUpdateRequest, token);
        return await _repository.UpdateAsync(characteristicUpdateRequest, token);
    }
}