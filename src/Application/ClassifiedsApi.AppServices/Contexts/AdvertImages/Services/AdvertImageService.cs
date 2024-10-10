using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.AdvertImages.Repositories;
using ClassifiedsApi.AppServices.Contexts.Files.Services;
using ClassifiedsApi.Contracts.Contexts.AdvertImages;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.AdvertImages.Services;

/// <inheritdoc />
public class AdvertImageService : IAdvertImageService
{
    private readonly IFileService _fileService;
    private readonly IAdvertImageRepository _advertImageRepository;
    
    private readonly IValidator<AdvertRequest> _advertRequestValidator;
    private readonly IValidator<AdvertImageUploadRequest> _imageUploadRequestValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertImageService"/>.
    /// </summary>
    /// <param name="fileService">Сервис файлов <see cref="IFileService"/>.</param>
    /// <param name="advertImageRepository">Репозиторий фотографий объявлений <see cref="IAdvertImageRepository"/>.</param>
    /// <param name="advertRequestValidator">Валидатор модели пользовательского запроса объявления.</param>
    /// <param name="imageUploadRequestValidator">Валидатор модели пользовательского запроса на добавление фотографии объявления.</param>
    public AdvertImageService(
        IFileService fileService, 
        IAdvertImageRepository advertImageRepository, 
        IValidator<AdvertRequest> advertRequestValidator,
        IValidator<AdvertImageUploadRequest> imageUploadRequestValidator)
    {
        _fileService = fileService;
        _advertImageRepository = advertImageRepository;
        _advertRequestValidator = advertRequestValidator;
        _imageUploadRequestValidator = imageUploadRequestValidator;
    }

    /// <inheritdoc />
    public async Task<string> UploadAsync(AdvertImageUploadRequest imageUploadRequest, CancellationToken token)
    {
        await _imageUploadRequestValidator.ValidateAndThrowAsync(imageUploadRequest, token);
        var imageId = await _fileService.UploadAsync(imageUploadRequest.ImageUpload, token);
        await _advertImageRepository.AddAsync(imageUploadRequest.AdvertId, imageId, token);
        return imageId;
    }
    
    /// <inheritdoc />
    public async Task DeleteAsync(AdvertImageDeleteRequest imageDeleteRequest, CancellationToken token)
    {
        await _advertRequestValidator.ValidateAndThrowAsync(imageDeleteRequest, token);
        await _advertImageRepository.DeleteAsync(imageDeleteRequest.AdvertId, imageDeleteRequest.ImageId, token);
        await _fileService.DeleteAsync(imageDeleteRequest.ImageId, token);
    }
}