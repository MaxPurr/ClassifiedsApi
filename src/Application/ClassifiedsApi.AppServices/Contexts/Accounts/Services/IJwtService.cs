using ClassifiedsApi.Contracts.Contexts.Accounts;

namespace ClassifiedsApi.AppServices.Contexts.Accounts.Services;

/// <summary>
/// Сервис для работы с JWT.
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Метод для получения JWT.
    /// </summary>
    /// <param name="accountInfo">Модель информации об аккаунте <see cref="AccountInfo"/>.</param>
    /// <returns>Токен доступа <see cref="AccessToken"/>.</returns>
    AccessToken GetToken(AccountInfo accountInfo);
}