using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Adverts.Services;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Services;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using ClassifiedsApi.Contracts.Contexts.Characteristics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassifiedsApi.Api.Controllers;

/// <summary>
/// Контроллер для работы с объявлениями.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class AdvertController : ApplicationController
{
    private readonly IAdvertService _advertService;
    private readonly ICharacteristicService _characteristicService;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertController"/>.
    /// </summary>
    /// <param name="httpContextAccessor">Средство доступа к HTTP-контексту.</param>
    /// <param name="advertService">Сервис объявлений <see cref="IAdvertService"/>.</param>
    /// <param name="characteristicService">Сервис характеристик объявлений <see cref="ICharacteristicService"/>.</param>
    public AdvertController(
        IHttpContextAccessor httpContextAccessor, 
        IAdvertService advertService,
        ICharacteristicService characteristicService) 
        : base(httpContextAccessor)
    {
        _advertService = advertService;
        _characteristicService = characteristicService;
    }
    
    /// <summary>
    /// Метод для создания нового объявления.
    /// </summary>
    /// <param name="advertCreate">Модель создания объявления.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Идентификатор созданного объявления.</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> CreateAsync([FromBody] AdvertCreate advertCreate, CancellationToken token)
    {
        var advertCreateRequest = GetUserRequest(advertCreate);
        var id = await _advertService.CreateAsync(advertCreateRequest, token);
        return StatusCode((int)HttpStatusCode.Created, id);
    }
    
    /// <summary>
    /// Метод для получения информации об объявлении.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Модель информации об объявлении.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AdvertInfo), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetInfoAsync([FromRoute] Guid id, CancellationToken token)
    {
        var advertInfo = await _advertService.GetByIdAsync(id, token);
        return Ok(advertInfo);
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
    [ProducesResponseType(typeof(AdvertInfo), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] AdvertUpdate advertUpdate, CancellationToken token)
    {
        var advertUpdateRequest = GetUserAdvertRequest(id, advertUpdate);
        var advertInfo = await _advertService.UpdateAsync(advertUpdateRequest, token);
        return Ok(advertInfo);
    }
    
    /// <summary>
    /// Метод для удаления объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken token)
    {
        await _advertService.DeleteAsync(id, CurrentUserId, token);
        return Ok();  
    }

    /// <summary>
    /// Метод для добавления новой характеристики объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="characteristicAdd">Модель добавления характеристики объявления.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Идентификатор добавленной характеристики объявления.</returns>
    [HttpPost("{id:guid}/characteristic")]
    [Authorize]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> AddCharacteristicAsync(
        [FromRoute] Guid id, 
        [FromBody] CharacteristicAdd characteristicAdd, 
        CancellationToken token)
    {
        var characteristicAddRequest = GetUserAdvertRequest(id, characteristicAdd);
        var characteristicId = await _characteristicService.AddAsync(characteristicAddRequest, token);
        return StatusCode((int)HttpStatusCode.Created, characteristicId);
    }
    
    /// <summary>
    /// Метод для удаления характеристики объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="characteristicId">Идентификатор характеристики объявления.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}/characteristic/{characteristicId:guid}")]
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteCharacteristicAsync([FromRoute] Guid id, [FromRoute] Guid characteristicId, CancellationToken token)
    {
        var characteristicDeleteRequest = new CharacteristicDeleteRequest
        {
            UserId = CurrentUserId,
            AdvertId = id,
            CharacteristicId = characteristicId
        };
        await _characteristicService.DeleteAsync(characteristicDeleteRequest, token);
        return Ok();
    }

    /// <summary>
    /// Метод для обновления характеристики объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="characteristicId">Идентификатор характеристики объявления.</param>
    /// <param name="characteristicUpdate">Модель обновления характеристики объявления.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Модель обновленной информации о характеристике объявления.</returns>
    [HttpPatch("{id:guid}/characteristic/{characteristicId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(CharacteristicInfo), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateCharacteristicAsync(
        [FromRoute] Guid id, 
        [FromRoute] Guid characteristicId,
        [FromBody] CharacteristicUpdate characteristicUpdate,
        CancellationToken token)
    {
        var characteristicUpdateRequest = new CharacteristicUpdateRequest
        {
            UserId = CurrentUserId,
            AdvertId = id,
            CharacteristicId = characteristicId,
            Model = characteristicUpdate
        };
        var characteristic = await _characteristicService.UpdateAsync(characteristicUpdateRequest, token);
        return Ok(characteristic);
    }
}