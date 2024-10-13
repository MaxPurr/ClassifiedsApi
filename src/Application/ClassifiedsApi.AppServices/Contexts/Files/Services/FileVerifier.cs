using System.Linq;
using ClassifiedsApi.AppServices.Exceptions.Files;

namespace ClassifiedsApi.AppServices.Contexts.Files.Services;

/// <inheritdoc />
public class FileVerifier : IFileVerifier
{
    private static readonly string[] ValidImageContentTypes = ["image/jpeg", "image/png"];
    
    /// <inheritdoc />
    public void VerifyImageContentTypeAndThrow(string contentType)
    {
        if (!ValidImageContentTypes.Contains(contentType))
        {
            throw new InvalidImageContentTypeException();
        }
    }
}