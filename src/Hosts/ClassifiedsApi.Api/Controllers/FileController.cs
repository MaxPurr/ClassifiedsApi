using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Files.Services;
using ClassifiedsApi.Contracts.Contexts.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassifiedsApi.Api.Controllers;

/// <summary>
/// Контроллер для работы с файлами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
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
    /// Метод для отправки файла на сервер.
    /// </summary>
    /// <param name="file">Файл.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Идентификатор отправленного файла.</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> UploadAsync(IFormFile file, CancellationToken token)
    {
        var fileUpload = new FileUpload
        {
            Name = file.FileName,
            ReadStream = file.OpenReadStream(),
            ContentType = file.ContentType,
        };
        var id = await _service.UploadAsync(fileUpload, token);
        return StatusCode((int)HttpStatusCode.Created, id);
    }
    
    /// <summary>
    /// Метод для получения информации о файле по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор файла.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Модель информации о файле.</returns>
    [HttpGet("{id}/info")]
    [ProducesResponseType(typeof(FileInfo), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetInfoAsync([FromRoute] string id, CancellationToken token)
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
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DownloadAsync([FromRoute] string id, CancellationToken token)
    {
        var fileDownload = await _service.DownloadAsync(id, token);
        Response.ContentType = fileDownload.ContentType;
        return File(fileDownload.ReadStream, fileDownload.ContentType, fileDownload.Name);
    }
    
    /// <summary>
    /// Метод для удаления файла по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор файла.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id, CancellationToken token)
    {
        await _service.DeleteAsync(id, token);
        return Ok();
    }
}