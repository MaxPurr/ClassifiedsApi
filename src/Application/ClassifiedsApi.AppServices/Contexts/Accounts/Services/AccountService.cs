using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Common.Services;
using ClassifiedsApi.AppServices.Contexts.Accounts.Repositories;
using ClassifiedsApi.AppServices.Contexts.Accounts.Validators;
using ClassifiedsApi.AppServices.Helpers;
using ClassifiedsApi.Contracts.Contexts.Accounts;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ClassifiedsApi.AppServices.Contexts.Accounts.Services;

/// <inheritdoc/>
public class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;
    private readonly IJwtService _jwtService;
    private readonly IAccountValidator _accountValidator;
    
    private readonly ILogger<AccountService> _logger;
    private readonly IStructuralLoggingService _logService;
    
    private readonly IValidator<AccountRegister> _accountRegisterValidator;
    private readonly IValidator<AccountVerify> _accountVerifyValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AccountService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий аккаунтов <see cref="IAccountRepository"/>.</param>
    /// <param name="jwtService">Сервис для работы с JWT <see cref="IJwtService"/>.</param>
    /// <param name="accountValidator">Валидатор аккаунтов <see cref="IAccountValidator"/>.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="logService">Сервис структурного логирования <see cref="IStructuralLoggingService"/>.</param>
    /// <param name="accountRegisterValidator">Валидатор модели регистрации нового аккаунта.</param>
    /// <param name="accountVerifyValidator">Валидатор модели для проверки учетных данных аккаунта.</param>
    public AccountService(
        IAccountRepository repository, 
        IJwtService jwtService,
        IAccountValidator accountValidator,
        ILogger<AccountService> logger, 
        IStructuralLoggingService logService,
        IValidator<AccountRegister> accountRegisterValidator, 
        IValidator<AccountVerify> accountVerifyValidator)
    {
        _repository = repository;
        _jwtService = jwtService;
        _accountValidator = accountValidator;
        _accountRegisterValidator = accountRegisterValidator;
        _accountVerifyValidator = accountVerifyValidator;
        _logger = logger;
        _logService = logService;
    }
    
    /// <inheritdoc/>
    public async Task<Guid> RegisterAsync(AccountRegister accountRegister, CancellationToken token)
    {
        using var _ = _logService.PushProperty("AccountRegistration", accountRegister, true);
        _logger.LogInformation("Запрос на регистрацию аккаунта.");
        
        _accountRegisterValidator.ValidateAndThrow(accountRegister);
        await _accountValidator.ValidateLoginAvailabilityAndThrowAsync(accountRegister.Login!, token);

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
        _logger.LogInformation("Аккаунт успешно зарегистирован. Идентификатор аккаунта: {AccountId}", id);
        
        return id;
    }

    /// <inheritdoc/>
    public async Task<string> GetAccessTokenAsync(AccountVerify accountVerify, CancellationToken token)
    {
        using var _ = _logService.PushProperty("AccountVerify", accountVerify, true);
        _logger.LogInformation("Запрос на получение токена доступа.");
        
        _accountVerifyValidator.ValidateAndThrow(accountVerify);
        
        var passwordHash = CryptoHelper.GetBase64Hash(accountVerify.Password!);
        var verifyRequest = new AccountVerifyRequest
        {
            Login = accountVerify.Login!,
            PasswordHash = passwordHash
        };
        var accountInfo = await _repository.GetInfoAsync(verifyRequest, token);
        _logger.LogInformation("Получена информация об аккаунте: {@AccountInfo}.", accountInfo);
        
        var accessToken = _jwtService.GetToken(accountInfo);
        _logger.LogInformation("Токен доступа успешно получен.");

        return accessToken;
    }
}