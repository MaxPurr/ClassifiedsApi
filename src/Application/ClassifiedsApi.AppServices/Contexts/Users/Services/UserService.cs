using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Common.Services;
using ClassifiedsApi.AppServices.Contexts.Files.Services;
using ClassifiedsApi.AppServices.Contexts.Users.Repositories;
using ClassifiedsApi.Contracts.Contexts.Files;
using ClassifiedsApi.Contracts.Contexts.Users;

namespace ClassifiedsApi.AppServices.Contexts.Users.Services;

/// <inheritdoc cref="IUserService"/>
public class UserService : ServiceBase, IUserService
{
    private readonly IUserRepository _repository;
    private readonly IFileService _fileService;
    private readonly IFileVerifier _fileVerifier;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="UserService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий пользователей <see cref="IUserRepository"/>.</param>
    /// <param name="fileService">Сервис файлов <see cref="IFileService"/>.</param>
    /// <param name="fileVerifier">Верификатор файлов <see cref="IFileVerifier"/>.</param>
    public UserService(IUserRepository repository, IFileService fileService, IFileVerifier fileVerifier)
    {
        _repository = repository;
        _fileService = fileService;
        _fileVerifier = fileVerifier;
    }
    
    /// <inheritdoc />
    public Task<UserInfo> GetByIdAsync(Guid id, CancellationToken token)
    {
        return _repository.GetByIdAsync(id, token);
    }
    
    /// <inheritdoc />
    public Task<UserContactsInfo> GetContactsInfoAsync(Guid id, CancellationToken token)
    {
        return _repository.GetContactsInfoAsync(id, token);
    }
    
    /// <inheritdoc />
    public async Task<Guid> UpdatePhotoAsync(Guid id, FileUpload photoUpload, CancellationToken token)
    {
        _fileVerifier.VerifyImageContentTypeAndThrow(photoUpload.ContentType);
        
        using var scope = CreateTransactionScope();
        var photoId = await _fileService.UploadAsync(photoUpload, token);
        var prevPhotoId = await _repository.UpdatePhotoAsync(id, photoId, token);
        if (prevPhotoId.HasValue)
        {
            await _fileService.DeleteAsync(prevPhotoId.Value, token);
        }
        scope.Complete();
        return photoId;
    }
    
    /// <inheritdoc />
    public async Task DeletePhotoAsync(Guid id, CancellationToken token)
    {
        using var scope = CreateTransactionScope();
        var photoId = await _repository.DeletePhotoAsync(id, token);
        await _fileService.DeleteAsync(photoId, token);
        scope.Complete();
    }
}