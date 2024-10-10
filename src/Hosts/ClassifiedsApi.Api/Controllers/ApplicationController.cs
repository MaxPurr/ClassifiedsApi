using System;
using System.Security.Claims;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using ClassifiedsApi.Contracts.Contexts.Characteristics;
using ClassifiedsApi.Contracts.Contexts.Files;
using ClassifiedsApi.Contracts.Contexts.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassifiedsApi.Api.Controllers;

/// <summary>
/// Базовый контроллер приложения.
/// </summary>
public abstract class ApplicationController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="ApplicationController"/>.
    /// </summary>
    /// <param name="httpContextAccessor">Средство доступа к HTTP-контексту.</param>
    protected ApplicationController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    /// <summary>
    /// Возвращает идентификатор текущего пользователя.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException">Возникает если пользователь не прошел аутентификацию.</exception>
    protected Guid CurrentUserId
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext?.User;
            if (user != null)
            {
                var id = user.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(id, out var result))
                {
                    return result;
                }
            }
            throw new UnauthorizedAccessException();
        }
    }
    
    /// <summary>
    /// Метод для получения типизированной модели пользовательского запроса.
    /// </summary>
    /// <param name="model">Модель запроса.</param>
    /// <typeparam name="TModel">Тип модели запроса.</typeparam>
    /// <returns>Модель пользовательского запроса.</returns>
    protected UserRequest<TModel> GetUserRequest<TModel>(TModel model) where TModel : class
    {
        return new UserRequest<TModel>
        {
            UserId = CurrentUserId,
            Model = model
        };
    }

    /// <summary>
    /// Метод для получения типизированной модели пользовательского запроса объявления.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="model">Модель запроса.</param>
    /// <typeparam name="TModel">Тип модели запроса.</typeparam>
    /// <returns>Модель пользовательского запроса объявления.</returns>
    protected AdvertRequest<TModel> GetAdvertRequest<TModel>(Guid advertId, TModel model) where TModel : class
    {
        return new AdvertRequest<TModel>
        {
            UserId = CurrentUserId,
            AdvertId = advertId,
            Model = model
        };
    }
    
    /// <summary>
    /// Метод для получения модели загрузки файла на сервер.
    /// </summary>
    /// <param name="file">Файл.</param>
    /// <returns>Модель загрузки файла на сервер.</returns>
    protected FileUpload GetFileUpload(IFormFile file)
    {
        return new FileUpload
        {
            Name = file.FileName,
            ContentType = file.ContentType,
            ReadStream = file.OpenReadStream()
        };
    }
}