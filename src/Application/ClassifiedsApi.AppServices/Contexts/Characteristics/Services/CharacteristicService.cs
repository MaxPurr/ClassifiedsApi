using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using ClassifiedsApi.Contracts.Contexts.Characteristics;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Services;

/// <inheritdoc />
public class CharacteristicService : ICharacteristicService
{
    private readonly ICharacteristicRepository _repository;
    
    private readonly IValidator<AdvertRequest> _advertRequestValidator;
    private readonly IValidator<AdvertRequest<CharacteristicAdd>> _characteristicAddRequestValidator;
    private readonly IValidator<CharacteristicUpdateRequest> _characteristicUpdateRequestValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий характеристик объявлений <see cref="ICharacteristicRepository"/>.</param>
    /// <param name="advertRequestValidator">Валидатор модели пользовательского запроса объявления.</param>
    /// <param name="characteristicAddRequestValidator">Валидатор модели пользовательского запроса на добавление характеристики объявления.</param>
    /// <param name="characteristicUpdateRequestValidator">Валидатор модели пользовательского запроса на обновление характеристики объявления.</param>
    public CharacteristicService(
        ICharacteristicRepository repository,
        IValidator<AdvertRequest> advertRequestValidator,
        IValidator<AdvertRequest<CharacteristicAdd>> characteristicAddRequestValidator, 
        IValidator<CharacteristicUpdateRequest> characteristicUpdateRequestValidator)
    {
        _repository = repository;
        _advertRequestValidator = advertRequestValidator;
        _characteristicAddRequestValidator = characteristicAddRequestValidator;
        _characteristicUpdateRequestValidator = characteristicUpdateRequestValidator;
    }
    
    /// <inheritdoc />
    public async Task<Guid> AddAsync(AdvertRequest<CharacteristicAdd> addRequest, CancellationToken token)
    {
        await _characteristicAddRequestValidator.ValidateAndThrowAsync(addRequest, token);
        var id = await _repository.AddAsync(addRequest, token);
        return id;
    }
    
    /// <inheritdoc />
    public async Task<CharacteristicInfo> UpdateAsync(CharacteristicUpdateRequest updateRequest, CancellationToken token)
    {
        await _characteristicUpdateRequestValidator.ValidateAndThrowAsync(updateRequest, token);
        return await _repository.UpdateAsync(updateRequest.AdvertId, updateRequest.CharacteristicId, updateRequest.Model, token);
    }
    
    /// <inheritdoc />
    public async Task DeleteAsync(CharacteristicDeleteRequest deleteRequest, CancellationToken token)
    {
        await _advertRequestValidator.ValidateAndThrowAsync(deleteRequest, token);
        await _repository.DeleteAsync(deleteRequest.AdvertId, deleteRequest.CharacteristicId, token);
    }
}