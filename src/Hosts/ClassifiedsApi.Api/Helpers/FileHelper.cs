using System.IO;
using Microsoft.AspNetCore.Http;

namespace ClassifiedsApi.Api.Helpers;

/// <summary>
/// Класс-помощник для работы с файлами.
/// </summary>
public static class FileHelper
{
    /// <summary>
    /// Метод для получения массива байтов из файла.
    /// </summary>
    /// <param name="file">Файл.</param>
    /// <returns>Массив байтов.</returns>
    public static byte[] GetByteArray(IFormFile file)
    {
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        var bytes = ms.ToArray();
        return bytes;
    }
}