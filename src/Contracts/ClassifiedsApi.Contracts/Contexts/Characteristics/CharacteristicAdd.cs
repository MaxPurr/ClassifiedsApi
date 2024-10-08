namespace ClassifiedsApi.Contracts.Contexts.Characteristics;

/// <summary>
/// Модель добавления характеристики объявления.
/// </summary>
public class CharacteristicAdd
{
    /// <summary>
    /// Название.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Значение.
    /// </summary>
    public string? Value { get; set; }
}