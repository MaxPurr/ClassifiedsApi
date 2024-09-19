using System.Linq.Expressions;
using ClassifiedsApi.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.Infrastructure.Repository;

public interface IRepository<TEntity, TContext> 
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