using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Accounts.Services;
using ClassifiedsApi.Contracts.Contexts.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace ClassifiedsApi.Api.Controllers;

/// <summary>
/// Контроллер для работы с аккаунтами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
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
    /// <param name="accountRegister">Модель регистрации нового аккаунта <see cref="AccountRegister"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор нового аккаунта.</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(Guid) ,(int)HttpStatusCode.Created)]
    public async Task<IActionResult> RegisterAsync(AccountRegister accountRegister, CancellationToken token)
    {
        var id = await _accountService.RegisterAsync(accountRegister, token);
        return StatusCode((int)HttpStatusCode.Created, id);
    }
    
    /// <summary>
    /// Метод для получения токена доступа.
    /// </summary>
    /// <param name="accountVerify">Модель для проверки учетных данных аккаунта <see cref="AccountVerify"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Токен доступа.</returns>
    [HttpPost("token")]
    [ProducesResponseType(typeof(AccessToken) ,(int)HttpStatusCode.OK)]
    public async Task<IActionResult> LoginAsync(AccountVerify accountVerify, CancellationToken token)
    {
        var accountInfo = await _accountService.GetInfoAsync(accountVerify, token);
        var accessToken = _jwtService.GetToken(accountInfo);
        return Ok(accessToken);
    }
}