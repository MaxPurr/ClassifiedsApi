using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Common.Validators;
using ClassifiedsApi.AppServices.Contexts.AdvertImages.Repositories;
using ClassifiedsApi.AppServices.Contexts.Files.Services;
using ClassifiedsApi.AppServices.Contexts.Users.Services;
using ClassifiedsApi.Contracts.Contexts.Files;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.AdvertImages.Services;

/// <inheritdoc />
public class AdvertImageService : IAdvertImageService
{
    private readonly IFileService _fileService;
    private readonly IAdvertImageRepository _advertImageRepository;
    private readonly IUserAccessVerifier _userAccessVerifier;

    private readonly IValidator<FileUpload> _imageContentTypeValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertImageService"/>.
    /// </summary>
    /// <param name="fileService">Сервис файлов <see cref="IFileService"/>.</param>
    /// <param name="advertImageRepository">Репозиторий фотографий объявлений <see cref="IAdvertImageRepository"/>.</param>
    /// <param name="userAccessVerifier">Верификатор прав пользователя <see cref="IUserAccessVerifier"/>.</param>
    public AdvertImageService(
        IFileService fileService, 
        IAdvertImageRepository advertImageRepository,
        IUserAccessVerifier userAccessVerifier)
    {
        _fileService = fileService;
        _advertImageRepository = advertImageRepository;
        _userAccessVerifier = userAccessVerifier;
        _imageContentTypeValidator = new ImageContentTypeValidator();
    }
    
    /// <inheritdoc />
    public async Task<string> UploadAsync(Guid userId, Guid advertId, FileUpload imageUpload, CancellationToken token)
    {
        await _userAccessVerifier.VerifyAdvertAccessAndThrowAsync(userId, advertId, token);
        _imageContentTypeValidator.ValidateAndThrow(imageUpload);
        var imageId = await _fileService.UploadAsync(imageUpload, token);
        await _advertImageRepository.AddAsync(advertId, imageId, token);
        return imageId;
    }
    
    /// <inheritdoc />
    public async Task DeleteAsync(Guid userId, Guid advertId, string imageId, CancellationToken token)
    {
        await _userAccessVerifier.VerifyAdvertAccessAndThrowAsync(userId, advertId, token);
        await _advertImageRepository.DeleteAsync(advertId, imageId, token);
        await _fileService.DeleteAsync(imageId, token);
    }

    /// <inheritdoc />
    public async Task DeleteAllAsync(Guid advertId, CancellationToken token)
    {
        var ids = await _advertImageRepository.GetAllAsync(advertId, token);
        foreach (var id in ids)
        {
            await _fileService.DeleteAsync(id, token);
        }
    }
}