using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Users.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Users;

namespace ClassifiedsApi.AppServices.Contexts.Users.Validators;

/// <inheritdoc />
public class UserValidator : IUserValidator
{
    private readonly IUserRepository _repository;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="UserValidator"/>.
    /// </summary>
    /// <param name="repository">Репозиторий пользователей <see cref="IUserRepository"/>.</param>
    public UserValidator(IUserRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task ValidateExistsAndThrowAsync(Guid id, CancellationToken token)
    {
        var exists = await _repository.IsExistsAsync(id, token);
        if (!exists)
        {
            throw new UserNotFoundException();
        }
    }
}