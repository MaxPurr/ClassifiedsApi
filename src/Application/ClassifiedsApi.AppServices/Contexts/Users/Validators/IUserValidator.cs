using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Exceptions.Users;

namespace ClassifiedsApi.AppServices.Contexts.Users.Validators;

/// <summary>
/// Валидатор пользователей.
/// </summary>
public interface IUserValidator
{
    /// <summary>
    /// Проверяет, что пользователь существует и вызывает исключение <see cref="UserNotFoundException"/> если нет.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task ValidateExistsAndThrowAsync(Guid id, CancellationToken token);
}