using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.Infrastructure.Repository.Sql;

public interface ISqlRepository<TEntity, TContext> 
    where TEntity : class 
    where TContext : DbContext
{
    IQueryable<TEntity> GetAll();
    
    IQueryable<TEntity> GetByPredicate(Expression<Func<TEntity, bool>> predicate);
    
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token);
    
    Task AddAsync(TEntity model, CancellationToken token);
    
    Task UpdateAsync(TEntity model, CancellationToken token);
    
    Task<bool> DeleteAsync(Guid id, CancellationToken token);
}