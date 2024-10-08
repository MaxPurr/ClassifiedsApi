namespace ClassifiedsApi.Contracts.Contexts.Characteristics;

/// <summary>
/// Модель обновления характеристики объявления.
/// </summary>
public class CharacteristicUpdate
{
    /// <summary>
    /// Новое название.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Новое значение.
    /// </summary>
    public string? Value { get; set; }
}