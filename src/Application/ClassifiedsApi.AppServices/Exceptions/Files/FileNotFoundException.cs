using ClassifiedsApi.AppServices.Exceptions.Common;

namespace ClassifiedsApi.AppServices.Exceptions.Files;

/// <summary>
/// Исключение, возникающее когда искомый файл не был найден.
/// </summary>
public class FileNotFoundException : EntityNotFoundException
{
    /// <summary>
    /// Инициализирует экземпляр <see cref="FileNotFoundException"/>.
    /// </summary>
    public FileNotFoundException() : base("Файл не был найден.")
    {
        
    }
}