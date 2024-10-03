using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Contexts.Accounts;

namespace ClassifiedsApi.AppServices.Contexts.Accounts.Services;

/// <summary>
/// Сервис аккаунтов.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Метод для регистрации нового аккаунта.
    /// </summary>
    /// <param name="accountRegister">Модель регистрации нового аккаунта <see cref="AccountRegister"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор нового аккаунта <see cref="Guid"/>.</returns>
    Task<Guid> RegisterAsync(AccountRegister accountRegister, CancellationToken token);
    
    Task<AccountInfo> GetInfoAsync(AccountVerify accountVerify, CancellationToken token);
}