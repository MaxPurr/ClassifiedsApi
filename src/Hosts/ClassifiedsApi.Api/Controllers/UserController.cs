using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Api.Controllers.Base;
using ClassifiedsApi.AppServices.Contexts.Adverts.Services;
using ClassifiedsApi.AppServices.Contexts.Users.Services;
using ClassifiedsApi.Contracts.Common.Errors;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using ClassifiedsApi.Contracts.Contexts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassifiedsApi.Api.Controllers;

/// <summary>
/// Контроллер для работы с пользователями.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class UserController : BaseApplicationController
{
    private readonly IUserService _service;
    private readonly IAdvertService _advertService;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="UserController"/>.
    /// </summary>
    /// <param name="httpContextAccessor">Средство доступа к HTTP-контексту.</param>
    /// <param name="service">Сервис пользователей <see cref="IUserService"/>.</param>
    /// <param name="advertService">Сервис объявлений <see cref="IAdvertService"/>.</param>
    public UserController(
        IHttpContextAccessor httpContextAccessor, 
        IUserService service, 
        IAdvertService advertService) 
        : base(httpContextAccessor)
    {
        _service = service;
        _advertService = advertService;
    }
    
    /// <summary>
    /// Метод для получения информации о пользователе.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Модель информации о пользователе.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInfoAsync([FromRoute] Guid id, CancellationToken token)
    {
        var userInfo = await _service.GetByIdAsync(id, token);
        return Ok(userInfo);
    }
    
    /// <summary>
    /// Метод для получения информации о текущем пользователе.
    /// </summary>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Модель информации о пользователе.</returns>
    [HttpGet("current")]
    [Authorize]
    [ProducesResponseType(typeof(UserInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetInfoAsync(CancellationToken token)
    {
        var userInfo = await _service.GetByIdAsync(CurrentUserId, token);
        return Ok(userInfo);
    }
    
    /// <summary>
    /// Метод для получения контактов пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Модель информации о контактах пользователя.</returns>
    [HttpGet("{id:guid}/contacts")]
    [Authorize]
    [ProducesResponseType(typeof(UserContactsInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetContactsInfoAsync([FromRoute] Guid id, CancellationToken token)
    {
        var contactsInfo = await _service.GetContactsInfoAsync(id, token);
        return Ok(contactsInfo);
    }
    
    /// <summary>
    /// Метод для получения контактов текущего пользователя.
    /// </summary>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Модель информации о контактах пользователя.</returns>
    [HttpGet("current/contacts")]
    [Authorize]
    [ProducesResponseType(typeof(UserContactsInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetContactsInfoAsync(CancellationToken token)
    {
        var contactsInfo = await _service.GetContactsInfoAsync(CurrentUserId, token);
        return Ok(contactsInfo);
    }

    /// <summary>
    /// Метод для получения объявлений пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="search">Модель поиска объявлений.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Список объявлений пользователя.</returns>
    [HttpPost("{id:guid}/adverts")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ShortAdvertInfo>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAdvertsAsync(
        [FromRoute] Guid id, 
        [FromBody] AdvertsSearch search, 
        CancellationToken token)
    {
        var adverts = await _advertService.GetByUserIdAsync(id, search, token);
        return Ok(adverts);
    }

    /// <summary>
    /// Метод для получения объявлений текущего пользователя.
    /// </summary>
    /// <param name="search">Модель поиска объявлений.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Список объявлений пользователя.</returns>
    [HttpPost("current/adverts")]
    [Authorize]
    [ProducesResponseType(typeof(IReadOnlyCollection<ShortAdvertInfo>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAdvertsAsync([FromBody] AdvertsSearch search, CancellationToken token)
    {
        var adverts = await _advertService.GetByUserIdAsync(CurrentUserId, search, token);
        return Ok(adverts);
    }

    /// <summary>
    /// Метод для обновления фотографии пользователя.
    /// </summary>
    /// <param name="file">Файл.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Идентификатор добавленной фотографии.</returns>
    [HttpPost("current/photo")]
    [Authorize]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdatePhotoAsync(IFormFile file, CancellationToken token)
    {
        var photoUpload = GetFileUpload(file);
        var imageId = await _service.UpdatePhotoAsync(CurrentUserId, photoUpload, token);
        return StatusCode(StatusCodes.Status201Created, imageId);
    }
    
    /// <summary>
    /// Метод для удаления фотографии пользователя.
    /// </summary>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns></returns>
    [HttpDelete("current/photo")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeletePhotoAsync(CancellationToken token)
    {
        await _service.DeletePhotoAsync(CurrentUserId, token);
        return NoContent();
    }
}