using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using ClassifiedsApi.Contracts.Contexts.Characteristics;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Services;

/// <summary>
/// Сервис характеристик объявлений.
/// </summary>
public interface ICharacteristicService
{
    /// <summary>
    /// Метод для добавления новой характеристики объявления.
    /// </summary>
    /// <param name="addRequest">Модель пользовательского запроса на добавление характеристики объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор новой характеристики объявления.</returns>
    Task<Guid> AddAsync(AdvertRequest<CharacteristicAdd> addRequest, CancellationToken token);
    
    /// <summary>
    /// Метод для обновления характеристики объявления.
    /// </summary>
    /// <param name="updateRequest">Модель пользовательского запроса на обновление харакатеристики объявления <see cref="CharacteristicUpdateRequest"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель обновленной информации о характеристике объявления.</returns>
    Task<CharacteristicInfo> UpdateAsync(CharacteristicUpdateRequest updateRequest, CancellationToken token);
    
    /// <summary>
    /// Метод для удаление характеристики объявления.
    /// </summary>
    /// <param name="deleteRequest">Модель пользовательского запроса на удаление харакатеристики объявления <see cref="CharacteristicDeleteRequest"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAsync(CharacteristicDeleteRequest deleteRequest, CancellationToken token);
}