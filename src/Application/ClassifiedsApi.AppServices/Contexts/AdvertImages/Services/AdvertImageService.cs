using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Common.Services;
using ClassifiedsApi.AppServices.Contexts.AdvertImages.Repositories;
using ClassifiedsApi.AppServices.Contexts.Files.Repositories;
using ClassifiedsApi.AppServices.Contexts.Files.Services;
using ClassifiedsApi.AppServices.Contexts.Users.Services;
using ClassifiedsApi.Contracts.Contexts.Files;

namespace ClassifiedsApi.AppServices.Contexts.AdvertImages.Services;

/// <inheritdoc cref="IAdvertImageService"/>
public class AdvertImageService : ServiceBase, IAdvertImageService
{
    private readonly IAdvertImageRepository _advertImageRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IUserAccessVerifier _userAccessVerifier;
    private readonly IFileVerifier _fileVerifier;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertImageService"/>.
    /// </summary>
    /// <param name="advertImageRepository">Репозиторий фотографий объявлений <see cref="IAdvertImageRepository"/>.</param>
    /// <param name="fileRepository">Репозиторий файлов <see cref="IFileRepository"/>.</param>
    /// <param name="userAccessVerifier">Верификатор прав пользователя <see cref="IUserAccessVerifier"/>.</param>
    /// <param name="fileVerifier">Верификатор файлов <see cref="IFileVerifier"/>.</param>
    public AdvertImageService(
        IAdvertImageRepository advertImageRepository,
        IFileRepository fileRepository,
        IUserAccessVerifier userAccessVerifier,
        IFileVerifier fileVerifier)
    {
        _advertImageRepository = advertImageRepository;
        _userAccessVerifier = userAccessVerifier;
        _fileRepository = fileRepository;
        _fileVerifier = fileVerifier;
    }
    
    /// <inheritdoc />
    public async Task<Guid> UploadAsync(Guid userId, Guid advertId, FileUpload imageUpload, CancellationToken token)
    {
        await _userAccessVerifier.VerifyAdvertAccessAndThrowAsync(userId, advertId, token);
        _fileVerifier.VerifyImageContentTypeAndThrow(imageUpload.ContentType);
        
        using var scope = CreateTransactionScope();
        var imageId = await _fileRepository.UploadAsync(imageUpload, token);
        await _advertImageRepository.AddAsync(advertId, imageId, token);
        scope.Complete();
        return imageId;
    }
    
    /// <inheritdoc />
    public async Task DeleteAsync(Guid userId, Guid advertId, Guid imageId, CancellationToken token)
    {
        await _userAccessVerifier.VerifyAdvertAccessAndThrowAsync(userId, advertId, token);
        
        using var scope = CreateTransactionScope();
        await _advertImageRepository.DeleteAsync(advertId, imageId, token);
        await _fileRepository.DeleteAsync(imageId, token);
        scope.Complete();
    }
    
    /// <inheritdoc />
    public async Task DeleteByAdvertIdAsync(Guid advertId, CancellationToken token)
    {
        using var scope = CreateTransactionScope();
        var imageIds = await _advertImageRepository.DeleteByAdvertIdAsync(advertId, token);
        await _fileRepository.DeleteRangeAsync(imageIds, token);
        scope.Complete();
    }
}