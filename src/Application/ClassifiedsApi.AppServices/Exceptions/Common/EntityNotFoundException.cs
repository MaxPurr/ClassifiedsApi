using System;

namespace ClassifiedsApi.AppServices.Exceptions.Common;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException() : this("Сущность не была найдена.") { }
    public EntityNotFoundException(string message) : base(message) { }
}