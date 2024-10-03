using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Accounts.Repositories;
using ClassifiedsApi.Contracts.Contexts.Accounts;

namespace ClassifiedsApi.AppServices.Contexts.Accounts.Services;

/// <inheritdoc/>
public class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AccountService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий аккаунтов <see cref="IAccountRepository"/>.</param>
    public AccountService(IAccountRepository repository)
    {
        _repository = repository;
    }
    
    /// <inheritdoc/>
    public Task<Guid> RegisterAsync(AccountRegister accountRegister, CancellationToken token)
    {
        return _repository.RegisterAsync(accountRegister, token);
    }

    /// <inheritdoc/>
    public Task<AccountInfo> GetInfoAsync(AccountVerify accountVerify, CancellationToken token)
    {
        return _repository.GetInfoAsync(accountVerify, token);
    }
}