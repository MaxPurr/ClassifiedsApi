using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Users.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Accounts;

namespace ClassifiedsApi.AppServices.Contexts.Accounts.Validators;

/// <inheritdoc />
public class AccountValidator : IAccountValidator
{
    private readonly IUserRepository _repository;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AccountValidator"/>.
    /// </summary>
    /// <param name="repository">Репозиторий пользователей <see cref="IUserRepository"/>.</param>
    public AccountValidator(IUserRepository repository)
    {
        _repository = repository;
    }
    
    /// <inheritdoc />
    public async Task ValidateLoginAvailabilityAndThrowAsync(string login, CancellationToken token)
    {
        var exists = await _repository.IsExistsAsync(login, token);
        if (exists)
        {
            throw new UnavailableAccountLoginException();
        }
    }
}