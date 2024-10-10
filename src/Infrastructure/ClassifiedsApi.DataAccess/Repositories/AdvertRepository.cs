using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Advert;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using ClassifiedsApi.Contracts.Contexts.Users;
using ClassifiedsApi.DataAccess.DbContexts;
using ClassifiedsApi.Domain.Entities;
using ClassifiedsApi.Infrastructure.Repository.Sql;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.DataAccess.Repositories;

/// <inheritdoc />
public class AdvertRepository : IAdvertRepository
{
    private readonly ISqlRepository<Advert, ApplicationDbContext> _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertRepository"/>.
    /// </summary>
    /// <param name="repository">Глуппый репозиторий <see cref="ISqlRepository{TEntity, TContext}"/>.</param>
    /// <param name="mapper">Маппер.</param>
    public AdvertRepository(ISqlRepository<Advert, ApplicationDbContext> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(UserRequest<AdvertCreate> advertCreateRequest, CancellationToken token)
    {
        var advert = _mapper.Map<Advert>(advertCreateRequest);
        await _repository.AddAsync(advert, token);
        return advert.Id;
    }
    
    /// <inheritdoc />
    public async Task<AdvertInfo> GetByIdAsync(Guid id, CancellationToken token)
    {
        var advert = await _repository
            .GetByPredicate(advert => advert.Id == id)
            .Include(advert => advert.Images)
            .Include(advert => advert.Characteristics)
            .ProjectTo<AdvertInfo>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(token);
        if (advert == null)
        {
            throw new AdvertNotFoundException();
        }
        return advert;
    }

    /// <inheritdoc />
    public async Task<AdvertInfo> UpdateAsync(Guid id, AdvertUpdate advertUpdate, CancellationToken token)
    {
        var advert = await _repository.FirstOrDefaultAsync(advert => advert.Id == id, token);
        if (advert == null)
        {
            throw new AdvertNotFoundException();
        }
        if (advertUpdate.Title != null)
        {
            advert.Title = advertUpdate.Title;
        }
        if (advertUpdate.Description != null)
        {
            advert.Description = advertUpdate.Description;
        }
        if (advertUpdate.Price.HasValue)
        {
            advert.Price = advertUpdate.Price.Value;
        }
        if (advertUpdate.CategoryId.HasValue)
        {
            advert.CategoryId = advertUpdate.CategoryId.Value;
        }
        await _repository.UpdateAsync(advert, token);
        return _mapper.Map<AdvertInfo>(advert);
    }
    
    /// <inheritdoc />
    public async Task DeleteAsync(Guid id, CancellationToken token)
    { 
        var success = await _repository.DeleteFirstAsync(advert => advert.Id == id, token);
        if (!success)
        {
            throw new AdvertNotFoundException();
        }
    }

    /// <inheritdoc />
    public Task<bool> IsExistsAsync(Guid id, CancellationToken token)
    {
        return _repository.IsAnyExistAsync(advert => advert.Id == id, token);
    }
    
    /// <inheritdoc />
    public Task<bool> IsExistsAsync(Guid userId, Guid advertId, CancellationToken token)
    {
        return _repository.IsAnyExistAsync(advert => advert.Id == advertId && advert.UserId == userId, token);
    }
}