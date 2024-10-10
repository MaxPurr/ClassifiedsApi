using ClassifiedsApi.AppServices.Common.Validators;
using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.AppServices.Contexts.Adverts.Validators;
using ClassifiedsApi.Contracts.Contexts.AdvertImages;

namespace ClassifiedsApi.AppServices.Contexts.AdvertImages.Validators;

/// <summary>
/// Валидатор модели пользовательского запроса на добавление фотографии объявления.
/// </summary>
public class AdvertImageUploadRequestValidator : AdvertRequestValidator<AdvertImageUploadRequest>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertImageUploadRequestValidator"/>.
    /// </summary>
    /// <param name="advertRepository"></param>
    public AdvertImageUploadRequestValidator(IAdvertRepository advertRepository) : base(advertRepository)
    {
        RuleFor(request => request.ImageUpload)
            .SetValidator(new ImageContentTypeValidator());
    }
}