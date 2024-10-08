using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.Infrastructure.Repository.Sql;

/// <inheritdoc />
public class SqlRepository<TEntity, TContext> : ISqlRepository<TEntity, TContext> 
    where TEntity : class, ISqlEntity 
    where TContext : DbContext
{
    private readonly TContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="SqlRepository{TEntity, TContext}"></see>.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public SqlRepository(TContext context)
    {
        _dbContext = context;
        _dbSet = _dbContext.Set<TEntity>();
    }
    
    /// <inheritdoc />
    public IQueryable<TEntity> GetAll()
    {
        return _dbSet;
    }
    
    /// <inheritdoc />
    public IQueryable<TEntity> GetByPredicate(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }
    
    /// <inheritdoc />
    public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return FirstOrDefaultAsync(entity => entity.Id == id, token);
    }
    
    /// <inheritdoc />
    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token)
    {
        return _dbSet.FirstOrDefaultAsync(predicate, token);
    }

    /// <inheritdoc />
    public async Task AddAsync(TEntity entity, CancellationToken token)
    {
        await _dbSet.AddAsync(entity, token);
        await _dbContext.SaveChangesAsync(token);
    }
    
    /// <inheritdoc />
    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken token)
    {
        await _dbSet.AddRangeAsync(entities, token);
        await _dbContext.SaveChangesAsync(token);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(TEntity entity, CancellationToken token)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync(token);
    }
    
    /// <inheritdoc />
    public async Task<bool> DeleteFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token)
    {
        var entity = await FirstOrDefaultAsync(predicate, token);
        if (entity == null)
        {
            return false;
        }
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync(token);
        return true;;
    }

    /// <inheritdoc />
    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token)
    {
        return DeleteFirstAsync(entity => entity.Id == id, token);
    }

    /// <inheritdoc />
    public Task<bool> IsExistAsync(Guid id, CancellationToken token)
    {
        return _dbSet.AnyAsync(entity => entity.Id == id, token);
    }
}