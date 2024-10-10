using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Characteristics;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using ClassifiedsApi.Contracts.Contexts.Characteristics;
using ClassifiedsApi.DataAccess.DbContexts;
using ClassifiedsApi.Domain.Entities;
using ClassifiedsApi.Infrastructure.Repository.Sql;

namespace ClassifiedsApi.DataAccess.Repositories;

/// <inheritdoc />
public class CharacteristicRepository : ICharacteristicRepository
{
    private readonly ISqlRepository<Characteristic, ApplicationDbContext> _repository;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicRepository"/>.
    /// </summary>
    /// <param name="repository">Глуппый репозиторий <see cref="ISqlRepository{TEntity, TContext}"/>.</param>
    /// <param name="mapper">Маппер <see cref="IMapper"/>.</param>
    public CharacteristicRepository(ISqlRepository<Characteristic, ApplicationDbContext> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    /// <inheritdoc />
    public async Task<Guid> AddAsync(AdvertRequest<CharacteristicAdd> characteristicAddRequest, CancellationToken token)
    {
        var advertCharacteristic = _mapper.Map<Characteristic>(characteristicAddRequest);
        await _repository.AddAsync(advertCharacteristic, token);
        return advertCharacteristic.Id;
    }
    
    /// <inheritdoc />
    public async Task DeleteAsync(Guid advertId, Guid id, CancellationToken token)
    {
        var success = await _repository.DeleteFirstAsync(characteristic => 
            characteristic.AdvertId == advertId &&
            characteristic.Id == id, 
            token);
        if (!success)
        {
            throw new CharacteristicNotFoundException();
        }
    }

    /// <inheritdoc />
    public async Task<CharacteristicInfo> UpdateAsync(Guid advertId, Guid id, CharacteristicUpdate characteristicUpdate, CancellationToken token)
    {
        var characteristic = await _repository.FirstOrDefaultAsync(characteristic => 
            characteristic.AdvertId == advertId &&
            characteristic.Id == id, 
            token);
        if (characteristic == null)
        {
            throw new CharacteristicNotFoundException();
        }
        if (characteristicUpdate.Name != null)
        {
            characteristic.Name = characteristicUpdate.Name;
        }
        if (characteristicUpdate.Value != null)
        {
            characteristic.Value = characteristicUpdate.Value;
        }
        await _repository.UpdateAsync(characteristic, token);
        return _mapper.Map<CharacteristicInfo>(characteristic);
    }

    /// <inheritdoc />
    public Task<bool> IsExistsAsync(Guid advertId, string characteristicName, CancellationToken token)
    {
        return _repository.IsAnyExistAsync(characteristic => 
            characteristic.AdvertId == advertId && 
            characteristic.Name == characteristicName, 
            token);
    }
}