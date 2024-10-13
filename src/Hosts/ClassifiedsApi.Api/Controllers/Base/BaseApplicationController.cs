using System;
using System.Security.Claims;
using ClassifiedsApi.Api.Helpers;
using ClassifiedsApi.Contracts.Contexts.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassifiedsApi.Api.Controllers.Base;

/// <summary>
/// Базовый контроллер приложения.
/// </summary>
public abstract class BaseApplicationController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="BaseApplicationController"/>.
    /// </summary>
    /// <param name="httpContextAccessor">Средство доступа к HTTP-контексту.</param>
    protected BaseApplicationController(IHttpContextAccessor httpContextAccessor)
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
    /// Аутентифицирован ли пользователь.
    /// </summary>
    protected bool IsUserAuthenticated
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext?.User;
            return user != null && 
                   user.Identity != null && 
                   user.Identity.IsAuthenticated;
        }
    }
    
    /// <summary>
    /// Метод для получения модели загрузки файла на сервер.
    /// </summary>
    /// <param name="file">Файл.</param>
    /// <returns>Модель загрузки файла на сервер.</returns>
    protected static FileUpload GetFileUpload(IFormFile file)
    {
        return new FileUpload
        {
            Name = file.FileName,
            ContentType = file.ContentType,
            Content = FileHelper.GetByteArray(file),
            Length = file.Length
        };
    }
}