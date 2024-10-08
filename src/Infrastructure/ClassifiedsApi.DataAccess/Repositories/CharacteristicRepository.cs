using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Characteristics;
using ClassifiedsApi.Contracts.Common.Requests;
using ClassifiedsApi.Contracts.Contexts.Characteristics;
using ClassifiedsApi.DataAccess.DbContexts;
using ClassifiedsApi.Domain.Entities;
using ClassifiedsApi.Infrastructure.Repository.Sql;
using Microsoft.EntityFrameworkCore;

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
    public async Task<Guid> AddAsync(UserAdvertRequest<CharacteristicAdd> characteristicAddRequest, CancellationToken token)
    {
        var advertCharacteristic = _mapper.Map<Characteristic>(characteristicAddRequest);
        await _repository.AddAsync(advertCharacteristic, token);
        return advertCharacteristic.Id;
    }
    
    /// <inheritdoc />
    public async Task DeleteAsync(CharacteristicDeleteRequest characteristicDeleteRequest, CancellationToken token)
    {
        var success = await _repository.DeleteFirstAsync(characteristic => 
            characteristic.Id == characteristicDeleteRequest.CharacteristicId &&
            characteristic.AdvertId == characteristicDeleteRequest.AdvertId, 
            token);
        if (!success)
        {
            throw new CharacteristicNotFoundException();
        }
    }

    /// <inheritdoc />
    public async Task<CharacteristicInfo> UpdateAsync(CharacteristicUpdateRequest characteristicUpdateRequest, CancellationToken token)
    {
        var characteristic = await _repository.FirstOrDefaultAsync(characteristic => 
            characteristic.AdvertId == characteristicUpdateRequest.AdvertId && 
            characteristic.Id == characteristicUpdateRequest.CharacteristicId,
            token);
        if (characteristic == null)
        {
            throw new CharacteristicNotFoundException();
        }
        var characteristicUpdate = characteristicUpdateRequest.Model;
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
    public Task<bool> IsExistsAsync(Guid advertId, string name, CancellationToken token)
    {
        return _repository
            .GetAll()
            .AnyAsync(characteristic => characteristic.AdvertId == advertId && characteristic.Name == name, token);
    }
}