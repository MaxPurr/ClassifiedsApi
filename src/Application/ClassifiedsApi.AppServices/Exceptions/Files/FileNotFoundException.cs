using ClassifiedsApi.AppServices.Exceptions.Common;

namespace ClassifiedsApi.AppServices.Exceptions.Files;

public class FileNotFoundException : EntityNotFoundException
{
    public FileNotFoundException() : base("Файл не был найден.") { }
}