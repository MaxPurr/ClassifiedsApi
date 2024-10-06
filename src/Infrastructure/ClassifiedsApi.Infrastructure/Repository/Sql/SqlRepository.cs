using System;
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
    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await GetByPredicate(entity => entity.Id == id)
            .FirstOrDefaultAsync(token);
    }

    /// <inheritdoc />
    public async Task AddAsync(TEntity model, CancellationToken token)
    {
        await _dbSet.AddAsync(model, token);
        await _dbContext.SaveChangesAsync(token);
    }
    
    /// <inheritdoc />
    public async Task UpdateAsync(TEntity model, CancellationToken token)
    {
        _dbSet.Update(model);
        await _dbContext.SaveChangesAsync(token);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id, CancellationToken token)
    {
        var entity = await GetByIdAsync(id, token);
        if (entity == null)
        {
            return false;
        }
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync(token);
        return true;
    }

    /// <inheritdoc />
    public Task<bool> IsExistAsync(Guid id, CancellationToken token)
    {
        return _dbSet.AnyAsync(entity => entity.Id == id, token);
    }
}