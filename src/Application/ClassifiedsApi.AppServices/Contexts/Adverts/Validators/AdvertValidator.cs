using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Advert;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Validators;

/// <inheritdoc />
public class AdvertValidator : IAdvertValidator
{
    private readonly IAdvertRepository _repository;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertValidator"/>.
    /// </summary>
    /// <param name="repository">Репозиторий объявлений <see cref="IAdvertRepository"/>.</param>
    public AdvertValidator(IAdvertRepository repository)
    {
        _repository = repository;
    }
    
    /// <inheritdoc />
    public async Task ValidateExistsAndThrowAsync(Guid id, CancellationToken token)
    {
        var exists = await _repository.IsExistsAsync(id, token);
        if (!exists)
        {
            throw new AdvertNotFoundException();
        }
    }
}