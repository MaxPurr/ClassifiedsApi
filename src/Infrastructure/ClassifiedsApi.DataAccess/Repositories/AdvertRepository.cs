using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Advert;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Adverts;
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
    public AdvertRepository(
        ISqlRepository<Advert, ApplicationDbContext> repository, 
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(AdvertCreateRequest createRequest, CancellationToken token)
    {
        var advert = _mapper.Map<Advert>(createRequest);
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
    
    private static Expression<Func<ShortAdvertInfo, object?>> GetOrderByExpression(AdvertsOrderBy orderBy)
    {
        return orderBy switch
        {
            AdvertsOrderBy.Title => advert => advert.Title,
            AdvertsOrderBy.CreatedAt => advert => advert.CreatedAt,
            _ => advert => advert.Id,
        };
    }
    
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<ShortAdvertInfo>> GetBySpecificationWithPaginationAsync(
        ISpecification<ShortAdvertInfo> specification, 
        int? skip, 
        int take, 
        AdvertsOrder order,
        CancellationToken token)
    {
        var query = _repository
            .GetAll()
            .Include(advert => advert.Images)
            .ProjectTo<ShortAdvertInfo>(_mapper.ConfigurationProvider)
            .Where(specification.PredicateExpression);
        var orderByExpression = GetOrderByExpression(order.By);
        query = order.Descending 
            ? query.OrderByDescending(orderByExpression)
            : query.OrderBy(orderByExpression);
        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }
        return await query
            .Take(take)
            .ToArrayAsync(token);
    }

    /// <inheritdoc />
    public async Task<UpdatedAdvertInfo> UpdateAsync(Guid id, AdvertUpdate advertUpdate, CancellationToken token)
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
        if (advertUpdate.Disabled.HasValue)
        {
            advert.Disabled = advertUpdate.Disabled.Value;
        }
        await _repository.UpdateAsync(advert, token);
        return _mapper.Map<UpdatedAdvertInfo>(advert);
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
    public async Task<Guid> GetUserIdAsync(Guid id, CancellationToken token)
    {
        var userId = await _repository
            .GetByPredicate(advert => advert.Id == id)
            .Select<Advert, Guid?>(advert => advert.UserId)
            .FirstOrDefaultAsync(token);
        if (!userId.HasValue)
        {
            throw new AdvertNotFoundException();
        }
        return userId.Value;
    }
}