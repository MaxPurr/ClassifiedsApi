using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using ClassifiedsApi.AppServices.Helpers;
using ClassifiedsApi.AppServices.Settings;
using ClassifiedsApi.Contracts.Contexts.Accounts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ClassifiedsApi.AppServices.Contexts.Accounts.Services;

/// <inheritdoc />
public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="JwtService"/>.
    /// </summary>
    /// <param name="jwtSettings">Параметры генерации JWT.</param>
    public JwtService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
    
    /// <inheritdoc />
    public string GetToken(AccountInfo accountInfo)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, accountInfo.Id.ToString()),
            new Claim(ClaimTypes.Name, accountInfo.Login)
        };
        claims.AddRange(accountInfo.RoleNames.Select(role => new Claim(ClaimTypes.Role, role)));
        
        var securityKey = CryptoHelper.GetSymmetricSecurityKey(_jwtSettings.SigningKey);
        var expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_jwtSettings.ValidityPeriodInMinutes));
        var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return token;
    }
}