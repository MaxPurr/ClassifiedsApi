using System;

namespace ClassifiedsApi.Domain.Base;

public interface ISqlEntity
{
    /// <summary>
    /// Идентификатор сущности.
    /// </summary>
    Guid Id { get; }
}