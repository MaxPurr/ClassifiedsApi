using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Exceptions.Accounts;

namespace ClassifiedsApi.AppServices.Contexts.Accounts.Validators;

/// <summary>
/// Валидатор аккаунтов.
/// </summary>
public interface IAccountValidator
{
    /// <summary>
    /// Проверяет доступность логина аккаунта и вызывает исключение <see cref="UnavailableAccountLoginException"/> если логин недоступен.
    /// </summary>
    /// <param name="login">Логин.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task ValidateLoginAvailabilityAndThrowAsync(string login, CancellationToken token);
}