using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Api.Controllers.Base;
using ClassifiedsApi.AppServices.Contexts.AdvertImages.Services;
using ClassifiedsApi.AppServices.Contexts.Adverts.Services;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Services;
using ClassifiedsApi.AppServices.Contexts.Comments.Services;
using ClassifiedsApi.Contracts.Common.Errors;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using ClassifiedsApi.Contracts.Contexts.Characteristics;
using ClassifiedsApi.Contracts.Contexts.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassifiedsApi.Api.Controllers;

/// <summary>
/// Контроллер для работы с объявлениями.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class AdvertController : BaseApplicationController
{
    private readonly IAdvertService _advertService;
    private readonly ICharacteristicService _characteristicService;
    private readonly IAdvertImageService _advertImageService;
    private readonly ICommentService _commentService;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertController"/>.
    /// </summary>
    /// <param name="httpContextAccessor">Средство доступа к HTTP-контексту.</param>
    /// <param name="advertService">Сервис объявлений <see cref="IAdvertService"/>.</param>
    /// <param name="characteristicService">Сервис характеристик объявлений <see cref="ICharacteristicService"/>.</param>
    /// <param name="advertImageService">Сервис фотографий объявлений <see cref="IAdvertImageService"/>.</param>
    /// <param name="commentService">Сервис комментариев <see cref="ICommentService"/>.</param>
    public AdvertController(
        IHttpContextAccessor httpContextAccessor, 
        IAdvertService advertService,
        ICharacteristicService characteristicService, 
        IAdvertImageService advertImageService, 
        ICommentService commentService) 
        : base(httpContextAccessor)
    {
        _advertService = advertService;
        _characteristicService = characteristicService;
        _advertImageService = advertImageService;
        _commentService = commentService;
    }
    
    /// <summary>
    /// Метод для создания нового объявления.
    /// </summary>
    /// <param name="advertCreate">Модель создания объявления.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Идентификатор созданного объявления.</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateAsync([FromBody] AdvertCreate advertCreate, CancellationToken token)
    {
        var id = await _advertService.CreateAsync(CurrentUserId, advertCreate, token);
        return StatusCode(StatusCodes.Status201Created, id);
    }
    
    /// <summary>
    /// Метод для получения информации об объявлении.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Модель информации об объявлении.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AdvertInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInfoAsync([FromRoute] Guid id, CancellationToken token)
    {
        var advertInfo = await _advertService.GetByIdAsync(id, token);
        return Ok(advertInfo);
    }
    
    /// <summary>
    /// Метод для поиска объявлений по заданному условию.
    /// </summary>
    /// <param name="search">Модель поиска объявлений.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Список объявлений.</returns>
    [HttpPost("search")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ShortAdvertInfo>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(AdvertsSearch search, CancellationToken token)
    {
        var adverts = await _advertService.SearchAsync(search, token);
        return Ok(adverts);
    }
    
    /// <summary>
    /// Метод для обновления объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="advertUpdate">Модель обновления объявления.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Модель обновленной информации об объявлении.</returns>
    [HttpPatch("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(UpdatedAdvertInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] AdvertUpdate advertUpdate, CancellationToken token)
    {
        var updatedInfo = await _advertService.UpdateAsync(CurrentUserId, id, advertUpdate, token);
        return Ok(updatedInfo);
    }
    
    /// <summary>
    /// Метод для удаления объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken token)
    {
        await _advertService.DeleteAsync(CurrentUserId, id, token);
        return NoContent();
    }

    /// <summary>
    /// Метод для добавления новой характеристики объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="characteristicAdd">Модель добавления характеристики объявления.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Идентификатор добавленной характеристики объявления.</returns>
    [HttpPost("{id:guid}/characteristics")]
    [Authorize]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddCharacteristicAsync(
        [FromRoute] Guid id, 
        [FromBody] CharacteristicAdd characteristicAdd, 
        CancellationToken token)
    {
        var characteristicId = await _characteristicService.AddAsync(CurrentUserId, id, characteristicAdd, token);
        return StatusCode(StatusCodes.Status201Created, characteristicId);
    }
    
    /// <summary>
    /// Метод для удаления характеристики объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="characteristicId">Идентификатор характеристики объявления.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}/characteristics/{characteristicId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteCharacteristicAsync([FromRoute] Guid id, [FromRoute] Guid characteristicId, CancellationToken token)
    {
        await _characteristicService.DeleteAsync(CurrentUserId, id, characteristicId, token);
        return NoContent();
    }

    /// <summary>
    /// Метод для обновления характеристики объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="characteristicId">Идентификатор характеристики объявления.</param>
    /// <param name="characteristicUpdate">Модель обновления характеристики объявления.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Модель обновленной информации о характеристике объявления.</returns>
    [HttpPatch("{id:guid}/characteristics/{characteristicId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(CharacteristicInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateCharacteristicAsync(
        [FromRoute] Guid id, 
        [FromRoute] Guid characteristicId,
        [FromBody] CharacteristicUpdate characteristicUpdate,
        CancellationToken token)
    {
        var characteristic = await _characteristicService.UpdateAsync(CurrentUserId, id, characteristicId, characteristicUpdate, token);
        return Ok(characteristic);
    }

    /// <summary>
    /// Метод для добавления фотографии объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="file">Файл.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Идентификатор добавленной фотографии.</returns>
    [HttpPost("{id:guid}/images")]
    [Authorize]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UploadImageAsync(
        [FromRoute] Guid id,
        IFormFile file,
        CancellationToken token)
    {
        var imageUpload = GetFileUpload(file);
        var imageId = await _advertImageService.UploadAsync(CurrentUserId, id, imageUpload, token);
        return StatusCode(StatusCodes.Status201Created, imageId);
    }
    
    /// <summary>
    /// Метод для удаления фотографии объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="imageId">Идентификатор фотографии.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}/images/{imageId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteImageAsync(
        [FromRoute] Guid id,
        [FromRoute] Guid imageId,
        CancellationToken token)
    {
        await _advertImageService.DeleteAsync(CurrentUserId, id, imageId, token);
        return NoContent();
    }
    
    /// <summary>
    /// Метод для создания комментария объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="commentCreate">Модель создания комментария.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Идентификатор созданного комментария.</returns>
    [HttpPost("{id:guid}/comments")]
    [Authorize]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCommentAsync(
        [FromRoute] Guid id, 
        [FromBody] CommentCreate commentCreate, 
        CancellationToken token)
    {
        var commentId = await _commentService.CreateAsync(CurrentUserId, id, commentCreate, token);
        return StatusCode(StatusCodes.Status201Created, commentId);
    }

    /// <summary>
    /// Метод для получения комментариев объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="search">Модель поиска комментариев.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Коллекция комментариев.</returns>
    [HttpPost("{id:guid}/comments/search")]
    [ProducesResponseType(typeof(IReadOnlyCollection<CommentInfo>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchCommentsAsync([FromRoute]Guid id, [FromBody] CommentsSearch search, CancellationToken token)
    {
        var comments = await _commentService.SearchAsync(id, search, token);
        return Ok(comments);
    }
}