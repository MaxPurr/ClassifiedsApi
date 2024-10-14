using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Users.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Accounts;

namespace ClassifiedsApi.AppServices.Contexts.Accounts.Services;

/// <inheritdoc />
public class AccountVerifier : IAccountVerifier
{
    private readonly IUserRepository _repository;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AccountVerifier"/>.
    /// </summary>
    /// <param name="repository">Репозиторий пользователей <see cref="IUserRepository"/>.</param>
    public AccountVerifier(IUserRepository repository)
    {
        _repository = repository;
    }
    
    /// <inheritdoc />
    public async Task VerifyLoginAvailabilityAndThrowAsync(string login, CancellationToken token)
    {
        var exists = await _repository.IsExistsAsync(login, token);
        if (exists)
        {
            throw new UnavailableAccountLoginException();
        }
    }
}