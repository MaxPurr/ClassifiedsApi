using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Characteristics;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Services;

/// <inheritdoc />
public class CharacteristicVerifier : ICharacteristicVerifier
{
    private readonly ICharacteristicRepository _repository;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий характеристик объявлений <see cref="ICharacteristicRepository"/>.</param>
    public CharacteristicVerifier(ICharacteristicRepository repository)
    {
        _repository = repository;
    }
    
    /// <inheritdoc />
    public async Task VerifyNameAvailabilityAndThrowAsync(Guid advertId, string characteristicName, CancellationToken token)
    {
        var exists = await _repository.IsExistsAsync(advertId, characteristicName, token);
        if (exists)
        {
            throw new UnavailableCharacteristicNameException();
        }
    }
}