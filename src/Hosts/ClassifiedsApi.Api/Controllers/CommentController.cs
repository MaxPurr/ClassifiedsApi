using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Api.Controllers.Base;
using ClassifiedsApi.AppServices.Contexts.Comments.Services;
using ClassifiedsApi.Contracts.Common.Errors;
using ClassifiedsApi.Contracts.Contexts.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassifiedsApi.Api.Controllers;

/// <summary>
/// Контроллер для работы с комментариями.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class CommentController : BaseApplicationController
{
    private readonly ICommentService _service;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CommentController"/>.
    /// </summary>
    /// <param name="httpContextAccessor">Средство доступа к HTTP-контексту.</param>
    /// <param name="commentService">Сервис комментариев <see cref="ICommentService"/>.</param>
    public CommentController(
        IHttpContextAccessor httpContextAccessor, 
        ICommentService commentService) 
        : base(httpContextAccessor)
    {
        _service = commentService;
    }
    
    /// <summary>
    /// Метод для получения информации о комментарии.
    /// </summary>
    /// <param name="id">Идентификатор комментария.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Модель информации о комментарии.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CommentInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInfoAsync([FromRoute] Guid id, CancellationToken token)
    {
        var comment = await _service.GetByIdAsync(id, token);
        return Ok(comment);
    }
    
    /// <summary>
    /// Метод для удаления комментария.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken token)
    {
        await _service.DeleteAsync(CurrentUserId, id, token);
        return NoContent();
    }

    /// <summary>
    /// Метод для обновления комментария.
    /// </summary>
    /// <param name="id">Идентификатор комментария.</param>
    /// <param name="commentUpdate">Модель обновления комментария.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Модель обновленной информации о комментарии.</returns>
    [HttpPatch("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(CommentInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] CommentUpdate commentUpdate, CancellationToken token)
    {
        var updatedComment = await _service.UpdateAsync(CurrentUserId, id, commentUpdate, token);
        return Ok(updatedComment);
    }
}