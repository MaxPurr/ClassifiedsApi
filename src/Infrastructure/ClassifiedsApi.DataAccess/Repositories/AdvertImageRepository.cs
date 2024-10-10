using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.AdvertImages.Repositories;
using ClassifiedsApi.AppServices.Exceptions.AdvertImages;
using ClassifiedsApi.DataAccess.DbContexts;
using ClassifiedsApi.Domain.Entities;
using ClassifiedsApi.Infrastructure.Repository.Sql;

namespace ClassifiedsApi.DataAccess.Repositories;

/// <inheritdoc />
public class AdvertImageRepository : IAdvertImageRepository
{
    private readonly ISqlRepository<AdvertImage, ApplicationDbContext> _repository;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertImageRepository"/>.
    /// </summary>
    /// <param name="repository">Глуппый репозиторий <see cref="ISqlRepository{TEntity, TContext}"/>.</param>
    public AdvertImageRepository(ISqlRepository<AdvertImage, ApplicationDbContext> repository)
    {
        _repository = repository;
    }
    
    /// <inheritdoc />
    public Task AddAsync(Guid advertId, string imageId, CancellationToken token)
    {
        var advertImage = new AdvertImage
        {
            ImageId = imageId,
            AdvertId = advertId,
        };
        return _repository.AddAsync(advertImage, token);
    }
    
    /// <inheritdoc />
    public async Task DeleteAsync(Guid advertId, string imageId, CancellationToken token)
    {
        var success = await _repository.DeleteFirstAsync(advertImage => 
            advertImage.AdvertId == advertId &&
            advertImage.ImageId == imageId, 
            token);
        if (!success)
        {
            throw new AdvertImageNotFoundException();
        }
    }
}