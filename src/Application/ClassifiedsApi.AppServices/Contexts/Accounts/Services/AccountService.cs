using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Accounts.Repositories;
using ClassifiedsApi.AppServices.Helpers;
using ClassifiedsApi.Contracts.Contexts.Accounts;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Accounts.Services;

/// <inheritdoc/>
public class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;
    private readonly IJwtService _jwtService;
    private readonly IAccountVerifier _accountVerifier;
    
    private readonly IValidator<AccountRegister> _accountRegisterValidator;
    private readonly IValidator<AccountVerify> _accountVerifyValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AccountService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий аккаунтов <see cref="IAccountRepository"/>.</param>
    /// <param name="jwtService">Сервис для работы с JWT <see cref="IJwtService"/>.</param>
    /// <param name="accountVerifier">Верификатор аккаунтов <see cref="IAccountVerifier"/>.</param>
    /// <param name="accountRegisterValidator">Валидатор модели регистрации нового аккаунта.</param>
    /// <param name="accountVerifyValidator">Валидатор модели для проверки учетных данных аккаунта.</param>
    public AccountService(
        IAccountRepository repository, 
        IJwtService jwtService,
        IAccountVerifier accountVerifier,
        IValidator<AccountRegister> accountRegisterValidator, 
        IValidator<AccountVerify> accountVerifyValidator)
    {
        _repository = repository;
        _jwtService = jwtService;
        _accountVerifier = accountVerifier;
        _accountRegisterValidator = accountRegisterValidator;
        _accountVerifyValidator = accountVerifyValidator;
    }
    
    /// <inheritdoc/>
    public async Task<Guid> RegisterAsync(AccountRegister accountRegister, CancellationToken token)
    {
        _accountRegisterValidator.ValidateAndThrow(accountRegister);
        await _accountVerifier.VerifyLoginAvailabilityAndThrowAsync(accountRegister.Login!, token);

        var passwordHash = CryptoHelper.GetBase64Hash(accountRegister.Password!);
        var registerRequest = new AccountRegisterRequest
        {
            Login = accountRegister.Login!,
            PasswordHash = passwordHash,
            FirstName = accountRegister.FirstName!,
            LastName = accountRegister.LastName!,
            Email = accountRegister.Email!,
            Phone = accountRegister.Phone,
            BirthDate = accountRegister.BirthDate.GetValueOrDefault(),
        };
        var id = await _repository.RegisterAsync(registerRequest, token);
        return id;
    }

    /// <inheritdoc/>
    public async Task<string> GetAccessTokenAsync(AccountVerify accountVerify, CancellationToken token)
    {
        _accountVerifyValidator.ValidateAndThrow(accountVerify);
        
        var passwordHash = CryptoHelper.GetBase64Hash(accountVerify.Password!);
        var verifyRequest = new AccountVerifyRequest
        {
            Login = accountVerify.Login!,
            PasswordHash = passwordHash
        };
        var accountInfo = await _repository.GetInfoAsync(verifyRequest, token);
        return _jwtService.GetToken(accountInfo);
    }
}