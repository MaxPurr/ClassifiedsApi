using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClassifiedsApi.AppServices.Contexts.Users.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Users;
using ClassifiedsApi.Contracts.Contexts.Users;
using ClassifiedsApi.DataAccess.DbContexts;
using ClassifiedsApi.Domain.Entities;
using ClassifiedsApi.Infrastructure.Repository.Sql;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.DataAccess.Repositories;

/// <inheritdoc />
public class UserRepository : IUserRepository
{
    private readonly ISqlRepository<User, ApplicationDbContext> _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="UserRepository"/>.
    /// </summary>
    /// <param name="repository">Глуппый репозиторий <see cref="ISqlRepository{TEntity, TContext}"/>.</param>
    /// <param name="mapper">Маппер <see cref="IMapper"/>.</param>
    public UserRepository(
        ISqlRepository<User, ApplicationDbContext> repository, 
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<UserInfo> GetByIdAsync(Guid id, CancellationToken token)
    {
        var userInfo = await _repository
            .GetByPredicate(user => user.Id == id)
            .ProjectTo<UserInfo>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(token);
        if (userInfo == null)
        {
            throw new UserNotFoundException();
        }
        return userInfo;
    }
    
    /// <inheritdoc />
    public async Task<UserContactsInfo> GetContactsInfoAsync(Guid id, CancellationToken token)
    {
        var contactsInfo = await _repository
            .GetByPredicate(user => user.Id == id)
            .ProjectTo<UserContactsInfo>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(token);
        if (contactsInfo == null)
        {
            throw new UserNotFoundException();
        }
        return contactsInfo;
    }
    
    /// <inheritdoc />
    public async Task<Guid?> UpdatePhotoAsync(Guid id, Guid photoId, CancellationToken token)
    {
        var user = await _repository.FirstOrDefaultAsync(user => user.Id == id, token);
        if (user == null)
        {
            throw new UserNotFoundException();
        }
        var prevPhotoId = user.PhotoId;
        user.PhotoId = photoId;
        await _repository.UpdateAsync(user, token);
        return prevPhotoId;
    }
    
    /// <inheritdoc />
    public async Task<Guid> DeletePhotoAsync(Guid id, CancellationToken token)
    {
        var user = await _repository.FirstOrDefaultAsync(user => user.Id == id, token);
        if (user == null)
        {
            throw new UserNotFoundException();
        }
        if (!user.PhotoId.HasValue)
        {
            throw new UserPhotoNotExistsException();
        }
        var prevPhotoId = user.PhotoId.Value;
        user.PhotoId = null;
        await _repository.UpdateAsync(user, token);
        return prevPhotoId;
    }
    
    /// <inheritdoc />
    public Task<bool> IsExistsAsync(Guid id, CancellationToken token)
    {
        return _repository.IsAnyExistAsync(user => user.Id == id, token);
    }
    
    /// <inheritdoc />
    public Task<bool> IsExistsAsync(string login, CancellationToken token)
    {
        return _repository.IsAnyExistAsync(user => user.Login == login, token);
    }
}