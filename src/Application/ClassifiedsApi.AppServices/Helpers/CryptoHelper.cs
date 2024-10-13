using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ClassifiedsApi.AppServices.Helpers;

/// <summary>
/// Класс-помощник для работы с криптографией.
/// </summary>
public static class CryptoHelper
{
    /// <summary>
    /// Конвертирует строку в Base64.
    /// </summary>
    /// <param name="str">Строка.</param>
    /// <returns>Base64</returns>
    public static string GetBase64Hash(string str)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(str);
        byte[] hash = SHA256.HashData(buffer);
        return Convert.ToBase64String(hash);
    }
    
    /// <summary>
    /// Конвертирует строку в симметричный ключ безопасности.
    /// </summary>
    /// <param name="key">Строка для конвертации.</param>
    /// <returns>Симметричный ключ безопасности <see cref="SymmetricSecurityKey"/>.</returns>
    public static SymmetricSecurityKey GetSymmetricSecurityKey(string key)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    }
}