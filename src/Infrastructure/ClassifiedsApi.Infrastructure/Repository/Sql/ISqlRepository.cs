using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.Infrastructure.Repository.Sql;

/// <summary>
/// Глупый репозиторий для работы с SQL.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
/// <typeparam name="TContext">Тип контекста базы данных.</typeparam>
public interface ISqlRepository<TEntity, TContext> 
    where TEntity : class
    where TContext : DbContext
{
    DbContext Context { get; }
    
    /// <summary>
    /// Метод для получения всех сущностей.
    /// </summary>
    /// <returns>Список всех сущностей.</returns>
    IQueryable<TEntity> GetAll();
    
    /// <summary>
    /// Метод для получения сущностей, удовлетворяющих условию.
    /// </summary>
    /// <param name="predicate">Условие.</param>
    /// <returns>Список сущностей, удовлетворяющих условию.</returns>
    IQueryable<TEntity> GetByPredicate(Expression<Func<TEntity, bool>> predicate);
    
    /// <summary>
    /// Метод для получения первой сущности, удовлетворяющей условию.
    /// </summary>
    /// <param name="predicate">Условие.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Сущность если найдена, иначе null.</returns>
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token);
    
    /// <summary>
    /// Метод для добавления новой сущности в репозиторий.
    /// </summary>
    /// <param name="entity">Сущность.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task AddAsync(TEntity entity, CancellationToken token);
    
    /// <summary>
    /// Метод для обновления сущности.
    /// </summary>
    /// <param name="entity">Обновленная сущность.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task UpdateAsync(TEntity entity, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления первой сущности, удовлетворяющей условию.
    /// </summary>
    /// <param name="predicate">Условие.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если сущность найдена и удалена, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> DeleteFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token);
    
    /// <summary>
    /// Метод для проверки существования сущности в репозитории по заданному условию.
    /// </summary>
    /// <param name="predicate">Условие</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если сущность найдена, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> IsAnyExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token);
}