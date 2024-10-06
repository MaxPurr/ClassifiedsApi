using ClassifiedsApi.Contracts.Common;

namespace ClassifiedsApi.AppServices.Extensions;

/// <summary>
/// Расширения для модели поиска строки в тексте <see cref="TextFilter"/>.
/// </summary>
public static class TextFilterExtensions
{
    /// <summary>
    /// Метод для получения регулярного выражения из модели поиска.
    /// </summary>
    /// <param name="filter">Модель поиска строки в тексте <see cref="TextFilter"/>.</param>
    /// <returns>Регулярное выражение</returns>
    public static string GetRegularExpression(this TextFilter filter)
    {
        var query = filter.IgnoreCase ? filter.Query.ToLower() : filter.Query;
        return filter.QueryPosition switch
        {
            QueryPosition.Start => $"{query}%",
            QueryPosition.End => $"%{query}",
            QueryPosition.Anywhere => $"%{query}%",
            _ => string.Empty
        };
    }
}