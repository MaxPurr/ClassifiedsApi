using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Accounts.Repositories;
using ClassifiedsApi.Contracts.Contexts.Accounts;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Accounts.Services;

/// <inheritdoc/>
public class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;
    
    private readonly IValidator<AccountRegister> _accountRegisterValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AccountService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий аккаунтов <see cref="IAccountRepository"/>.</param>
    /// <param name="accountRegisterValidator">Валидатор модели регистрации нового аккаунта.</param>
    public AccountService(IAccountRepository repository, IValidator<AccountRegister> accountRegisterValidator)
    {
        _repository = repository;
        _accountRegisterValidator = accountRegisterValidator;
    }
    
    /// <inheritdoc/>
    public Task<Guid> RegisterAsync(AccountRegister accountRegister, CancellationToken token)
    {
        _accountRegisterValidator.ValidateAndThrow(accountRegister);
        return _repository.RegisterAsync(accountRegister, token);
    }

    /// <inheritdoc/>
    public Task<AccountInfo> GetInfoAsync(AccountVerify accountVerify, CancellationToken token)
    {
        return _repository.GetInfoAsync(accountVerify, token);
    }
}