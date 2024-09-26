using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Files.Services;
using ClassifiedsApi.Contracts.Contexts.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassifiedsApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class FileController : ControllerBase
{
    private readonly IFileService _service;

    public FileController(IFileService service)
    {
        _service = service;
    }

    [HttpPost]
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
    
    [HttpGet("{id}/info")]
    [ProducesResponseType(typeof(FileInfo), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetFileInfoAsync(string id, CancellationToken token)
    {
        var fileInfo = await _service.GetFileInfoAsync(id, token);
        return Ok(fileInfo);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(FileDownload), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DownloadAsync(string id, CancellationToken token)
    {
        var fileDownload = await _service.DownloadAsync(id, token);
        Response.ContentType = fileDownload.ContentType;
        return File(fileDownload.ReadStream, fileDownload.ContentType, fileDownload.Name);
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteAsync(string id, CancellationToken token)
    {
        await _service.DeleteAsync(id, token);
        return Ok();
    }
}