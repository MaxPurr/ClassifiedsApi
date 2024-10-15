using System.Linq;
using ClassifiedsApi.AppServices.Exceptions.Files;

namespace ClassifiedsApi.AppServices.Contexts.Files.Validators;

/// <inheritdoc />
public class FileValidator : IFileValidator
{
    private static readonly string[] ValidImageContentTypes = ["image/jpeg", "image/png"];
    
    /// <inheritdoc />
    public void ValidateImageContentTypeAndThrow(string contentType)
    {
        if (!ValidImageContentTypes.Contains(contentType))
        {
            throw new InvalidImageContentTypeException();
        }
    }
}