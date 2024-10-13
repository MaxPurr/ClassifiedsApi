using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClassifiedsApi.AppServices.Contexts.Files.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Files;
using ClassifiedsApi.Contracts.Contexts.Files;
using ClassifiedsApi.DataAccess.DbContexts;
using ClassifiedsApi.Domain.Entities;
using ClassifiedsApi.Infrastructure.Repository.Sql;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.DataAccess.Repositories;

/// <inheritdoc/>
public class FileRepository : IFileRepository
{
    private readonly ISqlRepository<File, ApplicationDbContext> _repository;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="FileRepository"/>.
    /// </summary>
    /// <param name="repository">Глупый репозиторий <see cref="ISqlRepository{TEntity, TContext}"/>.</param>
    /// <param name="mapper">Маппер объектов <see cref="IMapper"/>.</param>
    public FileRepository(ISqlRepository<File, ApplicationDbContext> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    /// <inheritdoc/>
    public async Task<Guid> UploadAsync(FileUpload fileUpload, CancellationToken token)
    {
        var file = _mapper.Map<File>(fileUpload);
        await _repository.AddAsync(file, token);
        return file.Id;
    }
    
    /// <inheritdoc/>
    public async Task<FileInfo> GetInfoAsync(Guid id, CancellationToken token)
    {
        var fileInfo = await _repository
            .GetByPredicate(file => file.Id == id)
            .ProjectTo<FileInfo>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(token);
        if (fileInfo == null)
        {
            throw new FileNotFoundException();
        }
        return fileInfo;
    }
    
    /// <inheritdoc/>
    public async Task<FileDownload> DownloadAsync(Guid id, CancellationToken token)
    {
        var fileDownload = await _repository
            .GetByPredicate(file => file.Id == id)
            .ProjectTo<FileDownload>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(token);
        if (fileDownload == null)
        {
            throw new FileNotFoundException();
        }
        return fileDownload;
    }
    
    /// <inheritdoc/>
    public async Task DeleteAsync(Guid id, CancellationToken token)
    {
        var success = await _repository.DeleteFirstAsync(file => file.Id == id, token);
        if (!success)
        {
            throw new FileNotFoundException();
        }
    }
    
    /// <inheritdoc/>
    public Task DeleteRangeAsync(IEnumerable<Guid> ids, CancellationToken token)
    {
        return _repository.DeleteByPredicateAsync(file => ids.Contains(file.Id), token);
    }
}