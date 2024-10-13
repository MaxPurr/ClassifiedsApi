using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Users.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Users;

namespace ClassifiedsApi.AppServices.Contexts.Users.Services;

/// <summary>
/// Верификатор пользователей.
/// </summary>
public class UserVerifier : IUserVerifier
{
    private readonly IUserRepository _repository;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="UserVerifier"/>.
    /// </summary>
    /// <param name="repository">Репозиторий пользователей <see cref="IUserRepository"/>.</param>
    public UserVerifier(IUserRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task VerifyExistsAndThrowAsync(Guid id, CancellationToken token)
    {
        var exists = await _repository.IsExistsAsync(id, token);
        if (!exists)
        {
            throw new UserNotFoundException();
        }
    }
}