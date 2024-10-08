namespace ClassifiedsApi.Contracts.Contexts.Characteristics;

/// <summary>
/// Модель информации о характеристике обьявления.
/// </summary>
public class CharacteristicInfo
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Значение.
    /// </summary>
    public string Value { get; set; }
}