using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.Infrastructure.Repository.Sql;

public class SqlRepository<TEntity, TContext> : ISqlRepository<TEntity, TContext> 
    where TEntity : class 
    where TContext : DbContext
{
    public SqlRepository(TContext context)
    {
        DbContext = context;
        DbSet = DbContext.Set<TEntity>();
    }
    
    protected TContext DbContext { get; set; }
    protected DbSet<TEntity> DbSet { get; set; }
    
    public IQueryable<TEntity> GetAll()
    {
        return DbSet;
    }

    public IQueryable<TEntity> GetByPredicate(Expression<Func<TEntity, bool>> predicate)
    {
        return DbSet.Where(predicate);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await DbSet.FindAsync([id], token);
    }

    public async Task AddAsync(TEntity model, CancellationToken token)
    {
        await DbSet.AddAsync(model, token);
        await DbContext.SaveChangesAsync(token);
    }

    public async Task UpdateAsync(TEntity model, CancellationToken token)
    {
        DbSet.Update(model);
        await DbContext.SaveChangesAsync(token);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken token)
    {
        var entity = await GetByIdAsync(id, token);
        if (entity == null)
        {
            return false;
        }
        DbSet.Remove(entity);
        await DbContext.SaveChangesAsync(token);
        return true;
    }
}