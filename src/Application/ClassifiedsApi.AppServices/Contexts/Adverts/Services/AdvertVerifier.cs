using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Advert;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Services;

/// <inheritdoc />
public class AdvertVerifier : IAdvertVerifier
{
    private readonly IAdvertRepository _repository;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertVerifier"/>.
    /// </summary>
    /// <param name="repository">Репозиторий объявлений <see cref="IAdvertRepository"/>.</param>
    public AdvertVerifier(IAdvertRepository repository)
    {
        _repository = repository;
    }
    
    /// <inheritdoc />
    public async Task VerifyExistsAndThrowAsync(Guid id, CancellationToken token)
    {
        var exists = await _repository.IsExistsAsync(id, token);
        if (!exists)
        {
            throw new AdvertNotFoundException();
        }
    }
}