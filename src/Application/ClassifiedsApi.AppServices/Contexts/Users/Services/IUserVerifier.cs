using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Exceptions.Users;

namespace ClassifiedsApi.AppServices.Contexts.Users.Services;

/// <summary>
/// Верификатор пользователя.
/// </summary>
public interface IUserVerifier
{
    /// <summary>
    /// Верифицирует, что пользователь существует и вызывает исключение <see cref="UserNotFoundException"/> если нет.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task VerifyExistsAndThrowAsync(Guid id, CancellationToken token);
}