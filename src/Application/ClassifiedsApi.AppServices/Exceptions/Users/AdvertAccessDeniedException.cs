using ClassifiedsApi.AppServices.Exceptions.Common;

namespace ClassifiedsApi.AppServices.Exceptions.Users;

/// <summary>
/// Исключение, возникающее когда доступа к запрашиваемому объявлению запрещен.
/// </summary>
public class AdvertAccessDeniedException : ResourceAccessDeniedException
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertAccessDeniedException"/>.
    /// </summary>
    public AdvertAccessDeniedException() : base("Доступ к запрашиваемому объявлению запрещен.")
    {
        
    }
}