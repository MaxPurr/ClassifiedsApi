using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Categories.Services;
using ClassifiedsApi.Contracts.Contexts.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassifiedsApi.Api.Controllers;

/// <summary>
/// Контроллер для работы с категориями.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CategoryController"/>.
    /// </summary>
    /// <param name="service">Сервис категорий <see cref="ICategoryService"/>.</param>
    public CategoryController(ICategoryService service)
    {
        _service = service;
    }
    
    /// <summary>
    /// Метод для создания новой категории.
    /// </summary>
    /// <param name="categoryCreate">Модель создания категории.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Идентификатор новой категории.</returns>
    [HttpPost]
    // [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateAsync([FromBody] CategoryCreate categoryCreate, CancellationToken token)
    {
        var id = await _service.CreateAsync(categoryCreate, token);
        return StatusCode(StatusCodes.Status201Created, id);
    }
    
    /// <summary>
    /// Метод для получения информации о категории.
    /// </summary>
    /// <param name="id">Идентификатор категории.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Модель информации о категории.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CategoryInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInfoAsync([FromRoute] Guid id, CancellationToken token)
    {
        var categoryInfo = await _service.GetInfoAsync(id, token);
        return Ok(categoryInfo);
    }
    
    /// <summary>
    /// Метод для поиска категорий по заданному условию.
    /// </summary>
    /// <param name="search">Модель поиска категорий.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Список категорий.</returns>
    [HttpPost("search")]
    [ProducesResponseType(typeof(IReadOnlyCollection<CategoryInfo>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchAsync([FromBody] CategoriesSearch search, CancellationToken token)
    {
        var categories = await _service.SearchAsync(search, token);
        return Ok(categories);
    }
    
    /// <summary>
    /// Метод для удаления категории.
    /// </summary>
    /// <param name="id">Идентификатор категории.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken token)
    {
        await _service.DeleteAsync(id, token);
        return NoContent();
    }
    
    /// <summary>
    /// Метод для обновления категории.
    /// </summary>
    /// <param name="id">Идентификатор категории.</param>
    /// <param name="categoryUpdate">Модель обновления категории.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Модель обновленной информации о категории.</returns>
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] CategoryUpdate categoryUpdate, CancellationToken token)
    {
        var categoryInfo = await _service.UpdateAsync(id, categoryUpdate, token);
        return Ok(categoryInfo);
    }
}