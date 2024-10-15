using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Accounts.Services;
using ClassifiedsApi.Contracts.Common.Errors;
using ClassifiedsApi.Contracts.Contexts.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassifiedsApi.Api.Controllers;

/// <summary>
/// Контроллер для работы с аккаунтами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IJwtService _jwtService;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AccountController"/>.
    /// </summary>
    /// <param name="accountService">Сервис аккаунтов <see cref="IAccountService"/>.</param>
    /// <param name="jwtService">Сервис для работы с JWT <see cref="IJwtService"/>.</param>
    public AccountController(IAccountService accountService, IJwtService jwtService)
    {
        _accountService = accountService;
        _jwtService = jwtService;
    }
    
    /// <summary>
    /// Метод для регистрации нового аккаунта.
    /// </summary>
    /// <param name="accountRegister">Модель регистрации нового аккаунта.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Идентификатор нового аккаунта.</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterAsync([FromBody] AccountRegister accountRegister, CancellationToken token)
    {
        var id = await _accountService.RegisterAsync(accountRegister, token);
        return StatusCode(StatusCodes.Status201Created, id);
    }
    
    /// <summary>
    /// Метод для получения токена доступа.
    /// </summary>
    /// <param name="accountVerify">Модель для проверки учетных данных аккаунта.</param>
    /// <param name="token">Токен отмены операции.</param>
    /// <returns>Токен доступа.</returns>
    [HttpPost("token")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginAsync([FromBody] AccountVerify accountVerify, CancellationToken token)
    {
        var accessToken = await _accountService.GetAccessTokenAsync(accountVerify, token);
        return Ok(accessToken);
    }
}