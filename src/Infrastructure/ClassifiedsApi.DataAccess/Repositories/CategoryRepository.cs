using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Categories;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Categories;
using ClassifiedsApi.DataAccess.DbContexts;
using ClassifiedsApi.Domain.Entities;
using ClassifiedsApi.Infrastructure.Repository.Sql;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.DataAccess.Repositories;

/// <inheritdoc />
public class CategoryRepository : ICategoryRepository
{
    private readonly ISqlRepository<Category, ApplicationDbContext> _repository;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CategoryRepository"/>.
    /// </summary>
    /// <param name="repository">Глупый репозиторий <see cref="ISqlRepository{TEntity, TContext}"/>.</param>
    /// <param name="mapper">Маппер <see cref="IMapper"/>.</param>
    public CategoryRepository(ISqlRepository<Category, ApplicationDbContext> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(CategoryCreate categoryCreate, CancellationToken token)
    {
        var category = _mapper.Map<Category>(categoryCreate);
        await _repository.AddAsync(category, token);
        return category.Id;
    }
    
    /// <inheritdoc />
    public async Task<CategoryInfo> GetInfoAsync(Guid id, CancellationToken token)
    {
        var categoryInfo = await _repository
            .GetByPredicate(category => category.Id == id)
            .ProjectTo<CategoryInfo>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(token);
        if (categoryInfo == null)
        {
            throw new CategoryNotFoundException();
        }
        return categoryInfo;
    }
    
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<CategoryInfo>> GetBySpecificationWithPaginationAsync(
        ISpecification<CategoryInfo> specification, 
        int? skip, 
        int take,
        CategoriesOrder order,
        CancellationToken token)
    {
        var query = _repository
            .GetAll()
            .ProjectTo<CategoryInfo>(_mapper.ConfigurationProvider)
            .Where(specification.PredicateExpression);
        var desc = order.Descending;
        query = order.By switch
        {
            CategoriesOrderBy.Id => desc 
                ? query.OrderByDescending(c => c.Id) 
                : query.OrderBy(c => c.Id),
            CategoriesOrderBy.Name => desc 
                ? query.OrderByDescending(c => c.Name) 
                : query.OrderBy(c => c.Name),
            CategoriesOrderBy.ParentId => desc 
                ? query.OrderByDescending(c => c.ParentId) 
                : query.OrderBy(c => c.ParentId),
            _ => query
        };
        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }
        return await query
            .Take(take)
            .ToListAsync(token);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id, CancellationToken token)
    {
        var success = await _repository.DeleteAsync(id, token);
        if (!success)
        {
            throw new CategoryNotFoundException();
        }
    }

    /// <inheritdoc />
    public async Task<CategoryInfo> UpdateAsync(Guid id, CategoryUpdate categoryUpdate, CancellationToken token)
    {
        var category = await _repository.GetByIdAsync(id, token);
        if (category == null)
        {
            throw new CategoryNotFoundException();
        }
        if (categoryUpdate.Name != null)
        {
            category.Name = categoryUpdate.Name;
        }
        await _repository.UpdateAsync(category, token);
        return _mapper.Map<CategoryInfo>(category);
    }
    
    /// <inheritdoc />
    public Task<bool> IsExistsAsync(Guid id, CancellationToken token)
    {
        return _repository.IsExistAsync(id, token);
    }
}