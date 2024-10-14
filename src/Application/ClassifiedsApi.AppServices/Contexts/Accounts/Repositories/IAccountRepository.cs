using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Contexts.Accounts;

namespace ClassifiedsApi.AppServices.Contexts.Accounts.Repositories;

/// <summary>
/// Репозиторий аккаунтов.
/// </summary>
public interface IAccountRepository
{
     /// <summary>
     /// Метод для регистрации нового аккаунта.
     /// </summary>
     /// <param name="registerRequest">Запрос на регистрацию аккаунт <see cref="AccountRegisterRequest"/>.</param>
     /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
     /// <returns>Идентификатор нового аккаунта <see cref="Guid"/>.</returns>
     Task<Guid> RegisterAsync(AccountRegisterRequest registerRequest, CancellationToken token);
     
     /// <summary>
     /// Метод для получения информации об аккаунте.
     /// </summary>
     /// <param name="verifyRequest">Запрос на проверку учетных данных аккаунта <see cref="AccountVerifyRequest"/>.</param>
     /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
     /// <returns>Модель информации об аккаунте <see cref="AccountInfo"/>.</returns>
     Task<AccountInfo> GetInfoAsync(AccountVerifyRequest verifyRequest, CancellationToken token);
}