using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Files.Services;
using ClassifiedsApi.Contracts.Contexts.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassifiedsApi.Api.Controllers;

/// <summary>
/// Контроллер для работы с файлами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class FileController : ControllerBase
{
    private readonly IFileService _service;
    
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="FileController"/>.
    /// </summary>
    /// <param name="service">Сервис для работы с файлами <see cref="IFileService"/>.</param>
    public FileController(IFileService service)
    {
        _service = service;
    }
    
    /// <summary>
    /// Метод для получения информации о файле по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор файла.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Модель информации о файле.</returns>
    [HttpGet("{id:guid}/info")]
    [ProducesResponseType(typeof(FileInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInfoAsync([FromRoute] Guid id, CancellationToken token)
    {
        var fileInfo = await _service.GetInfoAsync(id, token);
        return Ok(fileInfo);
    }
    
    /// <summary>
    /// Метод для скачивания файла с сервера по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор файла.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(FileInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadAsync([FromRoute] Guid id, CancellationToken token)
    {
        var fileDownload = await _service.DownloadAsync(id, token);
        Response.ContentType = fileDownload.ContentType;
        return File(fileDownload.Content, fileDownload.ContentType, fileDownload.Name);
    }
}