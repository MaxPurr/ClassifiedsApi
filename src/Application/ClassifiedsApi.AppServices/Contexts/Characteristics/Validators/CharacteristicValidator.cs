using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Characteristics;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Validators;

/// <inheritdoc />
public class CharacteristicValidator : ICharacteristicValidator
{
    private readonly ICharacteristicRepository _repository;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicValidator"/>.
    /// </summary>
    /// <param name="repository">Репозиторий характеристик объявлений <see cref="ICharacteristicRepository"/>.</param>
    public CharacteristicValidator(ICharacteristicRepository repository)
    {
        _repository = repository;
    }
    
    /// <inheritdoc />
    public async Task ValidateNameAvailabilityAndThrowAsync(Guid advertId, string characteristicName, CancellationToken token)
    {
        var exists = await _repository.IsExistsAsync(advertId, characteristicName, token);
        if (exists)
        {
            throw new UnavailableCharacteristicNameException();
        }
    }
}